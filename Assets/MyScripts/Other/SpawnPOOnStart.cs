using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class SpawnPOOnStart : MonoBehaviour
    {
        [SerializeField] private GameObject objectToSpawn;
        [SerializeField] private int numToSpawn;
        [SerializeField] private float maxSpawnRadius, heigth;
        public void SpawnObject()
        {
            StartCoroutine(SpawnAfterWait());
        }
        IEnumerator SpawnAfterWait()
        {
            yield return new WaitForSecondsRealtime(3);
            for (int i = 0; i < numToSpawn; i++)
            {
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * maxSpawnRadius;
                spawnPosition.y += heigth;
                Debug.Log("Spawn IN progess  y coordinate = " + spawnPosition.y + " spawn pos= " + spawnPosition);
                Instantiate(objectToSpawn, spawnPosition, Quaternion.Euler(0f, 0f, 0f));
            }
            Destroy(gameObject, 0.5f);
        }
    }
}
