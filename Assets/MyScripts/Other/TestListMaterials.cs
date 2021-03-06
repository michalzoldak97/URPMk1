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
        private SceneStartManager sceneStartManager;

        void Start()
        {
            sceneStartManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            StartCoroutine(StartTest());
        }

        IEnumerator StartTest()
        {
            yield return new WaitForSecondsRealtime(1);
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
                TestMulti(targetTransform);//TestMulti(targetTransform);
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
            for (int i = 0; i < 10000; i++)
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
            for (int i = 0; i < 10000; i++)
            {
                PerformSingleAction(target);
            }
        }

        void PerformMultiAction(Transform target)
        {
            for (int i = 0; i < sceneStartManager.GetPlaceableObjects().Length; i++)
            {
                sceneStartManager.GetPlaceableObjects()[i].objectName = "test name" + i.ToString();
            }
        }
        void PerformSingleAction(Transform target)
        {
            PlaceableObject[] myPO = sceneStartManager.GetPlaceableObjects();
            int POLength = myPO.Length;
            for (int i = 0; i < POLength; i++)
            {
                myPO[i].objectName = "test name" + i.ToString();
            }
        }
    }
}
