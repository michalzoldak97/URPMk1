using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
public class POSpawner : MonoBehaviour
    {
        private SceneStartManager startManager;
        private void Start()
        {
            startManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            SpawnPoObjects();
        }
        private void SpawnPoObjects()
        {
            PlaceableObject[] objTovalid = startManager.GetPlaceableObjects();
            Vector3 zeroVector = new Vector3(-100, -100, -100);
            for (int i = 0; i < objTovalid.Length; i++)
            {
                if(objTovalid[i].isAvailable && objTovalid[i].isAddedToStack)
                {
                    for (int j = 0; j < objTovalid[i].numOfObjOnStack; j++)
                    {
                        try
                        {
                            if (objTovalid[i].worldPositions[j] != zeroVector)
                            {
                                GameObject spawnedPO = Instantiate(objTovalid[i].objToSpawn, objTovalid[i].worldPositions[j], Quaternion.Euler(90f, 0f, 0f));
                                spawnedPO.GetComponent<SpawnPOOnStart>().SpawnObject();
                            }
                            else
                            {
                                Debug.Log("Is zero vector for " + objTovalid[i].objectName);
                            }
                        }
                        catch
                        {
                            Debug.Log("Wrong pos index or not set");
                        }
                    }
                }
            }
        }
    }
}
