using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class ItemPickUp : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private void OnEnable()
        {
            itemMaster = GetComponent<ItemMaster>();
            itemMaster.EventPickupRequested += RequestAddToInventory;
        }

        private void OnDisable()
        {
            itemMaster.EventPickupRequested -= RequestAddToInventory;
        }
        private void RequestAddToInventory(Transform requestTransform)
        {
            if (!itemMaster.isInTransitionState && requestTransform.root.GetComponent<PlayerInventoryMaster>() != null)
            {
                itemMaster.SetIsInTransitionState(true);
                itemMaster.SetItemPhysics(false);
                requestTransform.root.GetComponent<PlayerInventoryMaster>().CallEventItemPlaceRequested(transform);
                itemMaster.CallEventObjectPickup();
            }
        }
    }
}
