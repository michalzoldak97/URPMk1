using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [CreateAssetMenu(menuName = "ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public string itemName;
        public Vector3 posToSet;
        public Vector3 rotToSet;
        public int toLayer;
        public int throwForce;
    }
}
