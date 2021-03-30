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
        private SceneStartManager startManager;

        private void OnEnable()
        {
            startManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            startManager.EventPOUTransactionResult += UpdateTransactionStatus;
        }
        private void OnDisable()
        {
            startManager.EventPOUTransactionResult -= UpdateTransactionStatus;
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
            playerCoins.text = "Coins: " + startManager.playerCoins.ToString();
            playerExperience.text = "EXP: " + startManager.playerExperience.ToString();
        }
        private void SetUpPOButtons()
        {
            foreach(Transform child in PObuttonsTransform)
            {
                Destroy(child.gameObject);
            }
            PlaceableObject[] placeableObjects = startManager.GetPlaceableObjects();
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
                            startManager.SetPlayerMaxSlots(startManager.playerMaxSlots + Int32.Parse(extensionInfo[1]));
                            placeableObjects[j].isAvailable = false;
                        }
                        else
                        {
                            GameObject POPanel = Instantiate(poShopPanel, PObuttonsTransform);
                            POPanel.GetComponent<POShopButton>().SetUpPOButton(j, placeableObjects[j], this);
                            if (startManager.playerExperience < placeableObjects[j].experiencePrice || startManager.currLevel < placeableObjects[j].levelFromAvailable)
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
            PlaceableObject[] placeableObjects = startManager.GetPlaceableObjects();
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
            PlaceableObject objToValid = startManager.GetPlaceableObjects()[idx];
            if (CountFreeSlots() > 0 && objToValid.numOfOwnedObjects > 0 && objToValid.numOfObjOnStack < objToValid.numOfOwnedObjects)
            {
                startManager.GetPlaceableObjects()[idx].isAddedToStack = true;
                startManager.GetPlaceableObjects()[idx].numOfObjOnStack++;
                SetUpPOButtons();
                SetUpSlotUI();
                SetUpSelectedPanel();
            }
        }

        private int CountFreeSlots()
        {
            int numOfPO = startManager.GetPlaceableObjects().Length;
            int slotsTaken = 0;
            for (int i = 0; i < numOfPO; i++)
            {
                if (startManager.GetPlaceableObjects()[i].isAddedToStack)
                {
                    slotsTaken += startManager.GetPlaceableObjects()[i].numOfObjOnStack;
                }
            }
            return (startManager.playerMaxSlots - slotsTaken);
        }
        public void RemoveButtonFromStack(int idx)
        {
            startManager.GetPlaceableObjects()[idx].numOfObjOnStack--;
            if(startManager.GetPlaceableObjects()[idx].numOfObjOnStack < 1)
                startManager.GetPlaceableObjects()[idx].isAddedToStack = false;
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
                coinsBeforeTransaction = startManager.playerCoins;
                numOwnedBeforeTransaction = startManager.GetPlaceableObjects()[index].numOfOwnedObjects;
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
                    startManager.SetPlayerCoins(coinsBeforeTransaction);
                    startManager.GetPlaceableObjects()[index].numOfOwnedObjects = numOwnedBeforeTransaction;
                    OnOffCursor(true);
                    isBuyingInProgress = false;
                }
            }
        }
        private bool AttemptToBuyPO(int index)
        {
            PlaceableObject objToValidate = startManager.GetPlaceableObjects()[index];
            if (startManager.playerCoins >= objToValidate.coinsPrice && startManager.playerExperience >= objToValidate.experiencePrice 
                && objToValidate.isAvailable && objToValidate.numOfOwnedObjects < objToValidate.maxNumOfOwnedObjects && startManager.currLevel >= objToValidate.levelFromAvailable)
            {
                startManager.SetPlayerCoins(startManager.playerCoins - objToValidate.coinsPrice);
                startManager.GetPlaceableObjects()[index].numOfOwnedObjects++;
                startManager.CallEventPOUBuy("");
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
                startManager.SetPlayerCoins(coinsBeforeTransaction);
                startManager.GetPlaceableObjects()[index].numOfOwnedObjects = numOwnedBeforeTransaction;
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
            infoPanel.GetComponent<ShopInfoPanel>().SetInfoPanel(startManager.GetPlaceableObjects()[POindex].objImage, startManager.GetPlaceableObjects()[POindex].objectInfo);
        }
        public void OnInfoButtonExit(int POindex)
        {
            infoPanel.SetActive(false);
        }
        public void NextLevel()
        {
            startManager.ChangeScene(3);
        }
        public void PreviousLevel()
        {
            startManager.ChangeScene(1);
        }
    }
}
