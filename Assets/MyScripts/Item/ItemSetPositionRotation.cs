using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemSetPositionRotation : MonoBehaviour
    {
        private ItemMaster itemMaster;
        void Start()
        {
            Initialize();
        }
        private void OnEnable()
        {
            Initialize();
            itemMaster.EventObjectPickup += SetPosition;
            itemMaster.EventObjectPickup += SetRotation;
        }
        private void OnDisable()
        {
            itemMaster.EventObjectPickup -= SetPosition;
            itemMaster.EventObjectPickup -= SetRotation;
        }
        void Initialize()
        {
            itemMaster = GetComponent<ItemMaster>();
            if(transform.root.CompareTag("Player"))
            {
                SetPosition();
                SetRotation();
            }
        }
        void SetPosition()
        {
            transform.localPosition = itemMaster.GetItemSO().posToSet;
        }
        void SetRotation()
        {
            transform.localRotation = Quaternion.Euler(itemMaster.GetItemSO().rotToSet);
        }
    }
}
