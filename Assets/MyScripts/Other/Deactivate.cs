using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class Deactivate : MonoBehaviour
    {
        public float waitingTime = 5;
        public bool randomTime;
        public bool toDeactivate;
        // Start is called before the first frame update
        public void OnObjectSpawn()
        {
            StartCoroutine(Deactivatevoi(gameObject));
        }
        void OnEnable()
        {
            StartCoroutine(Deactivatevoi(gameObject));
        }

        IEnumerator Deactivatevoi(GameObject obj)
        {
            if (!randomTime) 
            { 
                yield return new WaitForSeconds(waitingTime);
                if (!toDeactivate)
                {
                    Destroy(obj, Random.Range(10, 15));
                    gameObject.SetActive(false);
                }
                else
                {
                    transform.SetParent(null);
                    obj.SetActive(false);
                }
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(waitingTime, waitingTime + 3));
                Destroy(obj, Random.Range(10, 15));
                gameObject.SetActive(false);
            }
        }
    }
}
