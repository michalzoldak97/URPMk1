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
        [SerializeField] private Transform targetTransform;
        private float nextCheck;
        bool isOnPlayer = true;

        Vector3 toSet;
        // multi part

        //single part

        void Start()
        {
            StartCoroutine(StartTest());
            //multi part
            toSet = transform.position;
        }

        IEnumerator StartTest()
        {
            yield return new WaitForSecondsRealtime(3);
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Stop();
            //UnityEngine.Debug.Log("Is high resolution: " + Stopwatch.IsHighResolution);
            double avElapsedMsM = 0;
            double avElapsedTcsM = 0;
            for (int i = 0; i < 5; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                //if(isMulti)
                TestMulti(targetTransform);//TestMulti(targetTransform);
                stopwatch.Stop();
                //UnityEngine.Debug.Log("Time elapsed multi: " + stopwatch.ElapsedMilliseconds + "\nTime elapsed multi tics: " + stopwatch.ElapsedTicks);
                avElapsedMsM += stopwatch.ElapsedMilliseconds;
                avElapsedTcsM += stopwatch.ElapsedTicks;
            }
            UnityEngine.Debug.Log("AVG S :-------Time elapsed multi : " + avElapsedMsM / 5 + "\nTime elapsed multi tics: " + avElapsedTcsM / 5);
        }
        void TestMulti(Transform target)
        {
            for (int i = 0; i < 10; i++)
            {
                PerformMultiAction(target);
            }
            for (int i = 0; i < 100000; i++)
            {
                PerformMultiAction(target);
            }
        }
        void TestSingle(Transform target)
        {
            for (int i = 0; i < 10; i++)
            {
                PerformSingleAction(target);
            }
            for (int i = 0; i < 100000; i++)
            {
                PerformSingleAction(target);
            }
        }

        void PerformMultiAction(Transform target)
        {
            Vector3 v3Center = Vector3.zero;
            float x = 0.5f;
            toSet.Set(v3Center.x + x, v3Center.y + 0.1f, v3Center.z);
        }
        void PerformSingleAction(Transform target)
        {
            Vector3 v3Center = Vector3.zero;
            float x = 0.5f;
            //toSet.x = v3Center.x + x; toSet.y = v3Center.y + 0.1f; toSet.z = v3Center.z;
            toSet = SetVector3(toSet, v3Center.x + x, toSet.y = v3Center.y + 0.1f, toSet.z = v3Center.z);
        }

        private Vector3 SetVector3(Vector3 toSet, float _x, float _y, float _z)
        {
            toSet.x = _x; toSet.y = _y; toSet.z = _z;
            return toSet;
        }
    }
}
