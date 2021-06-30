using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemMaster : MonoBehaviour
    {
        [SerializeField] private string displayOnGui;
        public string GetTextToDisplayOnGUI() { return displayOnGui; }
        [SerializeField] private ItemSO itemSO;
        public ItemSO GetItemSO() { return itemSO; }
        public Transform playerTransform { get; private set; }
        private void SetPlayerTransform(Transform toSet){ playerTransform = toSet; }
        public bool shouldStayActive { get; private set; }
        public void SetShouldStayActive(bool toSet) { shouldStayActive = toSet; }
        public bool isInTransitionState { get; private set; }
        public void SetIsInTransitionState(bool toSet) { isInTransitionState = toSet; }
        public bool isOnPlayer { get; private set; }
        public float GetMass() 
        {
            if (GetComponent<Rigidbody>() != null)
                return GetComponent<Rigidbody>().mass;
            else
                return 0;
        }
        public delegate void ItemEventHandler();
        public event ItemEventHandler EventObjectThrowRequest;
        public event ItemEventHandler EventObjectThrow;
        public event ItemEventHandler EventObjectPickup;

        public delegate void PickupRequestEventHandler(Transform fpsCameraTransform);
        public event PickupRequestEventHandler EventPickupRequested;
        public void CallEventPickupRequested(Transform fpsCameraTransform)
        {
            if (EventPickupRequested != null)
            {
                SetPlayerTransform(fpsCameraTransform.root);
                EventPickupRequested(fpsCameraTransform);
            }
        }
        public void CallEventObjectPickup()
        {
            if (EventObjectPickup != null)
            {
                EventObjectPickup();
                isInTransitionState = false;
                isOnPlayer = true;
                if (!shouldStayActive)
                    gameObject.SetActive(false);
            }
        }
        public void CallEventObjectThrow()
        {
            if (EventObjectThrow != null)
            {
                EventObjectThrow();
                isInTransitionState = false;
                isOnPlayer = false;
            }
        }
        public void CallEventObjectThrowRequest()
        {
            if (EventObjectThrowRequest != null)
                EventObjectThrowRequest();
        }
        public void SetItemPhysics(bool toState)
        {
            if(GetComponent<ItemPhysics>() != null)
            {
                ItemPhysics myPhysics = GetComponent<ItemPhysics>();
                myPhysics.RigidbodiesActivateDeactivate(toState);
                myPhysics.CollidersActivateDeactivate(toState);
            }
        }
        public void SetShouldInformCamera(bool toState)
        {
            if(GetComponent<ItemCameraManipulation>() != null)
            {
                GetComponent<ItemCameraManipulation>().SetShouldInformCamera(toState);
            }
        }
    }
}
