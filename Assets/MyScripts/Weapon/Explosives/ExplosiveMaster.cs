using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ExplosiveMaster : MonoBehaviour
    {
        [SerializeField] private ExplosiveSO explosiveSO;
        public ExplosiveSO GetExplosiveSO()
        {
            return explosiveSO;
        }

        public delegate void ExplosiveEventsHandler();
        public event ExplosiveEventsHandler EventCountStarted;
        public event ExplosiveEventsHandler EventIgniteExplosion;
        public event ExplosiveEventsHandler EventExplode;

        public void CallEventCountStarted()
        {
            if (EventCountStarted != null)
            {
                EventCountStarted();
            }
        }
        public void CallEventIgniteExplosion()
        {
            if (EventIgniteExplosion != null)
            {
                EventIgniteExplosion();
            }
        }
        public void CallEventExplode()
        {
            if (EventExplode != null)
            {
                EventExplode();
            }
        }
    }
}
