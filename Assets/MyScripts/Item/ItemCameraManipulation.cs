using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemCameraManipulation : MonoBehaviour
    {
        private bool shouldInformCamera;
        public void SetShouldInformCamera(bool toSet) { shouldInformCamera = toSet; }
        private ItemMaster itemMaster;
        
        void SetInit()
        {
            itemMaster = GetComponent<ItemMaster>();
        }
        private void Start()
        {
            SetInit();
        }
        private void OnEnable()
        {
            SetInit();
            if (itemMaster.isOnPlayer)
                shouldInformCamera = true;
            itemMaster.EventObjectThrow += OnThrow;
        }
        private void OnDisable()
        {
            if (itemMaster.isOnPlayer)
                itemMaster.playerTransform.GetComponent<PlayerInventoryMaster>().ItemCameraChangeState(false);
            shouldInformCamera = false;
            itemMaster.EventObjectThrow -= OnThrow;
        }
        private void OnTriggerStay(Collider other)
        {
            if (shouldInformCamera)
            {
                itemMaster.playerTransform.GetComponent<PlayerInventoryMaster>().ItemCameraChangeState(true);
                shouldInformCamera = false;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(itemMaster.playerTransform != null)
                itemMaster.playerTransform.GetComponent<PlayerInventoryMaster>().ItemCameraChangeState(false);
            shouldInformCamera = true;
        }
        private void OnThrow()
        {
            itemMaster.playerTransform.GetComponent<PlayerInventoryMaster>().ItemCameraChangeState(false);
            shouldInformCamera = false;
        }
    }
}