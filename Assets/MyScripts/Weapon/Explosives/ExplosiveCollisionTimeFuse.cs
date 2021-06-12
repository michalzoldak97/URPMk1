using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ExplosiveCollisionTimeFuse : MonoBehaviour
    {
        private ExplosiveMaster explosiveMaster;
        private bool isExp;
        private void OnEnable()
        {
            SetInit();
        }
        private void SetInit()
        {
            explosiveMaster = GetComponent<ExplosiveMaster>();
            StartFuzeCounter();
        }
        private void StartFuzeCounter()
        {
            StartCoroutine(FuzeCounter());
        }
        private IEnumerator FuzeCounter()
        {
            yield return new WaitForSeconds(explosiveMaster.GetExplosiveSO().timeToExplode);
            isExp = true;
            yield return new WaitForSeconds(6);
            explosiveMaster.CallEventIgniteExplosion();
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (isExp)
                explosiveMaster.CallEventIgniteExplosion();
        }
    }
}
