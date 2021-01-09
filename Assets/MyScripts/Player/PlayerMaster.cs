using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class PlayerMaster : MonoBehaviour
    {
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventInventoryChanged;
        public event GeneralEventHandler EventPlayerShooted;

        public delegate void ControllerEventHandler(bool toState);
        public event ControllerEventHandler EventControllerFreeze;
        public event ControllerEventHandler EventCursorOnOff;

        public delegate void AmmoPickUpEventHandler(string ammoName, int quantity);
        public event AmmoPickUpEventHandler EventPickedUpAmmo;

        public bool isInventoryOn { get; set; }
        public void CallEventControllerFreeze(bool toState)
        {
            if(EventControllerFreeze!=null)
            {
                EventControllerFreeze(toState);
            }
        }
        public void CallEventCursorOnOff(bool toState)
        {
            if (EventCursorOnOff != null)
            {
                EventCursorOnOff(toState);
            }
        }
        public void CallEventInventoryChanged()
        {
            if (EventInventoryChanged != null)
            {
                EventInventoryChanged();
            }
        }
        public void CallEventPickedUpAmmo(string ammoName, int quantity)
        {
            if (EventPickedUpAmmo != null)
            {
                EventPickedUpAmmo(ammoName, quantity);
            }
        }
    }
}
