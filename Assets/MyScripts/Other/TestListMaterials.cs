using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

namespace U1
{
    public class TestListMaterials : MonoBehaviour
    {
        // Start is called before the first frame update
        private DamagableMaster damagableMaster;
        private ExplosiveMaster explosiveMaster;
        private int splitNum;
        private float expRadius, splintDmg, expPenetration, expForce, dmgThreshold, expDamage;
        [SerializeField] private LayerMask layersToAffect, layersToDamage;
        private Vector3[] randDir;
        private Transform myTransform;
        RaycastHit hit;
        [SerializeField] Transform targetObject;

        void Start()
        {
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
            explosiveMaster = GetComponent<ExplosiveMaster>();
            myTransform = transform;
            splitNum = 50;
            expRadius = 100;
            splintDmg = 100;
            expPenetration = 1;
            expForce = 10;
            dmgThreshold = 9;
            expDamage = 350;
            randDir = new Vector3[splitNum];
            for (int i = 0; i < splitNum; i++)
            {
                randDir[i] = Random.insideUnitSphere.normalized;
            }
            StartCoroutine(StartTest());
        }

        IEnumerator StartTest()
        {
            double multiRes = 0;
            double singleRes = 0;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(5);
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Stop();
                //UnityEngine.Debug.Log("Is high resolution: " + Stopwatch.IsHighResolution);
                double avElapsedMsM = 0;
                double avElapsedTcsM = 0;
                for (int j = 0; j < 40; j++)
                {
                    stopwatch.Reset();
                    stopwatch.Start();
                    //if(isMulti)
                    TestMulti();//TestMulti(targetTransform);
                    stopwatch.Stop();
                    //UnityEngine.Debug.Log("Time elapsed multi: " + stopwatch.ElapsedMilliseconds + "\nTime elapsed multi tics: " + stopwatch.ElapsedTicks);
                    avElapsedMsM += stopwatch.ElapsedMilliseconds;
                    avElapsedTcsM += stopwatch.ElapsedTicks;
                }
                UnityEngine.Debug.Log("AVG S :-------Time elapsed TestMulti : " + avElapsedMsM / 40 + "\nTime elapsed TestMulti tics: " + avElapsedTcsM / 40);
                multiRes += avElapsedTcsM / 40;
            }
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(1);
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Stop();
                //UnityEngine.Debug.Log("Is high resolution: " + Stopwatch.IsHighResolution);
                double avElapsedMsM = 0;
                double avElapsedTcsM = 0;
                for (int j = 0; j < 40; j++)
                {
                    stopwatch.Reset();
                    stopwatch.Start();
                    //if(isMulti)
                    TestSingle();//TestMulti(targetTransform);
                    stopwatch.Stop();
                    //UnityEngine.Debug.Log("Time elapsed multi: " + stopwatch.ElapsedMilliseconds + "\nTime elapsed multi tics: " + stopwatch.ElapsedTicks);
                    avElapsedMsM += stopwatch.ElapsedMilliseconds;
                    avElapsedTcsM += stopwatch.ElapsedTicks;
                }
                UnityEngine.Debug.Log("AVG S :-------Time elapsed TestSingle : " + avElapsedMsM / 40 + "\nTime elapsed TestSingle tics: " + avElapsedTcsM / 40);
                singleRes += avElapsedTcsM / 40;
            }
            UnityEngine.Debug.Log("---AVG MULTI: " + multiRes / 10 + "  ---AVG SINGLE: " + singleRes / 10);
        }
        void TestMulti()
        {
            for (int i = 0; i < 10; i++)
            {
                PerformMultiAction();
            }
            for (int i = 0; i < 100; i++)
            {
                PerformMultiAction();
            }
        }
        void TestSingle()
        {
            for (int i = 0; i < 10; i++)
            {
                PerformSingleAction();
            }
            for (int i = 0; i < 100; i++)
            {
                PerformSingleAction();
            }
        }


        void PerformMultiAction()
        {
            Explode();
        }
        void PerformSingleAction()
        {
            Explode1();
        }
        private void Explode()
        {
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
                }
            }
            explosiveMaster.CallEventExplode();
        }
        private void Explode1()
        {
            Vector3 myPosition = myTransform.position;
            Collider[] hitColliders = Physics.OverlapSphere(myPosition, expRadius, layersToAffect);
            int colLen = hitColliders.Length;
            for (int i = 0; i < colLen; i++)
            {
                GameObject objToDmg = hitColliders[i].gameObject;
                if (objToDmg.GetComponent<Rigidbody>() != null)
                {
                    CalculateVisibilityForce(hitColliders[i], layersToAffect, myPosition);
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
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
                else
                    CheckCorners(col.bounds, hit, targetTransform, myPosition);
            }
        }
        private void CheckCorners(Bounds bounds, RaycastHit hit, Transform targetTransform, Vector3 myPosition)
        {
            float x, y, z;
            Vector3 v3Corner = Vector3.zero;
            Vector3 v3Center = bounds.center;
            Vector3 v3Extents = bounds.extents;

            x = v3Extents.x; y = v3Extents.y; z = v3Extents.z;
            v3Corner.x = v3Center.x; v3Corner.y = v3Center.y + y; v3Corner.z = v3Center.z;  // top middle 
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
            }
            v3Corner.x = v3Center.x + x; v3Corner.y = v3Center.y; // right middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
            }
            v3Corner.x = v3Center.x - x;  // left middle x
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
            }
            v3Corner.x = v3Center.x; v3Corner.z = v3Center.z - z;  // right middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
            }
            v3Corner.z = v3Center.z + z;  // left middle z
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
            }
            v3Corner.y = v3Center.y - y; v3Corner.z = v3Center.z;  // bottom middle
            if (Physics.Raycast(myPosition, v3Corner - myPosition, out hit, expRadius, layersToAffect))
            {
                if (hit.transform == targetTransform)
                {
                    ApplayForceAndDamage(targetTransform, myPosition);
                    return;
                }
            }
        }
        private void ApplayForceAndDamage(Transform TTform, Vector3 myPosition)
        {
            float realDmg = 0;
            float realForce = 0;
            float distanceToTarget = (TTform.position - myPosition).sqrMagnitude;
            distanceToTarget = Mathf.Abs(distanceToTarget);
            if (distanceToTarget <= dmgThreshold)
            {
                realForce = expForce;
                if ((layersToDamage.value & (1 << TTform.gameObject.layer)) > 0)
                    realDmg = expDamage;
            }
            else
            {
                realForce = (expForce / (distanceToTarget / dmgThreshold));
                if ((layersToDamage.value & (1 << TTform.gameObject.layer)) > 0)
                    realDmg = (expDamage / (distanceToTarget / dmgThreshold));
            }
            damagableMaster.DamageObjExplosion(TTform, realDmg, expPenetration);
            TTform.GetComponent<Rigidbody>().AddExplosionForce((realForce), myPosition, expRadius, 1, ForceMode.Impulse);
        }
    }
}
