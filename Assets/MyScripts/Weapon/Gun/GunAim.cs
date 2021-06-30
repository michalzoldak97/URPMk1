using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunAim : MonoBehaviour
    {
        [SerializeField] Vector3 aimPosition;
        private GunMaster gunMaster;
        private ItemMaster itemMaster;
        private Vector3 startPosition;
        private Transform myTransform;
        private void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            itemMaster = GetComponent<ItemMaster>();
            myTransform = transform;
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
        private void AimOn()
        {
            if (!gunMaster.isReloading)
            {
                myTransform.localPosition = aimPosition;
                itemMaster.playerTransform.GetComponent<PlayerInventoryMaster>().ItemCameraChangeState(true);
            }
        }
        private void UnAim()
        {
            myTransform.localPosition = startPosition;
            itemMaster.playerTransform.GetComponent<PlayerInventoryMaster>().ItemCameraChangeState(false);
            itemMaster.SetShouldInformCamera(true);
        }
    }
}