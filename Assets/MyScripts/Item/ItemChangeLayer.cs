using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemChangeLayer : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private int originalLayer;

        private void Start()
        {
            originalLayer = gameObject.layer;
        }
        private void OnEnable()
        {
            itemMaster = GetComponent<ItemMaster>();
            itemMaster.EventObjectPickup += ChangeOnPickup;
            itemMaster.EventObjectThrow += ChangeOnThrow;
        }
        private void OnDisable()
        {
            itemMaster.EventObjectPickup -= ChangeOnPickup;
            itemMaster.EventObjectThrow -= ChangeOnThrow;
        }

        private void ChangeOnPickup()
        {
            gameObject.layer = itemMaster.GetItemSO().toLayer;
            foreach(Transform child in transform)
            {
                child.gameObject.layer = itemMaster.GetItemSO().toLayer;
            }
        }
        private void ChangeOnThrow()
        {
            gameObject.layer = originalLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = originalLayer;
            }
        }
    }
}
