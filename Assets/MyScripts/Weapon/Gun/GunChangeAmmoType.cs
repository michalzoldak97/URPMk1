using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [System.Serializable] public class AlternativeAmmo
    {
        public string ammoName;
        public float[] dmgEquation1;
        public float[] penetrationCoeff;
        public float range;
        public float recoil;
    }
    public class GunChangeAmmoType : MonoBehaviour
    {
        [SerializeField] List<AlternativeAmmo> alternativeAmmo;
        private GunAmmo gunAmmo;
        private GunUI gunUI;
        private GunSingleShoot gunSingleShoot;
        private int counter;

        void SetInit()
        {
            gunAmmo = GetComponent<GunAmmo>();
            gunUI = GetComponent<GunUI>();
            gunSingleShoot = GetComponent<GunSingleShoot>();
        }
        private void Start()
        {
            SetInit();
            alternativeAmmo.Add(gunSingleShoot.GetAmmoStats());
        }
        public void ChangeAmmoType()
        {
            gunAmmo.ChangeAmmoType(alternativeAmmo[counter].ammoName);
            gunSingleShoot.SetAmmoStats(alternativeAmmo[counter]);
            gunUI.UpdateAmmoType();
            gunUI.AmmoChangedOnGun(gunAmmo.currentAmmo);
            counter++;
            if (counter >= alternativeAmmo.Count)
                counter = 0;
        }
    }
}
