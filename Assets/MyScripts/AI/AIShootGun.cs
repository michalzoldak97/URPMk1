﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIShootGun : MonoBehaviour
    {
        [SerializeField] Transform gunTransform;
        private Transform myTransform;
        private Vector3 myPosition;
        private Vector3 myDirection;
        private AIMaster aMaster;
        private AIEnemy_1 aSettings;
        private float step = 0.1f;
        private float move = 0.5f;
        private Vector3 lookAtRotation;
        private Vector3 rotationMaskY = new Vector3(0f, 1f, 0f);
        private Quaternion finalRot;
        Vector3 targetPosition;
        Vector3 dirFromAtoB;
        float dotProd;
        void SetInit()
        {
            aMaster = GetComponent<AIMaster>();
            aSettings = aMaster.GetMasterSettings();
            myTransform = transform;
            step = aSettings.baseCheckRate / 15;
            StartCoroutine(SetStep());
        }

        private void OnEnable()
        {
            SetInit();
            aMaster.EventAttackTarget += Shoot;
        }
        private void OnDisable()
        {
            aMaster.EventAttackTarget -= Shoot;
        }

        void Shoot(Transform target)
        {
            if(CheckRotationTowards(target))
            {
                //Debug.Log("Call event shoot");
                aMaster.CallEventShootTarget(target);
                StartCoroutine(RotateTowardsObj(target));
            }
            else
                StartCoroutine(RotateTowardsObj(target));
        }
        bool CheckRotationTowards(Transform target)
        {
            targetPosition = target.position;
            myPosition = gunTransform.position;//myTransform.position;
            myDirection = gunTransform.forward;//myTransform.forward;
            targetPosition.y = 1;
            myPosition.y = 1;
            myDirection.y = 1;
            dirFromAtoB = (targetPosition - myPosition).normalized;
            dotProd = Vector3.Dot(dirFromAtoB, myDirection);
            //Debug.Log("dotProd " + dotProd);
            if (dotProd > 0.99)
            {
                // ObjA is looking mostly towards ObjB
                //Debug.Log("IS Facing towards");
                move = 0.5f;
                return true;
            }
            else if((targetPosition - myPosition).sqrMagnitude<20 && dotProd > 0.92f)
            {
                //Debug.Log("IS  Facing towards  " + dotProd + "  sqr magnitude:  " + (targetPosition - myPosition).sqrMagnitude);
                move = 0.1f;
                return true;
            }
            //Debug.Log("IS not Facing towards  " + dotProd + "  sqr magnitude:  " + (targetPosition - myPosition).sqrMagnitude);
            move = 0.5f;
            return false;
        }
        IEnumerator RotateTowardsObj(Transform target)
        {
            //Debug.Log("Step: " + step);
            //lookAtRotation = Quaternion.LookRotation(target.position - gunTransform.position).eulerAngles;
            //finalRot = Quaternion.Euler(Vector3.Scale(lookAtRotation, rotationMaskY));
            for (int i = 0; i < 15; i++)
            {
                yield return new WaitForSecondsRealtime(step);
                //Debug.Log("Move: " + move + " ii " + i);
                lookAtRotation = Quaternion.LookRotation(target.position - gunTransform.position).eulerAngles;
                finalRot = Quaternion.Euler(Vector3.Scale(lookAtRotation, rotationMaskY));
                myTransform.rotation = Quaternion.Slerp(gunTransform.rotation, finalRot, move);
            }
            move = 0.5f;
        }
        IEnumerator SetStep()
        {
            yield return new WaitForSeconds(1);
            if (GetComponent<AILook>() != null)
                step = GetComponent<AILook>().GetCheckRate() / 15;
        }
    }
}