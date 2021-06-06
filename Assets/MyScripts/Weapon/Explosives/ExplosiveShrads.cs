using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ExplosiveShrads : MonoBehaviour
    {
        private ExplosiveMaster explosiveMaster;
        private DamagableMaster damagableMaster;
        private int splitNum;
        private float splintRange, splintDmg, expPenetration, expForce;
        private LayerMask layersToAffect, layersToDamage;
        private Vector3[] randDir;
        private Transform myTransform;
        RaycastHit hit;

        private void Awake()
        {
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
            explosiveMaster = GetComponent<ExplosiveMaster>();
            myTransform = transform;
            ExplosiveSO myExpSO = explosiveMaster.GetExplosiveSO();
            splitNum = myExpSO.splintNum;
            splintRange = myExpSO.splintRange;
            splintDmg = Random.Range(myExpSO.splintDmg * 0.5f, myExpSO.splintDmg * 1.5f);
            expPenetration = myExpSO.expPenetration;
            expForce = myExpSO.expForce;
            layersToAffect = myExpSO.layersToAffect;
            layersToDamage = myExpSO.layersToDamage;
            randDir = new Vector3[splitNum];
            for (int i = 0; i < splitNum; i++)
            {
                randDir[i] = Random.insideUnitSphere.normalized;
            }
        }
        private void OnEnable()
        {
            explosiveMaster.EventExplode += ExplodeShrad;
        }
        private void OnDisable()
        {
            explosiveMaster.EventExplode -= ExplodeShrad;
        }
        private void ExplodeShrad()
        {
            Vector3[] directions = randDir;
            Vector3 myPos = myTransform.position;
            float range = splintRange;
            LayerMask toAffect = layersToAffect;
            LayerMask toDmg = layersToDamage;
            RaycastHit hit;
            for (int i = 0; i < splitNum; i++)
            {
                if (Physics.Raycast(myPos, directions[i], out hit, range, toAffect))
                {
                    Debug.DrawRay(myPos, directions[i] * range, Color.green, 10);
                    Transform target = hit.transform;
                    if ((toDmg.value & (1 << target.gameObject.layer)) > 0)
                        ApplayForceAndDamage(target, myPos);
                }
            }
        }
        private void ApplayForceAndDamage(Transform TTform, Vector3 myPosition)
        {
            damagableMaster.DamageObjGun(TTform, splintDmg, expPenetration);
            if (TTform.GetComponent<Rigidbody>() != null)
                TTform.GetComponent<Rigidbody>().AddExplosionForce((expForce / 10), myPosition, 10, 1, ForceMode.Impulse);
        }
    }
}
