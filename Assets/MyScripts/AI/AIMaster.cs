using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIMaster : MonoBehaviour
    {
        [SerializeField] private Transform[] wayPoints;
        [SerializeField] private AIEnemy_1 aSettings;

        public bool canAttack = true;
        public bool canShoot { get; set; }
        private int stateCounter;
        public Transform enemyTransform { get; private set; }

        public delegate void AITargetEventHandler(Transform target);
        public event AITargetEventHandler EventFollowTarget;
        public event AITargetEventHandler EventAttackTarget;
        public event AITargetEventHandler EventShootTarget;


        public delegate void AIMoveEventHandler();
        public event AIMoveEventHandler EventNoTargetVisible;

        public Transform[] GetWaypoints() { return wayPoints; }

        public AIEnemy_1 GetMasterSettings(){return aSettings;}

        public void CallEventFollowTarget(Transform toFollow)
        {
            if(EventFollowTarget != null)
                EventFollowTarget(toFollow);
        }
        public void CallEventAttackTarget(Transform toFollow)
        {
            if (EventAttackTarget != null)
                EventAttackTarget(toFollow);
        }

        public void CallEventShootTarget(Transform toFollow)
        {
            if (EventShootTarget != null)
                EventShootTarget(toFollow);
        }

        public void CallEventNoTargetVisible()
        {
            if (EventNoTargetVisible != null)
                EventNoTargetVisible();
        }
        public void SetClosestTarget(Transform toTarget, float distance)
        {
            if(enemyTransform == null)
                enemyTransform = toTarget;
            if (toTarget == enemyTransform)
            {
                stateCounter++;
                if(stateCounter > 2)
                    CallEventFollowTarget(enemyTransform);
                if(stateCounter > 3 && distance < aSettings.attackRange * aSettings.attackRange && canAttack)
                    CallEventAttackTarget(enemyTransform);
            }
            else if(toTarget != enemyTransform)
            {
                enemyTransform = null;
                stateCounter = 0;
            }
        }
    }
}
