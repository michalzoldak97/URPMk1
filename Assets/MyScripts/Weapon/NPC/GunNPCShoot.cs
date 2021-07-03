using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunNPCShoot : MonoBehaviour
    {
        private Transform myTransform;
        private Vector3 forwardTransform;
        private RaycastHit hit;
        private float realDamage;
        private float penetration;
        private float distance;
        private GunMaster gunMaster;
        private GunNPCInput gunInput;
        private NPCGunSO gunSettings;
        private DamagableMaster damagableMaster;

        void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            gunInput = GetComponent<GunNPCInput>();
            gunSettings = gunInput.GetMasterSettings();
            myTransform = transform;
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
        }
        private void OnEnable()
        {
            SetInitials();
            gunMaster.EventShootRequest += Shoot;
        }
        private void OnDisable()
        {
            gunMaster.EventShootRequest -= Shoot;
        }
        void Shoot()
        {
            //Debug.Log("Shoot");
            gunMaster.CallEventGunShoot();
            forwardTransform = myTransform.TransformDirection(Random.Range(-gunSettings.recoil, gunSettings.recoil), Random.Range(-gunSettings.recoil, gunSettings.recoil), 1);
            //Debug.DrawRay(myTransform.position, forwardTransform * 150, Color.red, 0.5f);
            if (Physics.Raycast(myTransform.position, forwardTransform, out hit, gunSettings.maxRange))
            {
                //Debug.Log("hit transform: " + hit.transform.name);
                if (gunSettings.layersToShoot == (gunSettings.layersToShoot | (1 << (hit.transform.gameObject.layer))))
                {
                    distance = Vector3.Distance(myTransform.position, hit.transform.position);
                    if (distance < 0)
                        distance = 1;
                    CalculateDamage(distance);
                    damagableMaster.DamageObjGun(hit.transform, realDamage, penetration);
                    gunMaster.CallEventHit(hit, hit.transform, hit.transform.gameObject.layer);
                    //Debug.Log("Shooted was: " + hit.transform.name + " Damage: " + realDamage.ToString() + " penetration: " + penetration +
                    //" at distance: " + distance.ToString());
                    //hit.transform.SendMessage("CallEventShootByGun", 10, SendMessageOptions.DontRequireReceiver);
                }
                /*else
                {
                    gunMaster.CallEventHit(hit, hit.transform, hit.transform.gameObject.layer);
                }*/
            }

        }
        void CalculateDamage(float atDistance)
        {
            realDamage = gunSettings.dmgEquation[0] * atDistance + gunSettings.dmgEquation[1];
            if (gunSettings.penetrationCoeff[0] != 0)
            {
                penetration = realDamage * gunSettings.penetrationCoeff[0];
                penetration = Random.Range(penetration * (1 - gunSettings.penetrationCoeff[1]), penetration * (1 + gunSettings.penetrationCoeff[1]));
            }
            else
            {
                penetration = 1;
            }
        }
    }
}
