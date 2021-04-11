using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunControlAimCamera : MonoBehaviour
    {
        [SerializeField] private GameObject myAimCamera;
        private GunMaster gunMaster;
        private void SetInit()
        {
            gunMaster = GetComponent<GunMaster>();
            myAimCamera.SetActive(false);
        }
        private void OnEnable()
        {
            SetInit();
            gunMaster.EventAimRequest += ActivateAimCamera;
            gunMaster.EventUnAim += DeactivateAimCamera;
        }
        private void OnDisable()
        {
            gunMaster.EventAimRequest -= ActivateAimCamera;
            gunMaster.EventUnAim -= DeactivateAimCamera;
        }
        private void ActivateAimCamera()
        {
            myAimCamera.SetActive(true);
        }
        private void DeactivateAimCamera()
        {
            myAimCamera.SetActive(false);
        }
    }
}
