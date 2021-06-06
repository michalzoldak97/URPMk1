using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemMaster : MonoBehaviour
    {
        [SerializeField] private ItemSO itemSO;
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventObjectThrowRequest;
        public event GeneralEventHandler EventObjectThrow;
        public event GeneralEventHandler EventObjectPickup;

        public delegate void PickupRequestEventHandler(Transform playerTransform);
        public event PickupRequestEventHandler EventPickupRequested;

        public bool shouldStayActive;
        public string displayOnGui = "Press 'E' to pickup";

        public ItemSO GetItemSO()
        {
            return itemSO;
        }
        public void CallEventPickupRequested(Transform playerTransform)
        {
            //Debug.Log("Pickup requested" + playerTransform);
            if (EventPickupRequested != null)
            {
                //Debug.Log("Pickup requested and not null 1");
                EventPickupRequested(playerTransform);
                //Debug.Log("Pickup requested and not null 2");
            }
            //else if(EventPickupRequested == null)
            //Debug.Log("NULL!!!");
        }
        public void CallEventObjectPickup()
        {
            if (EventObjectPickup != null)
            {
                //Debug.Log("Pickup going and not null 1");
                EventObjectPickup();
                //Debug.Log("Pickup going and not null 2");
            }
        }
        public void CallEventObjectThrow()
        {
            if (EventObjectThrow != null)
            {
                EventObjectThrow();
            }
        }
        public void CallEventObjectThrowRequest()
        {
            if (EventObjectThrowRequest != null)
            {
                EventObjectThrowRequest();
            }
        }

    }
}
