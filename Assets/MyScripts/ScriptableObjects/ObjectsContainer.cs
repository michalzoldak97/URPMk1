using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [CreateAssetMenu(menuName = "ObjectsContainer")]
    public class ObjectsContainer : ScriptableObject
    {
        public GameObject[] objecsSet1;
        public int combinationNum;
    }
}
