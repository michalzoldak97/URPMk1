using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{

    public class GrenadeBundle : MonoBehaviour
    {
        [SerializeField] GameObject[] grenadeFake;
        [SerializeField] GameObject grenade;
        [SerializeField] float timeToRelease, spreadForce, spreadDist;
        [SerializeField] int numOfObj;
        private Transform myTransform;

        private void Start()
        {
            myTransform = transform;
            StartCoroutine(ReleaseGrenades());
        }
        private IEnumerator ReleaseGrenades()
        {
            yield return new WaitForSeconds(timeToRelease);
            for (int i = 0; i < numOfObj; i++)
            {
                GameObject gr = Instantiate(grenade, grenadeFake[i].transform.position, grenadeFake[i].transform.rotation);
                gr.GetComponent<Rigidbody>().AddExplosionForce(spreadForce, myTransform.position, spreadDist);
                grenadeFake[i].SetActive(false);
            }
            Destroy(gameObject, 10);
        }
    }
}
