using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIRotateMultipleWeapon : MonoBehaviour
    {
        
        [SerializeField] Transform[] weaponTransforms;
        private AIMaster aMaster;

        private void SetInit()
        {
            aMaster = GetComponent<AIMaster>();
        }
        private void OnEnable()
        {
            SetInit();
            aMaster.EventShootTarget += RotateWeaponTowards;
        }
        private void OnDisable()
        {
            aMaster.EventShootTarget -= RotateWeaponTowards;
        }
        private void RotateWeaponTowards(Transform target)
        {
            Vector3 targetPos = target.position;
            for (int i = 0; i < weaponTransforms.Length; i++)
            {
                Quaternion testRotation = Quaternion.LookRotation(targetPos - weaponTransforms[i].position, Vector3.up);
                testRotation.y = 0; testRotation.z = 0;
                weaponTransforms[i].localRotation = testRotation;
            }
        }
    }
}
