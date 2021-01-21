using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace U1
{
    public class AINaviAttack : MonoBehaviour
    {
        private AIEnemy_1 aSettings;
        private Transform[] wayPoints;
        private Transform myTransform;
        private Transform targetTransform;
        private Vector3 myDestination;
        private int waypointCount;
        private int randomCount;
        private int waypointFailCount, targetFailCount;
        private bool attack;
        private NavMeshAgent myNevMesh;
        private AIMaster aMaster;
        void SetInit()
        {
            myNevMesh = GetComponent<NavMeshAgent>();
            aMaster = GetComponent<AIMaster>();
            myTransform = transform;
            wayPoints = aMaster.GetWaypoints();
            aSettings = aMaster.GetMasterSettings();
            myNevMesh.speed = aSettings.navMeshAgentSpeed;
            MoveAIAgent();
        }
        private void OnEnable()
        {
            SetInit();
            aMaster.EventNoTargetVisible += MoveAIAgent;
            aMaster.EventFollowTarget += SetFollowTarget;
        }
        private void OnDisable()
        {
            aMaster.EventNoTargetVisible -= MoveAIAgent;
            aMaster.EventFollowTarget -= SetFollowTarget;
        }
        void MoveAIAgent()
        {
            if ((!attack && wayPoints.Length == 0))
            {
                //Debug.Log("Move Random " + myDestination + " random count " + randomCount);
                MoveRandom();
                randomCount++;
            }
            else if (!attack)
            {
                //Debug.Log("Move Waypoint " + myDestination + " waypoints length: " + wayPoints.Length);
                MoveWaypoint();
            }
            else if(attack)
            {
                MoveTarget();
            }
        }
        void MoveRandom()
        {
            if (myDestination == Vector3.zero)
            {
                SetRandomDestination();
            }
            else if(DistanceToDestination() <= myNevMesh.stoppingDistance* myNevMesh.stoppingDistance)
            {
                SetRandomDestination();
            }
            else if(randomCount > 30)
            {
                SetRandomDestination();
            }
        }
        void MoveWaypoint()
        {
            if (myDestination == Vector3.zero)
            {
                SetWaypointDestination();
            }
            else if(DistanceToDestination() <= myNevMesh.stoppingDistance* myNevMesh.stoppingDistance && waypointCount < wayPoints.Length - 1)
            {
                waypointCount++;
                SetWaypointDestination();
            }
            else if(DistanceToDestination() <= myNevMesh.stoppingDistance* myNevMesh.stoppingDistance && waypointCount == wayPoints.Length - 1)
            {
                waypointCount = 0;
                SetWaypointDestination();
            }
        }
        void SetWaypointDestination()
        {
            if(wayPoints[waypointCount] != null)
            {
                myDestination = wayPoints[waypointCount].position;
                myNevMesh.SetDestination(myDestination);
                waypointFailCount = 0;
            }
            else
            {
                if (waypointCount < wayPoints.Length - 1)
                    waypointCount++;
                else
                    waypointCount = 0;
                waypointFailCount++;
                if(waypointFailCount>wayPoints.Length)
                {
                    waypointFailCount = 0;
                    SetRandomDestination();
                }
            }
        }
        void SetRandomDestination()
        {
            myDestination = RandomNavSphere(myTransform.position, aSettings.sightRange, aSettings.sightLayers);
            myNevMesh.SetDestination(myDestination);
            randomCount = 0;
        }

        void SetFollowTarget(Transform toFollow)
        {
            attack = true;
            targetFailCount = 0;
            targetTransform = toFollow;
            myDestination = toFollow.position;
            myNevMesh.SetDestination(myDestination);
        }
        void MoveTarget()
        {
            targetFailCount++;
            //Debug.Log("Move Target " + myDestination);
            if (targetFailCount>15 || targetTransform==null)
            {
                myDestination = Vector3.zero;
                targetTransform = null;
                attack = false;
            }
            else
                myDestination = targetTransform.position;
            myNevMesh.SetDestination(myDestination);
        }
        Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;

            randDirection += origin;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            return navHit.position;
        }
        float DistanceToDestination()
        {
            return (myDestination - myTransform.position).sqrMagnitude;
        }
    }
}