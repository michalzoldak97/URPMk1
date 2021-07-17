using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamageDestroyEffects : MonoBehaviour
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
            dmgMaster.EventDestroyEffects += Explode;
        }
        private void OnDisable()
        {
            dmgMaster.EventDestroyEffects -= Explode;
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
        }
    }
}