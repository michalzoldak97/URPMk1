using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace U1
{
    public class Explosion1 : MonoBehaviour
    {
        [SerializeField] GameObject effect;
        [SerializeField] LayerMask layersToDamage;
        [SerializeField] LayerMask layersToAffect;
        [SerializeField] float[] expDmg;
        float realDmg;
        [SerializeField] float radius;
        [SerializeField] float hitForce;
        float realForce;
        [SerializeField] int treshold;
        [SerializeField] float timeToExplode;
        //AudioSource myAudio;
        private Vector3 myPosition;
        private Vector3 targetPosition;
        private float distanceToTarget;
        private Transform myTransform;
        private Transform effectTransform;
        private Transform targetTransform;
        private Collider[] hitColliders;
        private RaycastHit hit;
        private DamagableMaster damagableMaster;
        float x, y, z;

        Vector3 v3Corner = Vector3.zero;
        Vector3 v3Center = Vector3.zero;
        Vector3 v3Extents = Vector3.zero;
        void SetInits()
        {
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
            treshold = treshold * treshold;
            effectTransform = effect.transform;
            myTransform = transform;
            StartExplosion();
        }
        private void OnEnable()
        {
            SetInits();
        }
        void StartExplosion()
        {
            StartCoroutine(Explode());
        }
        IEnumerator Explode()
        {
            yield return new WaitForSecondsRealtime(timeToExplode);
            myPosition = myTransform.position;
            hitColliders = Physics.OverlapSphere(myPosition, radius, layersToAffect);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                //add damage
                if (hitColliders[i].GetComponent<Rigidbody>() != null)
                {
                    CalculateVisibilityForce(hitColliders[i], layersToAffect);
                }
                else if (layersToDamage == (layersToDamage | (1 << (hitColliders[i].gameObject.layer))))
                {

                }
            }
            effectTransform.parent = null;
            effect.SetActive(true);
            Destroy(gameObject, 0.1f);
        }

        void CalculateVisibilityForce(Collider col, LayerMask maskToCheck)
        {
            targetTransform = col.transform;
            targetPosition = targetTransform.position;
            if (Physics.Linecast(myPosition, targetPosition, out hit , maskToCheck))
            {
                if(hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, targetPosition);
                    return;
                }
                else
                {
                    //Debug.Log("Do things!....  " + targetTransform.name);
                    CheckCorners(myPosition, col.bounds);
                }
            }
        }
        void CheckCorners(Vector3 startPos, Bounds bounds)
        {
            //Debug.Log("CheckCorners");
            v3Center = bounds.center;
            v3Extents = bounds.extents;

            x = v3Extents.x; y = v3Extents.y; z = v3Extents.z;
            v3Corner.Set(v3Center.x, v3Center.y + y, v3Center.z);  // top middle 
            if (Physics.Raycast(myPosition, v3Corner - myPosition,out hit, radius*10, layersToAffect)) 
            {
                //Debug.Log("Shootin Front top middle " + hit.transform.name + "   pos: " + hit.point);
                if (hit.transform == targetTransform)
                {
                    //Debug.Log("Front top middle ");
                    ApplayForceAndDamage(targetTransform, targetPosition);
                    return;
                }
            }
            v3Corner.Set(v3Center.x + x, v3Center.y, v3Center.z);  // right middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, radius * 10, layersToAffect))
            {
                //Debug.Log("Shootin right middle x " + hit.transform.name + "   pos: " + hit.point);
                if (hit.transform == targetTransform)
                {
                    //Debug.Log("right middle x ");
                    ApplayForceAndDamage(targetTransform, targetPosition);
                    return;
                }
            }
            v3Corner.Set(v3Center.x - x, v3Center.y, v3Center.z);  // left middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, radius * 10, layersToAffect))
            {
                //Debug.Log("Shootin left middle x " + hit.transform.name + "   pos: " + hit.point);
                if (hit.transform == targetTransform)
                {
                    //Debug.Log("left middle x ");
                    ApplayForceAndDamage(targetTransform, targetPosition);
                    return;
                }
            }
            v3Corner.Set(v3Center.x, v3Center.y, v3Center.z-z);  // right middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, radius * 10, layersToAffect))
            {
                //Debug.Log("Shootinright middle z " + hit.transform.name + "   pos: " + hit.point);
                if (hit.transform == targetTransform)
                {
                    //Debug.Log("right middle z ");
                    ApplayForceAndDamage(targetTransform, targetPosition);
                    return;
                }
            }
            v3Corner.Set(v3Center.x, v3Center.y, v3Center.z + z);  // left middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, radius * 10, layersToAffect))
            {
                //Debug.Log("Shootin left middle z" + hit.transform.name + "   pos: " + hit.point);
                if (hit.transform == targetTransform)
                {
                    //Debug.Log("left middle z ");
                    ApplayForceAndDamage(targetTransform, targetPosition);
                    return;
                }
            }
            v3Corner.Set(v3Center.x, v3Center.y - y, v3Center.z);  // bottom middle
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, radius * 10, layersToAffect))
            {
                //Debug.Log("Shootin Bottom middle " + hit.transform.name + "   pos: " + hit.point);
                if (hit.transform == targetTransform)
                {
                    //Debug.Log("Bottom middle");
                    ApplayForceAndDamage(targetTransform, targetPosition);
                    return;
                }
            }
        }
        void ApplayForceAndDamage(Transform TTform , Vector3 Tpos)
        {
            distanceToTarget = (targetPosition - myPosition).sqrMagnitude;
            distanceToTarget = Mathf.Abs(distanceToTarget);
            if (distanceToTarget <= treshold)
            {
                realForce = hitForce;
                if (layersToDamage == (layersToDamage | (1 << (targetTransform.gameObject.layer))))
                {
                    realDmg = expDmg[0];
                }
                //Debug.Log("Less than one" + distanceToTarget);
            }
            else
            {
                //Debug.Log("Morre than one" + distanceToTarget + " divide: " + (distanceToTarget / treshold));
                realForce = (hitForce / (distanceToTarget / treshold));
                if (layersToDamage == (layersToDamage | (1 << (targetTransform.gameObject.layer))))
                {
                    realDmg = (expDmg[0] / (distanceToTarget / treshold));
                }
            }
            damagableMaster.DamageObjExplosion(TTform, realDmg, expDmg[1]);
            targetTransform.GetComponent<Rigidbody>().AddExplosionForce((realForce), myPosition, radius, 1, ForceMode.Impulse);
        }
    }
}
