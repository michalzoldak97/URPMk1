﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIRotateMultipleWeapon : MonoBehaviour
    {
        
        [SerializeField] Transform[] weaponTransforms;
        private Transform myTransform;
        private Vector3[] baseAimPosition;
        private Quaternion lookAtRotation;
        private AIMaster aMaster;

        private void SetInit()
        {
            aMaster = GetComponent<AIMaster>();
            myTransform = transform;
            AssignWeaponPositions();
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
                int a = (i * 2) + 1;
                baseAimPosition[a] = weaponTransforms[i].position + weaponTransforms[i].forward*(Vector3.Distance(myTransform.position, targetPos));
                targetPos.x = baseAimPosition[a].x; targetPos.z = baseAimPosition[a].z;
                lookAtRotation = Quaternion.LookRotation((targetPos - weaponTransforms[i].position).normalized);
                weaponTransforms[i].rotation = Quaternion.Slerp(weaponTransforms[i].rotation, lookAtRotation, 5);
                baseAimPosition[a] = baseAimPosition[i * 2];
            }
        }
        private void AssignWeaponPositions()
        {
            baseAimPosition = new Vector3[weaponTransforms.Length * 2];
            for (int i = 0; i < weaponTransforms.Length; i++)
            {
                baseAimPosition[i * 2] = weaponTransforms[i].localPosition;
            }
        }
    }
}
