using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIWeaponRestricted : MonoBehaviour
    {
        [SerializeField] private Transform weaponTransform;
        [SerializeField] private bool restrictRotation;
        [SerializeField] private float maxUp, maxDown;
        private AIMaster aMaster;

        void SetInit()
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
        void RotateWeaponTowards(Transform targetTransform)
        {
            Quaternion testRotation = Quaternion.LookRotation(targetTransform.position - weaponTransform.position, Vector3.up);
            testRotation.y = 0; testRotation.z = 0;
            if (restrictRotation)
            {
                float maxUpTransformed = -maxUp / 180;
                float maxDownTransformed = -maxDown / 180;
                if (testRotation.x < maxUpTransformed)
                    testRotation.x = maxUpTransformed;
                else if (testRotation.x > maxDownTransformed)
                    testRotation.x = maxDownTransformed;
            }
            Debug.Log("Rotation x: " + testRotation.x);
            weaponTransform.localRotation = testRotation;
        }
    }
}
