using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamagableGetDamage : MonoBehaviour
    {
        private DamageMaster dmgMaster;
        private float armor = 0;

        private void SetInits()
        {
            dmgMaster = GetComponent<DamageMaster>();
            armor = dmgMaster.GetHealthStatsSO().armor;
        }
        private void OnEnable()
        {
            SetInits();
            dmgMaster.EventShootByGun += ApplyDamageGun;
            dmgMaster.EventHitByExplosion += ApplyDamageExplosion;
        }
        private void OnDisable()
        {
            dmgMaster.EventShootByGun -= ApplyDamageGun;
            dmgMaster.EventHitByExplosion -= ApplyDamageExplosion;
        }
        private void ApplyDamageGun(float damage, float penetration)
        {
            if(penetration > armor)
                dmgMaster.CallEventLowerHealth(damage);
        }
        private void ApplyDamageExplosion(float damage, float penetration)
        {
            if(penetration > armor)
                dmgMaster.CallEventLowerHealth(damage);
            else 
                dmgMaster.CallEventLowerHealth((penetration / (armor + penetration)) * damage);
        }
    }
}
