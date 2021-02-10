using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading;

public class TestListMaterials : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform targetTransform;

    // multi part

    [SerializeField] Transform[] weaponTransforms;
    private Transform myTransform;
    private Vector3[] baseAimPosition;
    private Quaternion lookAtRotation;

    //single part

    [SerializeField] protected Transform weaponTransform;

    void AssignWeaponPositions()
    {
        baseAimPosition = new Vector3[weaponTransforms.Length * 2];
        for (int i = 0; i < weaponTransforms.Length; i++)
        {
            baseAimPosition[i * 2] = weaponTransforms[i].localPosition;
        }
    }
    void Start()
    {
        StartCoroutine(StartTest());
        //multi part
        myTransform = transform;
        AssignWeaponPositions();
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
            TestSingle(targetTransform);//TestMulti(targetTransform);
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
        Vector3 targetPos = target.position;
        for (int i = 0; i < weaponTransforms.Length; i++)
        {
            int a = (i * 2) + 1;
            baseAimPosition[a] = weaponTransforms[i].position + weaponTransforms[i].forward * (Vector3.Distance(myTransform.position, targetPos));
            targetPos.x = baseAimPosition[a].x; targetPos.z = baseAimPosition[a].z;
            lookAtRotation = Quaternion.LookRotation((targetPos - weaponTransforms[i].position).normalized);
            weaponTransforms[i].rotation = Quaternion.Slerp(weaponTransforms[i].rotation, lookAtRotation, 5);
            baseAimPosition[a] = baseAimPosition[i * 2];
        }
    }
    void PerformSingleAction(Transform target)
    {
        Quaternion testRotation = Quaternion.LookRotation(targetTransform.position - weaponTransform.position, Vector3.up);
        testRotation.y = 0; testRotation.z = 0;
        weaponTransform.localRotation = testRotation;
    }

}
