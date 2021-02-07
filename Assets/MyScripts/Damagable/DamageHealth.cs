using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamageHealth : MonoBehaviour
    {
        private DamageMaster dmgMaster;
        [SerializeField] float health;
        private float currHealth;
        [SerializeField] float armor;
        float realDamage;
        void SetInit()
        {
            dmgMaster = GetComponent<DamageMaster>();
        }
        private void Start()
        {
            SetInit();
            currHealth = health;
        }
        private void OnEnable()
        {
            SetInit();
            dmgMaster.EventShootByGun += OnShoot;
            dmgMaster.HitByExplosion += OnExplosion;
        }
        private void OnDisable()
        {
            dmgMaster.EventShootByGun -= OnShoot;
            dmgMaster.HitByExplosion -= OnExplosion;
        }

        void OnShoot(float damage, float penetration)
        {
            if(penetration > armor && currHealth > 0)
            {
                currHealth -= damage;
                if(currHealth<1)
                {
                    dmgMaster.CallEventDestruction();
                }
                else 
                { 
                    dmgMaster.CallEventLowerHealth((currHealth / health), penetration); 
                }
            }
            else if (currHealth > 0)
            {
                dmgMaster.CallEventProjectileHit(damage, penetration);
            }
        }
        void OnExplosion(float damage, float penetration)
        {
            if (penetration > armor && currHealth > 0)
            {
                currHealth -= damage;
                if (currHealth < 1)
                {
                    dmgMaster.CallEventDestruction();
                }
                else
                {
                    dmgMaster.CallEventLowerHealth((currHealth / health), penetration);
                    //Debug.Log("Event called" );
                }
            }
            else if (currHealth > 0)
            {
                realDamage = (penetration / (armor + penetration)) * damage;
                currHealth -= realDamage;
                if (currHealth < 1)
                {
                    dmgMaster.CallEventDestruction();
                }
                else
                {
                    dmgMaster.CallEventLowerHealth((currHealth / health), penetration);
                }
            }
        }
    }
}
