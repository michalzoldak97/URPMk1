using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace U1
{
    [System.Serializable]
    public class PlaceableObject
    {
        public GameObject objToSpawn;
        public GameObject mapAlias;
        public Sprite objIcon;
        public Sprite objImage;
        public string objectName;
        public string objectInfo;
        public int numOfOwnedObjects;
        public int maxNumOfOwnedObjects;
        public int coinsPrice;
        public int experiencePrice;
        public bool isAvailable;
        public bool isAddedToStack;
        public Vector3[] worldPositions;
    }
}
