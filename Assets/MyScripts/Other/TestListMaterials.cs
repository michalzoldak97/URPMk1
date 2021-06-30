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
    public struct TestListMaterialState
    {
        public bool bOne, bTwo, bThree, bFour;
    }
    public class TestListMaterials : MonoBehaviour
    {
        private TestListMaterialState myState;
        void Start()
        {
            myState = new TestListMaterialState();
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
            for (int i = 0; i < 10000; i++)
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
            for (int i = 0; i < 10000; i++)
            {
                PerformSingleAction();
            }
        }

        //bools
        bool bOne, bTwo, bThree, bFour;

        void PerformMultiAction()
        {
            CheckClass();
        }
        void PerformSingleAction()
        {
            CheckStruct();
        }
        private void CheckClass()
        {
            if(!bOne && !bTwo && !bThree && !bFour)
                DoThings();
        }
        private void CheckStruct()
        {
            if (!myState.bOne)
            {
                DoThings();
                myState.bOne = true;
            }
            else if (!myState.bTwo)
            {
                DoThings();
                myState.bTwo = true;
            }
            else if (!myState.bThree)
            {
                DoThings();
                myState.bThree = true;
            }
            else if (!myState.bFour)
            {
                DoThings();
                myState.bFour = true;
            }
            else
            {
                DoThings();
                myState.bOne = false; myState.bTwo = false; myState.bThree = false; myState.bFour = false;
            }
        }
        private void DoThings()
        {
            Vector3 oneVector = new Vector3(3, 3, 3);
            Vector3 twoVector = new Vector3(5, 5, 5);
            float res1 = Mathf.Sqrt(oneVector.y * twoVector.x);
            float res2 = res1 * Mathf.Atan(res1);
            float dist = Vector3.Distance(oneVector, twoVector);
        }
    }
}
