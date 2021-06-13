using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AILook : UpdateBehaviour
    {
        private AIEnemy_1 aSettings;
        private float nextCheck, checkRate, minDistance;
        private Transform myTransform, currTarget, targetToCheck;
        private AIMaster aMaster;
        private GlobalEnemyChecker GEC;
        public float GetCheckRate() { return checkRate; }
        private void Start()
        {
            SetInit();
        }
        private void SetInit()
        {
            GEC = GameObject.FindGameObjectWithTag("GEC").GetComponent<GlobalEnemyChecker>();
            myTransform = transform;
            aMaster = GetComponent<AIMaster>();
            aSettings = aMaster.GetMasterSettings();
            checkRate = Random.Range(aSettings.baseCheckRate - 0.2f, aSettings.baseCheckRate + 0.2f);
        }
        public override void GetUpdate()
        {
            if(Time.time>nextCheck)
            {
                nextCheck = Time.time + checkRate;
                CheckByDistance();
            }
        }
        private void CheckByDistance()
        {
            for (int i = 0; i < aSettings.myEnemyIDs.Length; i++)
            {
                List<Transform> enemyList = GEC.GetEnemyList(aSettings.myEnemyIDs[i]);
                if (enemyList != null)
                {
                    int enemyListCount = enemyList.Count;
                    for (int j = 0; j < enemyListCount; j++)
                    {
                        Vector3 betweenMeAndEnemy = enemyList[j].position - myTransform.position;
                        if (betweenMeAndEnemy.sqrMagnitude < aSettings.sightRange * aSettings.sightRange)
                        {
                            float distanceToCheck = betweenMeAndEnemy.sqrMagnitude;
                            targetToCheck = enemyList[j];
                            if (distanceToCheck < minDistance)
                            {
                                if (CheckVisibility(distanceToCheck))
                                {
                                    minDistance = distanceToCheck;
                                    currTarget = targetToCheck;
                                }
                            }
                            else if (CheckVisibility(distanceToCheck) && minDistance == 0)
                            {
                                minDistance = distanceToCheck;
                                currTarget = targetToCheck;
                            }
                        }
                    }
                }
            }
            if (currTarget != null)
            {
                aMaster.SetClosestTarget(currTarget, minDistance);
                currTarget = null;
                minDistance = 0;
            }
            else
                aMaster.CallEventNoTargetVisible();
        }
        private bool CheckVisibility(float distanceToCheck)
        {
            RaycastHit hit;
            float myAttackRange = aSettings.attackRange;
            float mySightRange = aSettings.sightRange;
            LayerMask mySightLayers = aSettings.sightLayers;
            if (Physics.Raycast(myTransform.position, targetToCheck.position - myTransform.position, out hit, mySightRange, mySightLayers))
            {
                if (hit.transform == targetToCheck)
                    return true;
                else if (distanceToCheck < myAttackRange * myAttackRange)
                    return CheckCorners(targetToCheck.GetComponent<Collider>().bounds, mySightRange, mySightLayers, hit);
                else
                    return false;
            }
            else
                return false;
        }
        private bool CheckCorners(Bounds bounds, float sightRange, LayerMask sightlayers, RaycastHit hit)
        {
            float x, y, z;

            Vector3 v3Center = bounds.center;
            Vector3 v3Extents = bounds.extents;
            Vector3 myPosition = myTransform.position;
            Vector3 v3Corner = myPosition;
            x = v3Extents.x; y = v3Extents.y - 0.1f; z = v3Extents.z - 0.1f;
            v3Corner.x = v3Center.x; v3Corner.y = v3Center.y + y; v3Corner.z = v3Center.z;  // top middle 
            //Debug.DrawRay(myPosition, (v3Corner - myPosition)* aSettings.sightRange, Color.red, 5);
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                //Debug.Log("HIt, target transform=: " + targetToCheck + " hit transform = : " + hit.transform);
                if (hit.transform == targetToCheck)
                    return true;
            }
            v3Corner.x = v3Center.x + x; v3Corner.y = v3Center.y + 0.1f;  // right middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                if (hit.transform == targetToCheck)
                    return true;
            }
            v3Corner.x = v3Center.x - x;   // left middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                if (hit.transform == targetToCheck)
                    return true;
            }
            v3Corner.x = v3Center.x; v3Corner.z = v3Center.z - z;  // right middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                if (hit.transform == targetToCheck)
                    return true; 
            }
            v3Corner.z = v3Center.z + z;  // left middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                if (hit.transform == targetToCheck)
                   return true;
            }
            return false;
        }
    }
}