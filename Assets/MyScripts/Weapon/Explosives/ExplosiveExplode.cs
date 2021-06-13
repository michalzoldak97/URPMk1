using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ExplosiveExplode : MonoBehaviour
    {
        private float expRadius, expDamage, expPenetration, expForce, dmgTreshold;
        private LayerMask layersToDamage, layersToAffect;
        private Transform myTransform;
        private ExplosiveMaster explosiveMaster;
        private DamagableMaster damagableMaster;

        private void OnEnable()
        {
            SetInit();
            explosiveMaster.EventIgniteExplosion += Explode;
        }
        private void OnDisable()
        {
            explosiveMaster.EventIgniteExplosion -= Explode;
        }
        private void SetInit()
        {
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
            explosiveMaster = GetComponent<ExplosiveMaster>();
            ExplosiveSO myExpSO = explosiveMaster.GetExplosiveSO();
            expRadius = myExpSO.expRadius;
            expDamage = myExpSO.expDamage;
            expPenetration = myExpSO.expPenetration;
            expForce = myExpSO.expForce;
            dmgTreshold = myExpSO.dmgTreshold;
            layersToDamage = myExpSO.layersToDamage;
            layersToAffect = myExpSO.layersToAffect;
            dmgTreshold = dmgTreshold * dmgTreshold;
            myTransform = transform;
        }
        /*private void Explode()
        {
            Vector3 myPosition = myTransform.position;
            Collider[] hitColliders = Physics.OverlapSphere(myPosition, expRadius, layersToAffect);
            
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.GetComponent<Rigidbody>() != null)
                    CalculateVisibilityForce(hitColliders[i], layersToAffect, myPosition);
            }
            explosiveMaster.CallEventExplode();
        }*/
        private void Explode()
        {
            Debug.Log("Explosion called");
            Vector3 myPosition = myTransform.position;
            Collider[] hitColliders = Physics.OverlapSphere(myPosition, expRadius, layersToAffect);
            List<GameObject> dmgTransforms = new List<GameObject>(hitColliders.Length);
            int colLen = hitColliders.Length;
            for (int i = 0; i < colLen; i++)
            {
                GameObject objToDmg = hitColliders[i].gameObject;
                if (objToDmg.GetComponent<Rigidbody>() != null && !dmgTransforms.Contains(objToDmg))
                {
                    CalculateVisibilityForce(hitColliders[i], layersToAffect, myPosition);
                    dmgTransforms.Add(objToDmg);
                    Debug.Log("Calc called for: " + hitColliders[i].gameObject);
                }
            }
            explosiveMaster.CallEventExplode();
        }
        private void CalculateVisibilityForce(Collider col, LayerMask maskToCheck, Vector3 myPosition)
        {
            RaycastHit hit;
            Transform targetTransform = col.transform;
            if (Physics.Linecast(myPosition, targetTransform.position, out hit, maskToCheck))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition, col);
                    return;
                }
                else
                    CheckCorners(col, hit, targetTransform, myPosition);
            }
        }
        private void CheckCorners(Collider col, RaycastHit hit, Transform targetTransform, Vector3 myPosition)
        {
            float x, y, z;
            Vector3 v3Corner = Vector3.zero;
            Vector3 v3Center = col.bounds.center;
            Vector3 v3Extents = col.bounds.extents;

            x = v3Extents.x; y = v3Extents.y; z = v3Extents.z;
            v3Corner.x = v3Center.x; v3Corner.y = v3Center.y + y; v3Corner.z = v3Center.z;  // top middle 
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition, col);
                    return;
                }
            }
            v3Corner.x = v3Center.x + x; v3Corner.y = v3Center.y; // right middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition, col);
                    return;
                }
            }
            v3Corner.x = v3Center.x - x;  // left middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition, col);
                    return;
                }
            }
            v3Corner.x = v3Center.x; v3Corner.z = v3Center.z - z;  // right middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition, col);
                    return;
                }
            }
            v3Corner.z = v3Center.z + z;  // left middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition, col);
                    return;
                }
            }
            v3Corner.y = v3Center.y - y; v3Corner.z = v3Center.z;  // bottom middle
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition, col);
                    return;
                }
            }
        }
        private void ApplayForceAndDamage(Transform TTform, Vector3 myPosition, Collider col)
        {
            float realDmg = 0;
            float realForce = 0;
            Vector3 closestPoint = col.ClosestPointOnBounds(myPosition);
            float distanceToTarget = (closestPoint - myPosition).sqrMagnitude;
            Debug.Log("Distance to target  " + distanceToTarget);
            if (distanceToTarget <= dmgTreshold)
            {
                realForce = expForce;
                if((layersToDamage.value & (1 << TTform.gameObject.layer)) > 0)
                    realDmg = expDamage;
            }
            else
            {
                realForce = (expForce / (distanceToTarget / dmgTreshold));
                if ((layersToDamage.value & (1 << TTform.gameObject.layer)) > 0)
                    realDmg = Mathf.Abs((1 - (distanceToTarget / (expRadius*expRadius))) * expDamage);
            }
            damagableMaster.DamageObjExplosion(TTform, realDmg, expPenetration);
            Debug.Log(TTform.name + " Damaged with: " + realDmg);
            TTform.GetComponent<Rigidbody>().AddExplosionForce((realForce), myPosition, expRadius, 1, ForceMode.Impulse);
        }
    }
}
