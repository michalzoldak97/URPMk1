using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class SpawnPOOnStart : MonoBehaviour
    {
        [SerializeField] private GameObject objectToSpawn;
        [SerializeField] private int numToSpawn;
        [SerializeField] private float maxSpawnRadius;
        void Start()
        {
            SpawnObject();
        }
        private void SpawnObject()
        {
            Transform myTransform = transform;
            for (int i = 0; i < numToSpawn; i++)
            {
                Vector3 spawnPosition = myTransform.position + Random.insideUnitSphere * maxSpawnRadius;
                GameObject obj = Instantiate(objectToSpawn, spawnPosition, Quaternion.Euler(0f, 0f, 0f));
            }
            Destroy(gameObject, 0.5f);
        }
    }
}
