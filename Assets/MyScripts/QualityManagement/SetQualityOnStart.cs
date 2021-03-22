using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class SetQualityOnStart : MonoBehaviour
    {

        void Start()
        {
            //QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 30;
        }
    }
}
