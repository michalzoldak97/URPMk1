using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunAim : MonoBehaviour
    {
        private GunMaster gunMaster;
        private PlayerInventory playerInventory;
        //private GunPlayerInput gunPlayerInput;
        private Vector3 startPosition;
        private Transform myTransform;
        [SerializeField] Vector3 aimPosition;
        private void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            //gunPlayerInput = GetComponent<GunPlayerInput>();
            myTransform = transform;
            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            startPosition = myTransform.localPosition;
        }
        private void OnEnable()
        {
            SetInitials();
            gunMaster.EventAimRequest += AimOn;
            gunMaster.EventUnAim += UnAim;
        }
        private void OnDisable()
        {
            gunMaster.EventAimRequest -= AimOn;
            gunMaster.EventUnAim -= UnAim;
        }
        void AimOn()
        {
            if (!gunMaster.isReloading)
            {
                myTransform.localPosition = aimPosition;
                playerInventory.shouldCheckCamera = false;
                playerInventory.CameraOnOff(true, false);
            }
        }
        void UnAim()
        {
            myTransform.localPosition = startPosition;
            playerInventory.CameraOnOff(false, false);
            playerInventory.shouldCheckCamera = true;
        }

    }
}