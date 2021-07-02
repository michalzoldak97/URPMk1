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
            Vector3 vRotation = Quaternion.LookRotation(targetTransform.position - weaponTransform.position, Vector3.up).eulerAngles;
            if (vRotation.x < -maxUp)
            {
                vRotation.x = -maxUp;
                //aMaster.canShoot = false;
            }
            else if (vRotation.x > -maxDown)
            {
                vRotation.x = -maxDown;
                //aMaster.canShoot = false;
            }
            else
                //aMaster.canShoot = true;
            weaponTransform.rotation = Quaternion.Euler(vRotation.x, vRotation.y, vRotation.z);
            //Debug.Log("TestRotationX: " + weaponTransform.eulerAngles.x + " max down: " + -maxDown + " max up: " + -maxUp);
        }
    }
}
