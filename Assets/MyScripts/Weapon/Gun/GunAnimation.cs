using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{

    public class GunAnimation : MonoBehaviour
    {
        private GunMaster gunMaster;
        private ItemMaster itemMaster;
        private Animator myAnimator;
        public string CallOnShoot = "Shoot";
        public string CallOnReload = "Reload";
        [SerializeField] Transform animatedMesh;

        void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            itemMaster = GetComponent<ItemMaster>();
            myAnimator = GetComponentInChildren<Animator>();
        }
        private void OnEnable()
        {
            SetInitials();
            gunMaster.EventGunShoot += SetShootTrigger;
            gunMaster.EventReload += SetReloadTrigger;
            itemMaster.EventObjectPickup += EnableAnimator;
            itemMaster.EventObjectThrow += DisableAnimator;
        }
        private void OnDisable()
        {
            gunMaster.EventGunShoot -= SetShootTrigger;
            gunMaster.EventReload -= SetReloadTrigger;
            itemMaster.EventObjectPickup -= EnableAnimator;
            itemMaster.EventObjectThrow -= DisableAnimator;
        }

        void SetShootTrigger()
        {
            myAnimator.SetTrigger(CallOnShoot);
        }
        void SetReloadTrigger()
        {
            myAnimator.SetTrigger(CallOnReload);
        }

        void DisableAnimator()
        {
            animatedMesh.localPosition = Vector3.zero;
            animatedMesh.localRotation = Quaternion.Euler(0f,0f,0f);
            myAnimator.enabled = false;
        }
        void EnableAnimator()
        {
            myAnimator.enabled = true;
        }
    }
}
