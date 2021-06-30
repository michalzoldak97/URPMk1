using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class PlayerInventoryActions : MonoBehaviour
    {
        private PlayerInventoryMaster inventoryMaster;
        private void Start()
        {
            inventoryMaster = GetComponent<PlayerInventoryMaster>();
        }
        private void OnEnable()
        {
            inventoryMaster = GetComponent<PlayerInventoryMaster>();
            inventoryMaster.EventItemPlaceRequested += PlaceItemOnInventory;
            inventoryMaster.EventActivateItem += ActivateItem;
            inventoryMaster.EventRemoveItem += RemoveItem;
        }
        private void OnDisable()
        {
            inventoryMaster.EventItemPlaceRequested -= PlaceItemOnInventory;
            inventoryMaster.EventActivateItem -= ActivateItem;
            inventoryMaster.EventRemoveItem -= RemoveItem;
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F) && Time.timeScale > 0 && inventoryMaster.selectedItem != null)
            {
                inventoryMaster.selectedItem.GetComponent<ItemMaster>().CallEventObjectThrowRequest();
            }
        }
        private void PlaceItemOnInventory(Transform toPlace)
        {
            toPlace.parent = inventoryMaster.GetItemsParentTransform();
            inventoryMaster.AddItemToListOnPlayer(toPlace);
            inventoryMaster.CallEventItemPlaced(toPlace);
            inventoryMaster.CallEventReloadUI(toPlace);
        }
        public void DeactivateAllItems()
        {
            List<Transform> myItems = inventoryMaster.GetItemsOnPlayer();
            for (int i = 0; i < myItems.Count; i++)
            {
                if (myItems[i].gameObject.activeSelf && !myItems[i].GetComponent<ItemMaster>().shouldStayActive)
                {
                    myItems[i].gameObject.SetActive(false);
                }
            }
            inventoryMaster.SetSelectedItem(null);
        }
        private void ActivateItem(Transform toActivate)
        {
            DeactivateAllItems();
            toActivate.gameObject.SetActive(true);
            inventoryMaster.SetSelectedItem(toActivate);
        }
        private void RemoveItem(Transform toRemove)
        {
            inventoryMaster.SetSelectedItem(null);
            inventoryMaster.RemoveItemFromListOnPlayer(toRemove);
            inventoryMaster.CallEventReloadUI(toRemove);
        }
    }
}
