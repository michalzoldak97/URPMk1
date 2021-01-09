using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class AIDestroy : MonoBehaviour
    {
        [SerializeField] ObjectsContainer myObjectContainer;
        private DamageMaster dmgMaster;
        private GameObject effect;

        void SetInit()
        {
            dmgMaster = GetComponent<DamageMaster>();
        }

        void Start()
        {
            SetInit();
            ChooseDestroyScenario();
        }
        private void OnEnable()
        {
            SetInit();
            dmgMaster.EventDestruction += Explode;
        }
        private void OnDisable()
        {
            dmgMaster.EventDestruction -= Explode;
        }
        void ChooseDestroyScenario()
        {
            int num = Random.Range(0, myObjectContainer.combinationNum);
            switch (num)
            {
                case 0:
                    PrepareExplode(num);
                    break;
                case 1:
                    PrepareExplode(num);
                    break;
            }
        }
        void PrepareExplode(int num)
        {
            effect = Instantiate(myObjectContainer.objecsSet1[num], transform.position, transform.rotation, transform);
            effect.SetActive(false);
        }
        void Explode()
        {
            effect.SetActive(true);
            effect.transform.SetParent(null);
            Destroy(gameObject, Random.Range(6, 10));
            gameObject.SetActive(false);
        }
    }
}