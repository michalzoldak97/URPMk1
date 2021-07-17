using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIShootGun : MonoBehaviour
    {
        [SerializeField] private Transform gunTransform;
        private float move = 0.5f;
        private Vector3 rotationMaskY = new Vector3(0f, 1f, 0f);
        private Transform myTransform;
        private WaitForSeconds rotationDelay;
        private AIMaster aMaster;
        private AIEnemy_1 aSettings;
        private void SetInit()
        {
            aMaster = GetComponent<AIMaster>();
            aMaster.canShoot = true;
            aSettings = aMaster.GetMasterSettings();
            myTransform = transform;
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

        private void Shoot(Transform target)
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
        private bool CheckRotationTowards(Transform target)
        {
            Vector3 targetPosition = target.position;
            Vector3 myPosition = gunTransform.position;//myTransform.position;
            Vector3 myDirection = gunTransform.forward;//myTransform.forward;
            targetPosition.y = 1;
            myPosition.y = 1;
            myDirection.y = 1;
            Vector3 dirFromAtoB = (targetPosition - myPosition).normalized;
            float dotProd = Vector3.Dot(dirFromAtoB, myDirection);
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
        private IEnumerator RotateTowardsObj(Transform target)
        {
            for (int i = 0; i < 15; i++)
            {
                yield return rotationDelay;
                //Debug.Log("Move: " + move + " ii " + i);
                myTransform.rotation = Quaternion.Slerp(gunTransform.rotation, Quaternion.Euler(Vector3.Scale(Quaternion.LookRotation(target.position - gunTransform.position).eulerAngles, rotationMaskY)), move);
            }
            move = 0.5f;
        }
        private IEnumerator SetStep()
        {
            yield return new WaitForSeconds(1);
            if (GetComponent<AILook>() != null)
                rotationDelay = new WaitForSeconds((GetComponent<AILook>().GetCheckRate() / 15));
            else
                rotationDelay = new WaitForSeconds(aSettings.baseCheckRate / 15);
        }
    }
}
