using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace U1
{
    public class GunAmmo : MonoBehaviour
    {
        [SerializeField] int ammoCapacity;
        [SerializeField] int startAmmo;
        [SerializeField] string ammoName;
        [SerializeField] float reloadTime;

        public int currentAmmo { get; set; }
        private int amuontToRequest;
        private bool isReloading;
        private PlayerMaster playerMaster;
        private PlayerAmmo playerAmmo;
        private GunMaster gunMaster;
        private ItemMaster itemMaster;
        private GunUI gunUI;
        private AudioSource myAudio;
        void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            playerMaster = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMaster>();
            playerAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAmmo>();
            itemMaster = GetComponent<ItemMaster>();
            gunUI = GetComponent<GunUI>();
            myAudio = GetComponent<AudioSource>();
        }
        private void Start()
        {
            SetInitials();
            if (startAmmo > ammoCapacity || startAmmo < 0)
            {
                startAmmo = ammoCapacity;
            }
            currentAmmo = startAmmo;
            gunUI.AmmoChangedOnGun(currentAmmo);
            //ammoText.text = currentAmmo.ToString();
            if (currentAmmo > 0)
                SetCanShoot(true);
        }

        private void OnEnable()
        {
            SetInitials();
            gunMaster.EventGunShoot += OneShoot;
            gunMaster.EventReloadRequest += Reload;
            itemMaster.EventObjectThrow += FalseReload;
        }
        private void OnDisable()
        {
            gunMaster.EventGunShoot -= OneShoot;
            gunMaster.EventReloadRequest -= Reload;
            itemMaster.EventObjectThrow -= FalseReload;
            FalseReload();
        }

        void OneShoot()
        {
            currentAmmo -= 1;
            gunUI.AmmoChangedOnGun(currentAmmo);
            if (currentAmmo < 1)
                SetCanShoot(false);
        }

        void Reload()
        {
            if(currentAmmo<ammoCapacity && !isReloading)
            {
                //Debug.Log("Coroutine started:");// set is reloading value!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                StartCoroutine(Reloading());
            }
        }

        IEnumerator Reloading()
        {
            isReloading = true;
            SetCanShoot(false);
            gunMaster.CallEventReload();
            yield return new WaitForSecondsRealtime(reloadTime);
            amuontToRequest = ammoCapacity - currentAmmo;
            ChangeAmmo(playerAmmo.TakeAmmo(ammoName, amuontToRequest));
            //Debug.Log("Coroutine finished:");
            isReloading = false;
            if(currentAmmo>0)
                SetCanShoot(true);
        }
        public void DeloadAmmo()
        {
            if (currentAmmo>0)
            {
                playerMaster.CallEventPickedUpAmmo(ammoName, currentAmmo);
                ChangeAmmo(-currentAmmo);
            }
        }
        public void ChangeAmmoType(string toSet)
        {
            DeloadAmmo();
            ammoName = toSet;
        }
        void ChangeAmmo(int toTake)
        {
            currentAmmo += toTake;
            //Debug.Log("Current ammo: " + currentAmmo.ToString() + "  to take: " + toTake.ToString());
            gunUI.AmmoChangedOnGun(currentAmmo);
            if (currentAmmo < 1)
                SetCanShoot(false);
            else if (currentAmmo >= 1)
                SetCanShoot(true);
        }
        void SetCanShoot(bool toSet)
        {
            gunMaster.canShoot = toSet;
        }

        void FalseReload()
        {
            StopAllCoroutines();
            myAudio.Stop();
            //Debug.Log("Reload Stopped");
            isReloading = false;
            if(currentAmmo>0)
                SetCanShoot(true);
        }
        public string GetCurrentAmmoName()
        {
            return ammoName;
        }
    }
}
