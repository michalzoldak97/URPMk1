using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunMaster : MonoBehaviour
    {
        public delegate void GeneralEventHnadler();
        public event GeneralEventHnadler EventShootRequest;
        public event GeneralEventHnadler EventGunShoot;
        public event GeneralEventHnadler EventAimRequest;
        public event GeneralEventHnadler EventUnAim;
        public event GeneralEventHnadler EventReloadRequest;
        public event GeneralEventHnadler EventReload;

        public delegate void HitEventHnadler(RaycastHit rayHit, Transform hitTransform, int layer);
        public event HitEventHnadler EventHit;

        public bool canShoot { get; set; }

        public void CallEventShootRequest()
        {
            if(EventShootRequest!=null)
            {
                EventShootRequest();
            }
        }
        public void CallEventGunShoot()
        {
            if (EventGunShoot != null)
            {
                EventGunShoot();
            }
        }
        public void CallEventAimRequest()
        {
            if (EventAimRequest != null)
            {
                EventAimRequest();
            }
        }
        public void CallEventUnAim()
        {
            if (EventUnAim != null)
            {
                EventUnAim();
            }
        }
        public void CallEventReloadRequest()
        {
            if (EventReloadRequest != null)
            {
                EventReloadRequest();
            }
        }
        public void CallEventReload()
        {
            if (EventReload != null)
            {
                EventReload();
            }
        }
        public void CallEventHit(RaycastHit rayHit, Transform hitTransform, int layer)
        {
            if (EventHit != null)
            {
                EventHit(rayHit, hitTransform, layer);
            }
        }
    }
}
