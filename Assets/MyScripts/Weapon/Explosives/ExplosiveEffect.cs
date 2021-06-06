using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ExplosiveEffect : MonoBehaviour
    {
        [SerializeField] private Transform effectTransform;
        private ExplosiveMaster explosiveMaster;
        private void OnEnable()
        {
            SetInit();
            explosiveMaster.EventExplode += LaunchEffect;
        }
        private void OnDisable()
        {
            explosiveMaster.EventExplode -= LaunchEffect;
        }
        private void SetInit()
        {
            explosiveMaster = GetComponent<ExplosiveMaster>();
        }
        private void LaunchEffect()
        {
            effectTransform.parent = null;
            effectTransform.gameObject.SetActive(true);
            StartCoroutine(Deactivate());
        }
        private IEnumerator Deactivate()
        {
            Destroy(gameObject, 10f);
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
        }
    }
}
