using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GlobalEnemyChecker : MonoBehaviour
    {
        private Dictionary<int, List<Transform>> enemyTransforms = new Dictionary<int, List<Transform>>();

        public void AddToEnemyTransforms(int myID, Transform myTransform)
        {
           if(enemyTransforms.ContainsKey(myID))
           {
                enemyTransforms[myID].Add(myTransform);
                //Debug.Log("GEC Add id: " + myID + " transform " + myTransform.name);
           }
           else
           {
                enemyTransforms.Add(myID, new List<Transform>());
                enemyTransforms[myID].Add(myTransform);
           }
        }
        public void RemoveFromEnemyTransforms(int myID, Transform myTransform)
        {
            if (enemyTransforms.ContainsKey(myID) && enemyTransforms[myID].Contains(myTransform))
                enemyTransforms[myID].Remove(myTransform);
        }
        public List<Transform> GetEnemyList(int enemyID)
        {
            if (enemyTransforms.ContainsKey(enemyID))
                return enemyTransforms[enemyID];
            else
                return null;
        }
    }
}
