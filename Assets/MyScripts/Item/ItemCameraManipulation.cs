using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemCameraManipulation : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private PlayerInventory playerInventory;
        private Collider myCollider;
        private bool isPickedUp;
        void SetInit()
        {
            itemMaster = GetComponent<ItemMaster>();
            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            myCollider = GetComponent<Collider>();
        }
        private void Start()
        {
            SetInit();
        }
        private void OnEnable()
        {
            SetInit();
            itemMaster.EventObjectPickup += StartCheck;
            if (isPickedUp)
            {
                StartCoroutine(IEStartCheck());
                playerInventory.CameraOnOff(false, false);
            }
            //Debug.Log("IsPicked UP " + isPickedUp);
            itemMaster.EventObjectThrow += FinishCheck;
        }
        private void OnDisable()
        {
            itemMaster.EventObjectPickup -= StartCheck;
            itemMaster.EventObjectThrow -= FinishCheck;
        }
        void StartCheck()
        {
            isPickedUp = true;
            //Debug.Log("Pickup");
        }
        IEnumerator IEStartCheck()
        {
            yield return new WaitForEndOfFrame();
            myCollider.enabled = true;
            myCollider.isTrigger = true;
        }
        void FinishCheck()
        {
            myCollider.isTrigger = false;
            isPickedUp = false;
        }
        private void OnTriggerStay(Collider other)
        {
            if (playerInventory.shouldCheckCamera)
            {
                //Debug.Log("Entered other  " + other);
                playerInventory.CameraOnOff(true, false);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (playerInventory.shouldCheckCamera)
            {
                //Debug.Log("Exit other  " + other);
                playerInventory.CameraOnOff(false, false);
            }
        }
    }
}