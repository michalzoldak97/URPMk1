﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunNPCInput : MonoBehaviour
    {
        [SerializeField] NPCGunSO gunSettings;
        private bool isShooting, isReloading;
        private int currAmmo;
        private WaitForSeconds shootDelay; 
        private AIMaster aMaster;
        private GunMaster gunMaster;
        void SetInit()
        {
            aMaster = GetComponentInParent<AIMaster>();
            gunMaster = GetComponent<GunMaster>();
            currAmmo = gunSettings.maxAmmo;
            shootDelay = new WaitForSeconds(gunSettings.shootRate);
        }
        private void OnEnable()
        {
            SetInit();
            aMaster.EventShootTarget += CallShoot;
        }
        private void OnDisable()
        {
            aMaster.EventShootTarget -= CallShoot;
        }
        void CallShoot(Transform dummy)
        {
            if(!isShooting && aMaster.canShoot && currAmmo > 0 && Time.timeScale > 0)
            {
                StartCoroutine(ShootSerie());
            }
            else if (!isReloading && !isShooting && aMaster.canShoot && Time.timeScale > 0)
            {
                StartCoroutine(Reload());
            }
        }
        IEnumerator ShootSerie()
        {
            isShooting = true;
            for (int i = 0; i < gunSettings.numOfShoots; i++)
            {
                if(currAmmo > 0)
                {
                    gunMaster.CallEventShootRequest();
                    currAmmo -= 1;
                    yield return shootDelay;
                }
                else if(!isReloading)
                {
                    StartCoroutine(Reload());
                }
            }
            isShooting = false;
        }
        IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(gunSettings.reloadTime);
            currAmmo = gunSettings.maxAmmo;
            isReloading = false;
        }

        public NPCGunSO GetMasterSettings()
        {
            return gunSettings;
        }
    }
}
