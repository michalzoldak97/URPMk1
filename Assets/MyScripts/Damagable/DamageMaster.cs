using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamageMaster : MonoBehaviour
    {
        private DamagableMaster damagableMaster;
        public delegate void DestructionEventHandler();
        public event DestructionEventHandler EventDestruction;
        public delegate void GunDamageEventHandler(float damageAmount, float penetration);
        public event GunDamageEventHandler EventShootByGun;
        public event GunDamageEventHandler HitByExplosion;
        public event GunDamageEventHandler EventProjectileHit;
        public event GunDamageEventHandler EventLowerHealth;

        void Awake()
        {
            RegisterInMaster();
        }
        public void CallEventShootByGun(float damageAmount, float penetration)
        {
            if(EventShootByGun!=null)
            {
                EventShootByGun(damageAmount, penetration);
            }
        }
        public void CallHitByExplosion(float damageAmount, float penetration)
        {
            if (HitByExplosion != null)
            {
                HitByExplosion(damageAmount, penetration);
            }
        }
        public void CallEventProjectileHit(float damageAmount, float penetration)
        {
            if (EventProjectileHit != null)
            {
                EventProjectileHit(damageAmount, penetration);
            }
        }
        public void CallEventLowerHealth(float howBadly, float dummy)
        {
            if (EventLowerHealth != null)
            {
                EventLowerHealth(howBadly, dummy);
            }
        }
        public void CallEventDestruction()
        {
            if (EventDestruction != null)
            {
                EventDestruction();
            }
        }
        private void OnDestroy()
        {
            damagableMaster.RemoveFromDictionary(transform);
        }
        void RegisterInMaster()
        {
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
            damagableMaster.AddToDictionary(transform, this);
        }
    }
}
