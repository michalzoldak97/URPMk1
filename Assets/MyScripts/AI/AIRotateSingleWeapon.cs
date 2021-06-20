using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIRotateSingleWeapon : MonoBehaviour
    {
        [SerializeField] protected Transform weaponTransform;
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
        public virtual void RotateWeaponTowards(Transform targetTransform)
        {
            Quaternion testRotation = Quaternion.LookRotation(targetTransform.position - weaponTransform.position, Vector3.up);
            testRotation.y = 0; testRotation.z = 0;
            weaponTransform.localRotation = testRotation;
        }
    }
}
