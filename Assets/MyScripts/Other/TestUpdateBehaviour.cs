using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class TestUpdateBehaviour : UpdateBehaviour
    {
        /*void Update()
        {
            TestTask();
        }*/
        public override void GetUpdate()
        {
            TestTask();
        }

        void TestTask()
        {
            gameObject.GetComponent<Transform>().Rotate(Vector3.up, 1);
        }

    }
}
