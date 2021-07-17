using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamageHealth : MonoBehaviour
    {
        private float currHealth;
        public float GetCurrHealth() { return currHealth; }
        private DamageMaster dmgMaster;
        void SetInit()
        {
            dmgMaster = GetComponent<DamageMaster>();
            currHealth = (float)dmgMaster.GetHealthStatsSO().health;
        }
        private void OnEnable()
        {
            SetInit();
            dmgMaster.EventLowerHealth += LowerHealth;
        }
        private void OnDisable()
        {
            dmgMaster.EventLowerHealth -= LowerHealth;
        }
        private void LowerHealth(float damage)
        {
            currHealth -= damage;
            //Debug.Log(gameObject.name + "  was damaged by: " + damage);
            if(currHealth <= 0)
            {
                currHealth = 0;
                dmgMaster.CallEventDestruction();
            }
        }
    }
}
