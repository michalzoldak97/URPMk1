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
        [SerializeField] private Transform targetTransform, weaponTransform;
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

        float maxUp = 80;
        float maxDown = 5;
        void PerformMultiAction(Transform target)
        {
            Quaternion testRotation = Quaternion.LookRotation(targetTransform.position - weaponTransform.position, Vector3.up);
            testRotation.y = 0; testRotation.z = 0;
            float maxUpTransformed = -maxUp / 180;
            float maxDownTransformed = -maxDown / 180;
            if (testRotation.x < maxUpTransformed)
                testRotation.x = maxUpTransformed;
            else if (testRotation.x > maxDownTransformed)
                testRotation.x = maxDownTransformed;
            weaponTransform.localRotation = testRotation;
        }
        void PerformSingleAction(Transform target)
        {
            /*Quaternion testRotation = Quaternion.LookRotation(targetTransform.position - weaponTransform.position, Vector3.up);
            Vector3 vRotation = testRotation.eulerAngles;
            weaponTransform.rotation = Quaternion.Euler(Mathf.Clamp(vRotation.x, -maxDown, -maxUp), vRotation.y, vRotation.z);*/
            Vector3 vRotation = Quaternion.LookRotation(targetTransform.position - weaponTransform.position, Vector3.up).eulerAngles;
            if (vRotation.x < -maxUp)
                vRotation.x = -maxUp;
            else if (vRotation.x > -maxDown)
            {
                vRotation.x = -maxDown;
            }
            weaponTransform.rotation = Quaternion.Euler(vRotation.x, vRotation.y, vRotation.z);
        }
    }
}
