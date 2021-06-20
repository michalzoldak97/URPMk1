using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunGrenadeLauncher : MonoBehaviour
    {
        [SerializeField] private float launchForce, fowPos;
        //[SerializeField] Vector3 launchPos;
        [SerializeField] private GameObject realWarhead, warheadModel;
        private Transform myTransform;
        private GunMaster gunMaster;
        private void OnEnable()
        {
            myTransform = transform;
            gunMaster = GetComponent<GunMaster>();
            gunMaster.EventShootRequest += LaunchGrenade;
        }
        private void OnDisable()
        {
            gunMaster.EventShootRequest -= LaunchGrenade;
        }
        private void LaunchGrenade()
        {
            Vector3 launchPos = myTransform.position + myTransform.forward*fowPos;
            GameObject go = Instantiate(realWarhead, launchPos, myTransform.rotation);
            go.GetComponent<Rigidbody>().AddForce(go.transform.forward * launchForce, ForceMode.Impulse);
            gunMaster.CallEventGunShoot();
            warheadModel.SetActive(false);
        }
    }
}
