using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        #region Singleton
        public static ObjectPooler Instance;

        //public Transform[] wayPoints;

        private void Awake()
        {
            Instance = this;
        }
        #endregion
        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                    if (obj.GetComponent<DeactivateForPool>() != null)
                    {
                        obj.GetComponent<DeactivateForPool>().SetInits();
                    }
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }
        public GameObject SpawnFromPoolHitEffect(string tag, Vector3 position, Quaternion rotation, Transform hitTransform, float time)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.Log("Pool with tag " + tag + " doesn't exist");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.SetParent(hitTransform);

            if(objectToSpawn.GetComponent<DeactivateForPool>()!=null)
            {
                objectToSpawn.GetComponent<DeactivateForPool>().OnObjectSpwan();
            }

            poolDictionary[tag].Enqueue(objectToSpawn);

            StartCoroutine(Deactivatevoi(objectToSpawn, time));
            return objectToSpawn;
        }

        IEnumerator Deactivatevoi(GameObject obj, float time)
        {
            yield return new WaitForSeconds(time);
            obj.SetActive(true);
            yield return new WaitForFixedUpdate();
            obj.transform.SetParent(null);
            obj.SetActive(false);
        }
    }
}
