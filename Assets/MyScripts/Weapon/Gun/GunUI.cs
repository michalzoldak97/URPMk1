using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace U1
{
    public class GunUI : MonoBehaviour
    {
        private GunMaster gunMaster;
        private PlayerMaster playerMaster;
        private ItemMaster itemMaster;
        private GunAmmo gunAmmo;
        private PlayerAmmo playerAmmo;
        bool canUpdateAmmo;
        [SerializeField] GameObject myUI;
        [SerializeField] TMP_Text textGunAmmo;
        [SerializeField] TMP_Text ammoTypeText;
        [SerializeField] GameObject[] optionsUI;
        [SerializeField] Image burstMode, automaticMode;

        void SetInit()
        {
            gunMaster = GetComponent<GunMaster>();
            gunAmmo = GetComponent<GunAmmo>();
            itemMaster = GetComponent<ItemMaster>();
            playerMaster = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMaster>();
            playerAmmo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAmmo>();
            UpdateAmmoType();
            if(canUpdateAmmo)
                StartCoroutine(RefreshPlayerAmmo());
        }
        private void Start()
        {
            SetInit();
            canUpdateAmmo = true;
        }
        private void OnEnable()
        {
            SetInit();
            playerMaster.EventPickedUpAmmo += StartIERefresh;
            itemMaster.EventObjectPickup += ActivateUI;
            itemMaster.EventObjectThrow += ActivateUI;
        }
        private void OnDisable()
        {
            playerMaster.EventPickedUpAmmo -= StartIERefresh;
            itemMaster.EventObjectPickup -= ActivateUI;
            itemMaster.EventObjectThrow -= ActivateUI;
        }
        public void UpdateAmmoType()
        {
            ammoTypeText.text = gunAmmo.GetCurrentAmmoName();
        }
        public void AmmoChangedOnGun(int currentNo)
        {
            textGunAmmo.text = currentNo.ToString() + " | " + playerAmmo.GetAmmoNum(gunAmmo.GetCurrentAmmoName());
        }

        public void ActivateOptions()
        {
            if(optionsUI.Length > 0 && optionsUI[0].activeSelf)
            {
                for (int i = 0; i < optionsUI.Length; i++)
                {
                    optionsUI[i].SetActive(false);
                }
            }
            else if(optionsUI.Length > 0 && !optionsUI[0].activeSelf)
            {
                for (int i = 0; i < optionsUI.Length; i++)
                {
                    optionsUI[i].SetActive(true);
                }
            }
        }

        void ActivateUI()
        {
            myUI.SetActive(!myUI.activeSelf);
        }

        void StartIERefresh(string dummy, int sth)
        {
            StartCoroutine(RefreshPlayerAmmo());
        }
        IEnumerator RefreshPlayerAmmo()
        {
            yield return new WaitForEndOfFrame();
            AmmoChangedOnGun(gunAmmo.currentAmmo);
        }

        public void BurstModeChanged(bool changedTo)
        {
            if (burstMode != null)
            {
                if (changedTo)
                {
                    burstMode.color = new Color32(0, 255, 0, 200);
                }
                else
                {
                    burstMode.color = new Color32(255, 0, 0, 200);
                }
            }
        }
        public void AutomaticModeChanged(bool changedTo)
        {
            if (automaticMode != null)
            {
                if (changedTo)
                {
                    automaticMode.color = new Color32(0, 255, 0, 200);
                }
                else
                {
                    automaticMode.color = new Color32(255, 0, 0, 200);
                }
            }
        }
    }
}
