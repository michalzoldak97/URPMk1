using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamageMaster : MonoBehaviour
    {
        [SerializeField] private HealthStatsSO myHealthStats;
        private DamagableMaster damagableMaster;
        public delegate void DestructionEventHandler();
        public event DestructionEventHandler EventDestruction;
        public delegate void GetDamageEventHandler(float damageAmount, float penetration);
        public event GetDamageEventHandler EventShootByGun;
        public event GetDamageEventHandler EventHitByExplosion;
        public delegate void DamageEventHandler(float damageAmount);
        public event DamageEventHandler EventProjectileHit;
        public event DamageEventHandler EventLowerHealth;

        public HealthStatsSO GetHealthStatsSO()
        {
            return myHealthStats;
        }

        private void Awake()
        {
            RegisterInMaster();
        }
        private void RegisterInMaster()
        {
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
            damagableMaster.AddToDictionary(transform, this);
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
            if (EventHitByExplosion != null)
            {
                EventHitByExplosion(damageAmount, penetration);
            }
        }
        public void CallEventProjectileHit(float damageAmount)
        {
            if (EventProjectileHit != null)
            {
                EventProjectileHit(damageAmount);
            }
        }
        public void CallEventLowerHealth(float damage)
        {
            if (EventLowerHealth != null)
            {
                EventLowerHealth(damage);
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
    }
}
