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
        private List<UpdateBehaviour> toUpdate = new List<UpdateBehaviour>();
        UpdateBehaviour u1 = new UpdateBehaviour();
        UpdateBehaviour u2 = new UpdateBehaviour();
        UpdateBehaviour u3 = new UpdateBehaviour();
        UpdateBehaviour u4 = new UpdateBehaviour();
        UpdateBehaviour u5 = new UpdateBehaviour();
        UpdateBehaviour u6 = new UpdateBehaviour();
        UpdateBehaviour u7 = new UpdateBehaviour();
        UpdateBehaviour u8 = new UpdateBehaviour();
        UpdateBehaviour u9 = new UpdateBehaviour();

        void Start()
        {
            UpdateBehaviour u1 = new UpdateBehaviour();
            UpdateBehaviour u2 = new UpdateBehaviour();
            UpdateBehaviour u3 = new UpdateBehaviour();
            UpdateBehaviour u4 = new UpdateBehaviour();
            UpdateBehaviour u5 = new UpdateBehaviour();
            UpdateBehaviour u6 = new UpdateBehaviour();
            UpdateBehaviour u7 = new UpdateBehaviour();
            UpdateBehaviour u8 = new UpdateBehaviour();
            UpdateBehaviour u9 = new UpdateBehaviour();
            toUpdate.Add(u1); toUpdate.Add(u2); toUpdate.Add(u3); toUpdate.Add(u4); toUpdate.Add(u5); toUpdate.Add(u6); toUpdate.Add(u7); toUpdate.Add(u8); toUpdate.Add(u9);
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
            for (int i = 0; i < 1; i++)
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
            for (int i = 0; i < 1000000; i++)
            {
                PerformMultiAction(target);
            }
        }
        void TestSingle(Transform target)
        {
            /*for (int i = 0; i < 10; i++)
            {
                PerformSingleAction(target);
            }
            for (int i = 0; i < 1000000; i++)
            {
                PerformSingleAction(target);
            }*/
            PerformSingleAction(target);
        }

        void PerformMultiAction(Transform target)
        {
            int count = toUpdate.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    toUpdate[i].GetUpdate();
                }
            }
        }
        void PerformSingleAction(Transform target)
        {
            int counter = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    counter++;
                    if (i < 2)
                    {
                        UnityEngine.Debug.Log(counter);
                    }
                    else
                        break;
                }
            }
            UnityEngine.Debug.Log("Final" + counter);
        }
    }
}
