using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace U1
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private Transform PObuttonsTransform, selectedPOTransform;
        [SerializeField] private GameObject infoPanel, poShopPanel, selectedPOpanel, transactionInfoPanel, buyConfirmCanvas;
        [SerializeField] private TMP_Text slotsAvailable, playerCoins, playerExperience;
        private int coinsBeforeTransaction, numOwnedBeforeTransaction;
        private bool isBuyingInProgress;
        private string transactionStatus = "waiting";
        private string errorText;
        private SceneStartManager sceneStartManager;

        private void OnEnable()
        {
            sceneStartManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            sceneStartManager.EventPOUTransactionResult += UpdateTransactionStatus;
        }
        private void OnDisable()
        {
            sceneStartManager.EventPOUTransactionResult -= UpdateTransactionStatus;
        }
        private void Start()
        {
            SetUpPOButtons();
            SetUpSlotUI();
            SetUpSelectedPanel();
            infoPanel.SetActive(false);
        }
        private void SetUpSlotUI()
        {
            slotsAvailable.text = "Slots available: " + CountFreeSlots().ToString();
            playerCoins.text = "Coins: " + sceneStartManager.playerCoins.ToString();
            playerExperience.text = "EXP: " + sceneStartManager.playerExperience.ToString();
        }
        private void SetUpPOButtons()
        {
            foreach(Transform child in PObuttonsTransform)
            {
                Destroy(child.gameObject);
            }
            PlaceableObject[] placeableObjects = sceneStartManager.GetPlaceableObjects();
            int POlength = placeableObjects.Length;
            for (int i = 0; i < PlaceableObject.numOfObjTypes; i++)
            {
                for (int j = 0; j < POlength; j++)
                {
                    if (placeableObjects[j].isAvailable && placeableObjects[j].objType == i)
                    {
                        if (placeableObjects[j].numOfOwnedObjects > 0 && placeableObjects[j].objectName == "Slot Extension")
                        {
                            string[] extensionInfo = placeableObjects[j].objectInfo.Split(' ');
                            sceneStartManager.SetPlayerMaxSlots(sceneStartManager.playerMaxSlots + Int32.Parse(extensionInfo[1]));
                            placeableObjects[j].isAvailable = false;
                        }
                        else
                        {
                            GameObject POPanel = Instantiate(poShopPanel, PObuttonsTransform);
                            POPanel.GetComponent<POShopButton>().SetUpPOButton(j, placeableObjects[j], this);
                            if (sceneStartManager.playerExperience < placeableObjects[j].experiencePrice || sceneStartManager.currLevel < placeableObjects[j].levelFromAvailable)
                            {
                                POPanel.GetComponent<POShopButton>().SetAvailabilityImage(true);
                            }
                            else
                                POPanel.GetComponent<POShopButton>().SetAvailabilityImage(false);
                        }
                    }
                }
            }
        }

        private void SetUpSelectedPanel()
        {
            foreach (Transform child in selectedPOTransform)
            {
                Destroy(child.gameObject);
            }
            PlaceableObject[] placeableObjects = sceneStartManager.GetPlaceableObjects();
            int POlength = placeableObjects.Length;
            for (int i = 0; i < PlaceableObject.numOfObjTypes; i++)
            {
                for (int j = 0; j < POlength; j++)
                {
                    if (placeableObjects[j].isAvailable && placeableObjects[j].objType == i 
                        && placeableObjects[j].isAddedToStack && placeableObjects[j].numOfObjOnStack > 0)
                    {
                        for (int k = 0; k < placeableObjects[j].numOfObjOnStack; k++)
                        {
                            GameObject SOPanel = Instantiate(selectedPOpanel, selectedPOTransform);
                            SOPanel.GetComponent<POSelectedButton>().SetUpButton(placeableObjects[j].objIcon, placeableObjects[j].objectName, j, this);
                        }
                    }
                }
            }
        }
        public void AddButtonToStack(int idx)
        {
            PlaceableObject objToValid = sceneStartManager.GetPlaceableObjects()[idx];
            Debug.Log("Add called for idx: " + idx + " there are " + CountFreeSlots() + " free and numOfOwned is > 0: " + objToValid.numOfOwnedObjects + " and on stack: " + objToValid.numOfObjOnStack);
            if (CountFreeSlots() > 0 && objToValid.numOfOwnedObjects > 0 && objToValid.numOfObjOnStack < objToValid.numOfOwnedObjects)
            {
                sceneStartManager.GetPlaceableObjects()[idx].isAddedToStack = true;
                sceneStartManager.GetPlaceableObjects()[idx].numOfObjOnStack++;
                SetUpPOButtons();
                SetUpSlotUI();
                SetUpSelectedPanel();
            }
        }

        private int CountFreeSlots()
        {
            int numOfPO = sceneStartManager.GetPlaceableObjects().Length;
            int slotsTaken = 0;
            for (int i = 0; i < numOfPO; i++)
            {
                if (sceneStartManager.GetPlaceableObjects()[i].isAddedToStack)
                {
                    slotsTaken += sceneStartManager.GetPlaceableObjects()[i].numOfObjOnStack;
                }
            }
            return (sceneStartManager.playerMaxSlots - slotsTaken);
        }
        public void RemoveButtonFromStack(int idx)
        {
            sceneStartManager.GetPlaceableObjects()[idx].numOfObjOnStack--;
            if(sceneStartManager.GetPlaceableObjects()[idx].numOfObjOnStack < 1)
                sceneStartManager.GetPlaceableObjects()[idx].isAddedToStack = false;
            SetUpPOButtons();
            SetUpSlotUI();
            SetUpSelectedPanel();
        }
        
        public void AttemptBuyPO(int index)
        {
            buyConfirmCanvas.SetActive(true);
            buyConfirmCanvas.GetComponent<ConfirmBuy>().SetPOIndex(index);
        }
        public void BuyPO(int index)
        {
            if (!isBuyingInProgress)
            {
                isBuyingInProgress = true;
                OnOffCursor(false);
                coinsBeforeTransaction = sceneStartManager.playerCoins;
                numOwnedBeforeTransaction = sceneStartManager.GetPlaceableObjects()[index].numOfOwnedObjects;
                if (AttemptToBuyPO(index))
                {
                    StartCoroutine(WaitForServerOnBuyResponse(index));
                    buyConfirmCanvas.SetActive(false);
                }
                else
                {
                    Debug.Log("Sth wet wronk");
                    errorText = "Can't perform transaction";
                    buyConfirmCanvas.GetComponent<ConfirmBuy>().StartDisplayErrorMessage(errorText);
                    sceneStartManager.SetPlayerCoins(coinsBeforeTransaction);
                    sceneStartManager.GetPlaceableObjects()[index].numOfOwnedObjects = numOwnedBeforeTransaction;
                    OnOffCursor(true);
                    isBuyingInProgress = false;
                }
            }
        }
        private bool AttemptToBuyPO(int index)
        {
            PlaceableObject objToValidate = sceneStartManager.GetPlaceableObjects()[index];
            if (sceneStartManager.playerCoins >= objToValidate.coinsPrice && sceneStartManager.playerExperience >= objToValidate.experiencePrice 
                && objToValidate.isAvailable && objToValidate.numOfOwnedObjects < objToValidate.maxNumOfOwnedObjects && sceneStartManager.currLevel >= objToValidate.levelFromAvailable)
            {
                sceneStartManager.SetPlayerCoins(sceneStartManager.playerCoins - objToValidate.coinsPrice);
                sceneStartManager.GetPlaceableObjects()[index].numOfOwnedObjects++;
                sceneStartManager.CallEventPOUBuy("");
                return true;
            }
            else
                return false;
        }
        IEnumerator WaitForServerOnBuyResponse(int index)
        {
            transactionInfoPanel.SetActive(true);
            Color32 panelColor = transactionInfoPanel.GetComponent<Image>().color;
            transactionInfoPanel.GetComponentInChildren<TMP_Text>().text = "Transaction in progress\nPlease wait";
            for (int i = 0; i < 100; i++)
            {
                if (transactionStatus != "1")
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                }
                else
                {
                    break;
                }
            }
            if(transactionStatus != "1")
            {
                transactionInfoPanel.GetComponentInChildren<TMP_Text>().text = "Transaction failed";
                transactionInfoPanel.GetComponent<Image>().color = Color.red;
                yield return new WaitForSecondsRealtime(1f);
                sceneStartManager.SetPlayerCoins(coinsBeforeTransaction);
                sceneStartManager.GetPlaceableObjects()[index].numOfOwnedObjects = numOwnedBeforeTransaction;
            }
            else
            {
                transactionInfoPanel.GetComponentInChildren<TMP_Text>().text = "Transaction finished sucessfully";
                transactionInfoPanel.GetComponent<Image>().color = Color.green;
                yield return new WaitForSecondsRealtime(1f);
            }
            coinsBeforeTransaction = 0;
            numOwnedBeforeTransaction = 0;
            transactionStatus = "waiting";
            transactionInfoPanel.SetActive(false);
            transactionInfoPanel.GetComponent<Image>().color = panelColor;
            OnOffCursor(true);
            isBuyingInProgress = false;
            SetUpPOButtons();
            SetUpSlotUI();
        }
        private void OnOffCursor(bool toState)
        {
            if(toState)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
        private void UpdateTransactionStatus(string toSet)
        {
            transactionStatus = toSet;
        }

        public void OnInfoButtonEnter(int POindex, Transform infoButtonTransform)
        {
            infoPanel.SetActive(true);
            int x, y;
            if(Input.mousePosition.x <= Screen.width/2)
            {
                x = (int)(infoButtonTransform.position.x + (Screen.width / 10)*1.1f);
            }
            else
            {
                x = (int)infoButtonTransform.position.x - Screen.height / 10;
            }
            if(Input.mousePosition.y >= Screen.height / 2)
            {
                y = (int)infoButtonTransform.position.y - Screen.height / 3;
            }
            else
            {
                y = (int)infoButtonTransform.position.y + Screen.height / 3;
            }
            Vector3 infoPosition = infoPanel.transform.position;
            infoPosition.x = x; infoPosition.y = y;
            infoPanel.transform.position = infoPosition;
            infoPanel.GetComponent<ShopInfoPanel>().SetInfoPanel(sceneStartManager.GetPlaceableObjects()[POindex].objImage, sceneStartManager.GetPlaceableObjects()[POindex].objectInfo);
        }
        public void OnInfoButtonExit(int POindex)
        {
            infoPanel.SetActive(false);
        }
        public void NextLevel()
        {
            sceneStartManager.ChangeScene(3);
        }
        public void PreviousLevel()
        {
            sceneStartManager.ChangeScene(1);
        }
    }
}
