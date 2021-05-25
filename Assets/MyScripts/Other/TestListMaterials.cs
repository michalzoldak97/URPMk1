using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;

namespace U1
{
    public class TestListMaterials : MonoBehaviour
    {
        // Start is called before the first frame update
        private DamagableMaster damagableMaster;
        private int splitNum;
        private float splintRange, splintDmg, expPenetration, expForce;
        [SerializeField] private LayerMask layersToAffect, layersToDamage;
        private Vector3[] randDir;
        private Transform myTransform;
        RaycastHit hit;

        void Start()
        {
            damagableMaster = GameObject.FindGameObjectWithTag("DamagableMaster").GetComponent<DamagableMaster>();
            myTransform = transform;
            splitNum = 50;
            splintRange = 100;
            splintDmg = 100;
            expPenetration = 1;
            expForce = 10;
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
            ExplodeShrad();
        }
        void PerformSingleAction()
        {
            ExplodeShrad1();
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
                    Transform target = hit.transform;
                    if ((toDmg.value & (1 << target.gameObject.layer)) > 0)
                        ApplayForceAndDamage1(target, myPos);
                }
            }
        }
        private void ExplodeShrad1()
        {
            Vector3[] directions = randDir;
            Vector3 myPos = myTransform.position;
            float range = splintRange;
            LayerMask toAffect = layersToAffect;
            LayerMask toDmg = layersToDamage;
            for (int i = 0; i < splitNum; i++)
            {
                if (Physics.Raycast(myPos, directions[i], out hit, range, toAffect))
                {
                    Transform target = hit.transform;
                    if ((toDmg.value & (1 << target.gameObject.layer)) > 0)
                        ApplayForceAndDamage1(target, myPos);
                }
            }
        }
        private void ApplayForceAndDamage(Transform TTform, Vector3 myPosition)
        {
            damagableMaster.DamageObjGun(TTform, splintDmg, expPenetration);
            try
            {
                TTform.GetComponent<Rigidbody>().AddExplosionForce((expForce / 10), myPosition, 10, 1, ForceMode.Impulse);
            }
            catch
            {
                return;
            }
        }
        private void ApplayForceAndDamage1(Transform TTform, Vector3 myPosition)
        {
            damagableMaster.DamageObjGun(TTform, splintDmg, expPenetration);
            if(TTform.GetComponent<Rigidbody>() != null)
                TTform.GetComponent<Rigidbody>().AddExplosionForce((expForce / 10), myPosition, 10, 1, ForceMode.Impulse);
        }
    }
}
