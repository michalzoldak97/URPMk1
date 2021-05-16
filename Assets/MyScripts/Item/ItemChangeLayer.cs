using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemChangeLayer : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private int originalLayer;

        void SetInit()
        {
            itemMaster = GetComponent<ItemMaster>();
            originalLayer = gameObject.layer;
        }

        private void OnEnable()
        {
            SetInit();
            itemMaster.EventObjectPickup += ChangeOnPickup;
            itemMaster.EventObjectThrow += ChangeOnThrow;
        }
        private void OnDisable()
        {
            itemMaster.EventObjectPickup -= ChangeOnPickup;
            itemMaster.EventObjectThrow -= ChangeOnThrow;
        }

        void ChangeOnPickup()
        {
            gameObject.layer = itemMaster.GetItemSO().toLayer;
            foreach(Transform child in transform)
            {
                child.gameObject.layer = itemMaster.GetItemSO().toLayer;
            }
        }
        void ChangeOnThrow()
        {
            gameObject.layer = originalLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = originalLayer;
            }
        }
    }
}
