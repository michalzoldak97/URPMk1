using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace U1
{
    public class AINavi : UpdateBehaviour
    {
        private AIEnemy_1 aSettings;
        private NavMeshAgent myNevMesh;
        private AIMaster aMaster;
        private int counter = 0;
        private float nextCheck;
        void SetInit()
        {
            aMaster = GetComponent<AIMaster>();
            aSettings = aMaster.GetMasterSettings();
            myNevMesh = GetComponent<NavMeshAgent>();
            myNevMesh.speed = aSettings.navMeshAgentSpeed;
        }
        private void Start()
        {
            SetInit();
            myNevMesh.SetDestination(aMaster.GetWaypoints()[counter].position);
        }
        public override void GetUpdate()
        {
            if(Time.time > nextCheck)
            {
                nextCheck = Time.time + aSettings.baseCheckRate;
                CheckByDistance();
            }
        }
        void CheckByDistance()
        {
            if(myNevMesh.remainingDistance < aSettings.sightRange)
            {
                if (counter < aMaster.GetWaypoints().Length - 1)
                {
                    counter++;
                    myNevMesh.SetDestination(aMaster.GetWaypoints()[counter].position);
                }
                else
                    this.enabled = false;
            }
        }
    }
}

