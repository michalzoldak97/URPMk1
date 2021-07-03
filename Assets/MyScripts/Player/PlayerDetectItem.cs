using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class PlayerDetectItem : MonoBehaviour
    {
        //private float baseCheckRate = 0.1f; 
        [SerializeField] private LayerMask itemLayer;
        private Transform fpsCameraTransform, itemInRange;
        private float nextCheck;
        private bool isItemInRange;
        private PlayerInventoryMaster inventoryMaster;

        private void Start()
        {
            fpsCameraTransform = Camera.main.transform;
            inventoryMaster = GetComponent<PlayerInventoryMaster>();
        }
        private void Update()
        {
            ManageItemSearch();
            CheckForItemPickUpAttempt();
        }
        private void ManageItemSearch()
        {
            if(!isItemInRange && Time.time > nextCheck)
            {
                CheckForItemInRange();
                nextCheck = Time.time + 0.1f;
            }
            else if(isItemInRange)
            {
                CheckForItemInRange();
            }
        }
        private void CheckForItemInRange()
        {
            RaycastHit hit;
            if(Physics.SphereCast(fpsCameraTransform.position, 0.5f, fpsCameraTransform.forward, out hit, 3, itemLayer))
            {
                itemInRange = hit.transform;
                isItemInRange = true;
            }
            else
            {
                isItemInRange = false;
                itemInRange = null;
            }
        }
        private void CheckForItemPickUpAttempt()
        {
            if(isItemInRange && Input.GetKeyDown(KeyCode.E) && Time.timeScale > 0 && !inventoryMaster.CheckIfItemOnPlayer(itemInRange) 
                && itemInRange.GetComponent<ItemMaster>() != null)
            {
                    itemInRange.GetComponent<ItemMaster>().CallEventPickupRequested(fpsCameraTransform);
            }
        }
        private void OnGUI()
        {
            if (itemInRange && itemInRange != null)
            {
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2, 200, 50), itemInRange.name + " " + 
                    itemInRange.GetComponent<ItemMaster>().GetTextToDisplayOnGUI());
            }
        }
    }
}
