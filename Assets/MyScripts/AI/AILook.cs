using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AILook : UpdateBehaviour
    {
        private AIEnemy_1 aSettings;
        private float nextCheck;
        private float checkRate;
        private float minDistance;
        private Transform myTransform;
        private Transform currTarget;
        private Transform targetToCheck;
        private Vector3 betweenMeAndEnemy;
        private AIMaster aMaster;
        private GlobalEnemyChecker GEC;
        void Start()
        {
            SetInit();
        }
        void SetInit()
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
        void CheckByDistance()
        {
            for (int i = 0; i < aSettings.myEnemyIDs.Length; i++)
            {
                List<Transform> enemyList = GEC.GetEnemyList(aSettings.myEnemyIDs[i]);
                int enemyListCount = enemyList.Count;
                if (enemyList != null)
                {
                    for (int j = 0; j < enemyListCount; j++)
                    {
                        betweenMeAndEnemy = enemyList[j].position - myTransform.position;
                        if (betweenMeAndEnemy.sqrMagnitude < aSettings.sightRange * aSettings.sightRange)
                        {
                            //Debug.Log("Is in range: " + GEC.GetEnemyList(aSettings.myEnemyIDs[i])[j].name + " at distance " + betweenMeAndEnemy.sqrMagnitude);
                            float distanceToCheck = betweenMeAndEnemy.sqrMagnitude;
                            targetToCheck = enemyList[j];
                            if (distanceToCheck < minDistance)
                            {
                                if (CheckVisibility(distanceToCheck))
                                {
                                    //Debug.Log("Visibility&&Distance " + targetToCheck.name + " distanceToCheck " + distanceToCheck + "  min distance " + minDistance);
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
                //Debug.Log("ICan see: " + currTarget.name + " minDistance " + minDistance);
                currTarget = null;
                minDistance = 0;
            }
            else
            {
                aMaster.CallEventNoTargetVisible();
                //Debug.Log("NOt visible: " );
            }
        }
        bool CheckVisibility(float distanceToCheck)
        {
            RaycastHit hit;
            float myAttackRange = aSettings.attackRange;
            float mySightRange = aSettings.sightRange;
            LayerMask mySightLayers = aSettings.sightLayers;
            //Debug.Log("Check visibility for: " + targetToCheck.name + " at distance " + distanceToCheck);
            if (Physics.Raycast(myTransform.position, targetToCheck.position - myTransform.position, out hit, mySightRange, mySightLayers))
            {
                //Debug.Log(hit.transform.name);
                if (hit.transform == targetToCheck)
                {
                    //Debug.Log("Is visible " + targetToCheck.name + " at distance " + distanceToCheck);
                    return true;
                }
                else if (distanceToCheck < myAttackRange * myAttackRange)
                {
                    //Debug.Log("Check corners");
                    return CheckCorners(targetToCheck.GetComponent<Collider>().bounds, mySightRange, mySightLayers, hit);
                }
                else
                    return false;
            }
            else
                return false;
        }
        bool CheckCorners(Bounds bounds, float sightRange, LayerMask sightlayers, RaycastHit hit)
        {
            float x, y, z;

            Vector3 v3Center;
            Vector3 v3Extents;
            Vector3 myPosition = myTransform.position;
            Vector3 v3Corner = myPosition;
            v3Center = bounds.center;
            v3Extents = bounds.extents;
            x = v3Extents.x; y = v3Extents.y - 0.1f; z = v3Extents.z - 0.1f; 
            v3Corner.Set(v3Center.x, v3Center.y + y, v3Center.z);  // top middle 
            //Debug.DrawRay(myPosition, (v3Corner - myPosition)* aSettings.sightRange, Color.red, 5);
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                //Debug.Log("HIt, target transform=: " + targetToCheck + " hit transform = : " + hit.transform);
                if (hit.transform == targetToCheck)
                {
                    return true;
                }
            }
            v3Corner.Set(v3Center.x + x, v3Center.y+0.1f, v3Center.z);  // right middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                //Debug.Log("HIt, target transform=: " + targetToCheck + " hit transform = : " + hit.transform + " x: " + x);
                if (hit.transform == targetToCheck)
                {
                    return true;
                }
            }
            
            v3Corner.Set(v3Center.x - x, v3Center.y + 0.1f, v3Center.z);  // left middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                //Debug.Log("HIt, target transform=: " + targetToCheck + " hit transform = : " + hit.transform + " x: " + x);
                if (hit.transform == targetToCheck)
                {
                    return true;
                }
            }
            v3Corner.Set(v3Center.x, v3Center.y + 0.1f, v3Center.z - z);  // right middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                //Debug.Log("HIt, target transform=: " + targetToCheck + " hit transform = : " + hit.transform + " x: " + x);
                if (hit.transform == targetToCheck)
                {
                    return true; 
                }
            }
            v3Corner.Set(v3Center.x, v3Center.y + 0.1f, v3Center.z + z);  // left middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, sightRange, sightlayers))
            {
                //Debug.Log("HIt, target transform=: " + targetToCheck + " hit transform = : " + hit.transform + " x: " + x);
                if (hit.transform == targetToCheck)
                {
                    return true;
                }
            }
            return false;
        }

        public float GetCheckRate()
        {
            return checkRate;
        }
    }
}