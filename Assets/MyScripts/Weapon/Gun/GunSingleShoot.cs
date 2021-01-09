using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunSingleShoot : MonoBehaviour
    {
        private float realDamage;
        private float distance;
        [SerializeField] float[] dmgEquation; // 1 - multiplayer, 2 - constant
        [SerializeField] float[] penetrationCoeff; // 1 - divider, 2 - variance range
        private float penetration;
        [SerializeField] float range;
        [SerializeField] float recoil;
        [SerializeField] Vector3 startPosition;
        [SerializeField] LayerMask layersToShoot;
        private Vector3 forwardTransform;
        private RaycastHit hit;
        private Transform myTransform;
        private GunMaster gunMaster;
        private GunAmmo gunAmmo;
        private DamagableMaster damagableMaster;
        private void Start()
        {
            SetInitials();
        }
        void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            gunAmmo = GetComponent<GunAmmo>();
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
            if (gunMaster.canShoot)
            {
                gunMaster.CallEventGunShoot();
                forwardTransform = myTransform.TransformDirection(Random.Range(-recoil, recoil), Random.Range(-recoil, recoil), 1);
                if (Physics.Raycast(myTransform.TransformPoint(startPosition), forwardTransform, out hit, range))
                {
                    if (layersToShoot == (layersToShoot | (1 << (hit.transform.gameObject.layer))))
                    {
                        distance = Vector3.Distance(myTransform.position, hit.transform.position);
                        if (distance < 0)
                            distance = 1;
                        CalculateDamage(distance);
                        damagableMaster.DamageObjGun(hit.transform, realDamage, penetration);
                        gunMaster.CallEventHit(hit, hit.transform, hit.transform.gameObject.layer);
                        Debug.Log("Shooted was: " + hit.transform.name + " Damage: " + realDamage.ToString() +" penetration: " + penetration + 
                        " at distance: " + distance.ToString());
                        //hit.transform.SendMessage("CallEventShootByGun", 10, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        gunMaster.CallEventHit(hit, hit.transform, hit.transform.gameObject.layer);
                    }

                }
            }
        }
        void CalculateDamage(float atDistance)
        {
            realDamage = dmgEquation[0] * atDistance + dmgEquation[1];
            if(penetrationCoeff[0] != 0)
            {
                penetration = realDamage * penetrationCoeff[0];
                penetration = Random.Range(penetration*(1- penetrationCoeff[1]), penetration * (1 + penetrationCoeff[1]));
            }
            else
            {
                penetration = 1;
            }
        }
        public AlternativeAmmo GetAmmoStats()
        {
            AlternativeAmmo myAmmo = new AlternativeAmmo();
            myAmmo.ammoName = gunAmmo.GetCurrentAmmoName();
            myAmmo.dmgEquation1 = dmgEquation;
            myAmmo.penetrationCoeff = penetrationCoeff;
            myAmmo.range = range;
            myAmmo.recoil = recoil;
            return myAmmo;
        }
        public void SetAmmoStats(AlternativeAmmo toSet)
        {
            dmgEquation = toSet.dmgEquation1;
            penetrationCoeff = toSet.penetrationCoeff;
            range = toSet.range;
            recoil = toSet.recoil;
        }
    }
}
