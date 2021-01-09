using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    public Transform toRotate;
    public bool rotateUpwards;
    float tan;
    Vector3 dir;
    void Update()
    {
        dir = toRotate.forward;
        if(rotateUpwards)
        {
            tan = -(Mathf.Rad2Deg*Mathf.Asin(0.5f));
            //tan = tan * 0.0174533f;
            //Debug.Log(" Tan: " + tan);
            dir = Quaternion.AngleAxis(tan, toRotate.right) * dir;
        }
        Debug.DrawRay(toRotate.position,dir*10, Color.green, 2);
    }

}
