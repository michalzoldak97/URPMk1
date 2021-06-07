using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamagableDmgText : MonoBehaviour
    {
        private string dmgTextTag;
        private Transform myTransform;
        private DamageMaster damageMaster;
        private ObjectPooler objectPooler;
        private void Start()
        {
            objectPooler = ObjectPooler.Instance;
        }

        private void OnEnable()
        {
            SetInit();
            damageMaster.EventLowerHealth += SpawnTextFromPool;
        }
        private void OnDisable()
        {
            damageMaster.EventLowerHealth -= SpawnTextFromPool;
        }
        private void SetInit()
        {
            damageMaster = GetComponent<DamageMaster>();
            objectPooler = ObjectPooler.Instance;
            myTransform = transform;
            dmgTextTag = damageMaster.GetHealthStatsSO().dmgText;
        }
        private void SpawnTextFromPool(float dmg)
        {
            GameObject dmgTxt = objectPooler.SpawnFromPoolHitEffect(dmgTextTag, myTransform.position, myTransform.rotation, null, 3);
            dmgTxt.GetComponent<TextDmg>().SetText(dmg.ToString("N1"));
        }
    }
}
