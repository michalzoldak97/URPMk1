using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [System.Serializable]
    public class PlaceableObject
    {
        public GameObject objToSpawn;
        public GameObject objButon;
        public GameObject mapAlias;
        public int numOfOwnedObjects;
        public int maxNumOfOwnedObjects;
        public int coinsPrice;
        public int experiencePrice;
        public bool isAvailable;
        public bool isAddedToStack;
        public Vector3[] worldPositions;
    }
}
