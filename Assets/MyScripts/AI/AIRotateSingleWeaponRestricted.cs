using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIRotateSingleWeaponRestricted : AIRotateSingleWeapon
    {
        [SerializeField] private float maxUp, maxDown;
        public override void RotateWeaponTowards(Transform targetTransform)
        {
            Quaternion testRotation = Quaternion.LookRotation(targetTransform.position - weaponTransform.position, Vector3.up);
            testRotation.y = 0; testRotation.z = 0;
            float maxUpTransformed = -maxUp / 180;
            float maxDownTransformed = -maxDown / 180;
            if (testRotation.x < maxUpTransformed)
                testRotation.x = maxUpTransformed;
            else if (testRotation.x > maxDownTransformed)
                testRotation.x = maxDownTransformed;
            weaponTransform.localRotation = testRotation;
        }
    }
}
