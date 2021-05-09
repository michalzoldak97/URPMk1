using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

namespace U1
{
    public class AIStopOnDamage : MonoBehaviour
    {
        private NavMeshAgent myAgent;
        private DamageMaster dmgMaster;
        private AIMaster aMaster;
        void SetInit()
        {
            dmgMaster = GetComponent<DamageMaster>();
            aMaster = GetComponent<AIMaster>();
            myAgent = GetComponent<NavMeshAgent>();
        }
        private void OnEnable()
        {
            SetInit();
            dmgMaster.EventLowerHealth += Stop;
        }
        private void OnDisable()
        {
            dmgMaster.EventLowerHealth -= Stop;
        }
        void Stop(float howBadly)
        {
            if(aMaster.canAttack==true)
                StartCoroutine(StopOnDamage());
        }
        IEnumerator StopOnDamage()
        {
            aMaster.canAttack = false;
            myAgent.speed = 0;
            yield return new WaitForSecondsRealtime(5);
            aMaster.canAttack = true;
            myAgent.speed = aMaster.GetMasterSettings().navMeshAgentSpeed;
        }
    }
}
