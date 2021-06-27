using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIDestroy : MonoBehaviour
    {
        [SerializeField] ObjectsContainer myObjectContainer;
        private GameObject effect;
        private DamageMaster dmgMaster;

        private void Start()
        {
            dmgMaster = GetComponent<DamageMaster>();
            ChooseDestroyScenario();
        }
        private void OnEnable()
        {
            dmgMaster = GetComponent<DamageMaster>();
            dmgMaster.EventDestruction += Explode;
        }
        private void OnDisable()
        {
            dmgMaster.EventDestruction -= Explode;
        }
        private void ChooseDestroyScenario()
        {
            int num = Random.Range(0, myObjectContainer.combinationNum);
            PrepareExplode(num);
        }
        private void PrepareExplode(int num)
        {
            effect = Instantiate(myObjectContainer.objecsSet1[num], transform.position, transform.rotation, transform);
            effect.SetActive(false);
        }
        private void Explode()
        {
            effect.SetActive(true);
            effect.transform.SetParent(null);
            Destroy(gameObject, Random.Range(6, 10));
            StartCoroutine(DeactivateThis());
        }
        private IEnumerator DeactivateThis()
        {
            yield return new WaitForEndOfFrame();
            gameObject.SetActive(false);
        }
    }
}