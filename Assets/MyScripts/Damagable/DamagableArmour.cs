using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class DamagableArmour : MonoBehaviour
    {
        [SerializeField] private float thresholdArmour;
        [SerializeField] private Transform rootTransform;
        private DamageMaster damageMaster, rootDamageMaster;

        private void SetInit()
        {
            damageMaster = GetComponent<DamageMaster>();
            rootDamageMaster = rootTransform.GetComponent<DamageMaster>();
        }
        private void OnEnable()
        {
            SetInit();
            damageMaster.EventShootByGun += PassDamage;
            damageMaster.EventHitByExplosion += PassDamage;
        }
        private void OnDisable()
        {
            damageMaster.EventShootByGun -= PassDamage;
            damageMaster.EventHitByExplosion -= PassDamage;
        }
        private void PassDamage(float dmgToPass, float penetration)
        {
            if(penetration > thresholdArmour)
                rootDamageMaster.CallEventShootByGun(dmgToPass, penetration);
        }
    }
}