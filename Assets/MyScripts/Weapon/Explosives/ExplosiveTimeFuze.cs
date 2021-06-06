using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ExplosiveTimeFuze : MonoBehaviour
    {
        private ExplosiveMaster explosiveMaster;
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
            explosiveMaster.CallEventIgniteExplosion();
        }
    }
}
