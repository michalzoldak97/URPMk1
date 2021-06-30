using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class PlayerInventoryMaster : MonoBehaviour
    {
        [SerializeField] private GameObject itemCamera;
        public void ItemCameraChangeState(bool toState)
        {
            itemCamera.SetActive(toState);
        }
        private Transform itemsParentTransform;
        public Transform GetItemsParentTransform() { return itemsParentTransform; }
        public Transform selectedItem { get; private set; }
        public void SetSelectedItem(Transform toSet) { selectedItem = toSet; }
        private List<Transform> itemsOnPlayer = new List<Transform>();
        public List<Transform> GetItemsOnPlayer() { return itemsOnPlayer; }
        public void AddItemToListOnPlayer(Transform toAdd) 
        {
            if (!itemsOnPlayer.Contains(toAdd))
                itemsOnPlayer.Add(toAdd);
        }
        public void RemoveItemFromListOnPlayer(Transform toRemove)
        {
            if (itemsOnPlayer.Contains(toRemove))
                itemsOnPlayer.Remove(toRemove);
        }
        public bool CheckIfItemOnPlayer(Transform toCheck)
        {
            return itemsOnPlayer.Contains(toCheck);
        }
        private void Start()
        {
            itemsParentTransform = Camera.main.transform;
        }
        public delegate void InventoryEventhandler(Transform itemTransform);
        public event InventoryEventhandler EventItemPlaceRequested;
        public event InventoryEventhandler EventItemPlaced;
        public event InventoryEventhandler EventActivateItem;
        public event InventoryEventhandler EventRemoveItem;
        public event InventoryEventhandler EventReloadUI;
        public void CallEventItemPlaceRequested(Transform itemTransform)
        {
            if (EventItemPlaceRequested != null)
                EventItemPlaceRequested(itemTransform);
        }
        public void CallEventItemPlaced(Transform itemTransform)
        {
            if (EventItemPlaced != null)
                EventItemPlaced(itemTransform);
        }
        public void CallEventActivateItem(Transform itemTransform)
        {
            if (EventActivateItem != null)
                EventActivateItem(itemTransform);
        }
        public void CallEventRemoveItem(Transform itemTransform)
        {
            if (EventRemoveItem != null)
                EventRemoveItem(itemTransform);
        }
        public void CallEventReloadUI(Transform itemTransform)
        {
            if (EventReloadUI != null)
                EventReloadUI(itemTransform);
        }
    }
}
