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
        private WaitForSeconds dmgDelay = new WaitForSeconds(5f);
        private void SetInit()
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
        private void Stop(float howBadly)
        {
            if(aMaster.canAttack==true && gameObject.activeSelf)
                StartCoroutine(StopOnDamage());
        }
        private IEnumerator StopOnDamage()
        {
            aMaster.canAttack = false;
            myAgent.speed = 0;
            yield return dmgDelay;
            aMaster.canAttack = true;
            myAgent.speed = aMaster.GetMasterSettings().navMeshAgentSpeed;
        }
    }
}
