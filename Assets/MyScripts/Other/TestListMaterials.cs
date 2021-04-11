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
        [SerializeField] GameObject testObject, normalObject;
        private GunSingleShoot singleShoot;

        void Start()
        {
            singleShoot = normalObject.GetComponent<GunSingleShoot>();
            StartCoroutine(StartTest());
        }

        IEnumerator StartTest()
        {
            testObject.SetActive(false);
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
            }
            normalObject.SetActive(false);
            testObject.SetActive(true);
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
            }
        }
        void TestMulti()
        {
            for (int i = 0; i < 10; i++)
            {
                PerformMultiAction();
            }
            for (int i = 0; i < 100000; i++)
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
            for (int i = 0; i < 100000; i++)
            {
                PerformSingleAction();
            }
        }

        void PerformMultiAction()
        {
            singleShoot.Shoot();
        }
        void PerformSingleAction()
        {
            //singleShootTest.Shoot();
        }
    }
}
