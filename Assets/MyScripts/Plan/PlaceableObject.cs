using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [System.Serializable]
    public class PlaceableObject
    {
        public static int numOfObjTypes = 3;
        public GameObject objToSpawn;
        public Sprite objIcon;
        public Sprite objImage;
        public string objectName;
        public string objectInfo;
        public int numOfOwnedObjects;
        public int maxNumOfOwnedObjects;
        public int coinsPrice;
        public int experiencePrice;
        public int levelFromAvailable = 1;
        public int objType;
        public int numOfObjOnStack;
        public bool isAvailable;
        public bool isAddedToStack;
        public Vector3[] worldPositions;
    }
}
