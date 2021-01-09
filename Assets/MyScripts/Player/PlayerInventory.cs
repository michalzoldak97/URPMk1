using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

 namespace U1
{
    public class PlayerInventory : MonoBehaviour
    {

        [SerializeField] GameObject canvasInventory;
        [SerializeField] GameObject inventoryButton;
        [SerializeField] GameObject itemCamera;
        [SerializeField] Transform canvasContent;
        [SerializeField] Transform FPSCameraTransform;
        private List<Transform> inventoryItems = new List<Transform>();
        private int currentItem;
        private Transform currentItemTransform;
        private string itemTag = "Item";
        private PlayerMaster playerMaster;
        public bool shouldCameraOn { get; private set; }
        public bool shouldCheckCamera { get; set; }
        private void Start()
        {
            StartCoroutine(StartUpdate());
        }

        private void OnEnable()
        {
            playerMaster = GetComponent<PlayerMaster>();
            playerMaster.EventInventoryChanged += ClearInventoryUI;
            playerMaster.EventInventoryChanged += UpdateInventoryUI;
        }
        private void OnDisable()
        {
            playerMaster.EventInventoryChanged -= UpdateInventoryUI;
            playerMaster.EventInventoryChanged -= ClearInventoryUI;
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                CanvasOnOf();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Throw();
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                CursorOnOff();
            }
        }

        void UpdateInventoryUI()
        {
            currentItem = 0;
            inventoryItems.Clear();
            inventoryItems.TrimExcess();
            ClearInventoryUI();
            foreach (Transform child in FPSCameraTransform)
            {
                if(child.CompareTag(itemTag))
                {
                    AddItem(child, currentItem);
                    //Debug.Log("Add called from update: " + child.gameObject.name);
                    currentItem++;
                }
                if (inventoryItems.Count == 0)
                    CameraOnOff(false, true);
                //else if(inventoryItems.Count != 0)
                //itemCamera.SetActive(true);
            }
        }
        void AddItem(Transform itemToAdd, int index)
        {
            inventoryItems.Add(itemToAdd);
            GameObject Ibutton = Instantiate(inventoryButton, canvasContent);
            //Instantiate(inventoryButton, canvasContent); // stats button, to develop
            Ibutton.GetComponentInChildren<TMP_Text>().text = itemToAdd.name;
            Ibutton.GetComponent<Button>().onClick.AddListener(delegate { ActivateItem(index); });
        }
        void ActivateItem(int index)
        {
            //Debug.Log("Activate inventory: " + index + " inventory count  " + inventoryItems.Count);
            DeactivateInventory();
            CameraOnOff(true, true);
            inventoryItems[index].gameObject.SetActive(true);
            currentItemTransform = inventoryItems[index];
        }
        void ClearInventoryUI()
        {
            foreach(Transform childTransform in canvasContent)
            {
                Destroy(childTransform.gameObject);
            }
        }
        void DeactivateInventory()
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (!inventoryItems[i].gameObject.GetComponent<ItemMaster>().shouldStayActive)
                {
                    inventoryItems[i].gameObject.SetActive(false);
                }
            }
            currentItemTransform = null;
        }

        void CanvasOnOf()
        {
            if(canvasInventory.activeSelf)
            {
                playerMaster.CallEventControllerFreeze(false);
                playerMaster.CallEventCursorOnOff(false);
                playerMaster.isInventoryOn = false;
                canvasInventory.SetActive(false);
            }
            else
            {
                playerMaster.CallEventControllerFreeze(true);
                playerMaster.CallEventCursorOnOff(true);
                playerMaster.isInventoryOn = true;
                canvasInventory.SetActive(true);
            }
        }
        void CursorOnOff()
        {
            playerMaster.CallEventCursorOnOff(Cursor.lockState == CursorLockMode.Locked);
            playerMaster.CallEventControllerFreeze(!(Cursor.lockState == CursorLockMode.Locked));
            playerMaster.isInventoryOn = !(Cursor.lockState == CursorLockMode.Locked);
        }
        void Throw()
        {
            if (currentItemTransform != null && currentItemTransform.gameObject.activeSelf)
            {
                if (currentItemTransform.GetComponent<ItemMaster>() != null)
                {
                    currentItemTransform.GetComponent<ItemMaster>().CallEventObjectThrowRequest();
                    currentItemTransform = null;
                }
            }
        }

        public void EmptyHands()
        {
            if(currentItemTransform != null)
                currentItemTransform = null;
            DeactivateInventory();
            CameraOnOff(false, true);
        }
        public void CameraOnOff(bool toState, bool changeState)
        {
            //Debug.Log("Camera inventory called: " + toState + "  " + changeState);
            itemCamera.SetActive(toState);
            if (changeState) 
            {
                shouldCameraOn = toState;
                shouldCheckCamera = toState;
            }
        }
        IEnumerator StartUpdate()
        {
            yield return new WaitForSeconds(0.1f);
            currentItem = 0;
            inventoryItems.Clear();
            inventoryItems.TrimExcess();
            ClearInventoryUI();
            foreach (Transform child in FPSCameraTransform)
            {
                if (child.CompareTag(itemTag))
                {
                    child.gameObject.GetComponent<ItemMaster>().CallEventPickupRequested(FPSCameraTransform);
                }
            }
        }
    }
}