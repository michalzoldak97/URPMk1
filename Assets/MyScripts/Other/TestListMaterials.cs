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

        void Start()
        {
            StartCoroutine(StartTest());
        }

        IEnumerator StartTest()
        {
            yield return new WaitForSecondsRealtime(3);
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Stop();
            //UnityEngine.Debug.Log("Is high resolution: " + Stopwatch.IsHighResolution);
            double avElapsedMsM = 0;
            double avElapsedTcsM = 0;
            for (int i = 0; i < 40; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                //if(isMulti)
                TestSingle(targetTransform);//TestMulti(targetTransform);
                stopwatch.Stop();
                //UnityEngine.Debug.Log("Time elapsed multi: " + stopwatch.ElapsedMilliseconds + "\nTime elapsed multi tics: " + stopwatch.ElapsedTicks);
                avElapsedMsM += stopwatch.ElapsedMilliseconds;
                avElapsedTcsM += stopwatch.ElapsedTicks;
            }
            UnityEngine.Debug.Log("AVG S :-------Time elapsed multi : " + avElapsedMsM / 40 + "\nTime elapsed multi tics: " + avElapsedTcsM / 40);
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
            Vector3 v3Center = transform.position;

            Vector3 v3Corner = Vector3.zero;

            v3Corner.x = v3Center.x; v3Corner.y = v3Center.y + 0.1f; v3Corner.z = -v3Center.z;
        }
        void PerformSingleAction(Transform target)
        {
            Vector3 v3Center = transform.position;

            Vector3 v3Corner = Vector3.zero;

            v3Corner.Set(v3Center.x, v3Center.y + 0.1f, -v3Center.z);
        }
    }
}
