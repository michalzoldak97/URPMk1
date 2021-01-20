using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace U1
{
    public class Explosion1 : MonoBehaviour
    {
        [SerializeField] private GameObject effect;
        [SerializeField] private LayerMask layersToDamage, layersToAffect;
        [SerializeField] private float[] expDmg;
        [SerializeField] private float radius, hitForce, timeToExplode;
        [SerializeField] private int treshold;
        //AudioSource myAudio;
        private Transform myTransform, effectTransform;
        //private Transform targetTransform;
        private DamagableMaster damagableMaster;
        void SetInits()
        {
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
            treshold = treshold * treshold;
            effectTransform = effect.transform;
            myTransform = transform;
            StartCoroutine(Explode());
        }
        private void OnEnable()
        {
            SetInits();
        }
        IEnumerator Explode()
        {
            yield return new WaitForSecondsRealtime(timeToExplode);
            Vector3 myPosition = myTransform.position;
            Collider[] hitColliders = Physics.OverlapSphere(myPosition, radius, layersToAffect);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                //add damage
                if (hitColliders[i].gameObject.GetComponent<Rigidbody>() != null)
                {
                    Debug.Log("Found " + hitColliders[i].name + " hit colider length: " + hitColliders.Length);
                    CalculateVisibilityForce(hitColliders[i], layersToAffect, myPosition);
                }
                /*else if (layersToDamage == (layersToDamage | (1 << (hitColliders[i].gameObject.layer))))
                {

                }*/
            }
            effectTransform.parent = null;
            effect.SetActive(true);
            Destroy(gameObject, 0.1f);
        }

        void CalculateVisibilityForce(Collider col, LayerMask maskToCheck, Vector3 myPosition)
        {
            RaycastHit hit;
            Transform targetTransform = col.transform;
            if (Physics.Linecast(myPosition, targetTransform.position, out hit , maskToCheck))
            {
                if(hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
                else
                {
                    //Debug.Log("Do things!....  " + targetTransform.name);
                    CheckCorners(col.bounds, hit, targetTransform, myPosition);
                }
            }
        }
        void CheckCorners(Bounds bounds, RaycastHit hit, Transform targetTransform, Vector3 myPosition)
        {
            //Debug.Log("CheckCorners");
            float x, y, z;
            Vector3 v3Corner = Vector3.zero;
            Vector3 v3Center;
            Vector3 v3Extents;

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
                    ApplayForceAndDamage(targetTransform, myPosition);
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
                    ApplayForceAndDamage(targetTransform, myPosition);
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
                    ApplayForceAndDamage(targetTransform, myPosition);
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
                    ApplayForceAndDamage(targetTransform, myPosition);
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
                    ApplayForceAndDamage(targetTransform, myPosition);
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
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
            }
        }
        void ApplayForceAndDamage(Transform TTform, Vector3 myPosition)
        {
            float realDmg = 0;
            float realForce = 0;
            float distanceToTarget = (TTform.position - myPosition).sqrMagnitude;
            distanceToTarget = Mathf.Abs(distanceToTarget);
            if (distanceToTarget <= treshold)
            {
                realForce = hitForce;
                if (layersToDamage == (layersToDamage | (1 << (TTform.gameObject.layer))))
                {
                    realDmg = expDmg[0];
                    //Debug.Log("Less than one" + distanceToTarget);
                }
            }
            else
            {
                realForce = (hitForce / (distanceToTarget / treshold));
                if (layersToDamage == (layersToDamage | (1 << (TTform.gameObject.layer))))
                {
                    realDmg = (expDmg[0] / (distanceToTarget / treshold));
                    //Debug.Log("Morre than one" + distanceToTarget + " divide: " + (distanceToTarget / treshold));
                }
            }
            damagableMaster.DamageObjExplosion(TTform, realDmg, expDmg[1]);
            Debug.Log(TTform.name + " Damaged with: " + realDmg);
            TTform.GetComponent<Rigidbody>().AddExplosionForce((realForce), myPosition, radius, 1, ForceMode.Impulse);
        }
    }
}
