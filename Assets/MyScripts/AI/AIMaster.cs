using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIMaster : MonoBehaviour
    {
        public bool canAttack = true;
        [SerializeField] private Transform[] wayPoints;
        [SerializeField] private AIEnemy_1 aSettings;
        public Transform enemyTransform { get; private set; }
        //private float distanceToClosest;
        private int stateCounter;

        public delegate void AITargetEventHandler(Transform target);
        public event AITargetEventHandler EventFollowTarget;
        public event AITargetEventHandler EventAttackTarget;
        public event AITargetEventHandler EventShootTarget;


        public delegate void AIMoveEventHandler();
        public event AIMoveEventHandler EventNoTargetVisible;



        public AIEnemy_1 GetMasterSettings()
        {
            return aSettings;
        }

        public void CallEventFollowTarget(Transform toFollow)
        {
            if(EventFollowTarget != null)
            {
                EventFollowTarget(toFollow);
            }
        }
        public void CallEventAttackTarget(Transform toFollow)
        {
            if (EventAttackTarget != null)
            {
                EventAttackTarget(toFollow);
            }
        }

        public void CallEventShootTarget(Transform toFollow)
        {
            if (EventShootTarget != null)
            {
                EventShootTarget(toFollow);
            }
        }

        public void CallEventNoTargetVisible()
        {
            if (EventNoTargetVisible != null)
            {
                EventNoTargetVisible();
            }
        }

        public Transform[] GetWaypoints()
        {
            return wayPoints;
        }
        public void SetClosestTarget(Transform toTarget, float distance)
        {
            //Debug.Log("closest target: "+ toTarget.name);
            if(enemyTransform == null)
            {
                enemyTransform = toTarget;
                //distanceToClosest = distance;
            }
            if (toTarget == enemyTransform)
            {
                stateCounter++;
                if(stateCounter > 2)
                {
                    CallEventFollowTarget(enemyTransform);
                    //Debug.Log("Follow target: " + enemyTransform.name);
                }
                if(stateCounter > 3 && distance < aSettings.attackRange * aSettings.attackRange && canAttack)
                {
                    CallEventAttackTarget(enemyTransform);
                    //Debug.Log("Attack: " + enemyTransform.name);
                }
            }
            else if(toTarget != enemyTransform)
            {
                enemyTransform = null;
                stateCounter = 0;
            }
        }
    }
}
