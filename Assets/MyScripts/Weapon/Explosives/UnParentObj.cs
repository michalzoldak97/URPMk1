using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class UnParentObj : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(UnParent());
        }
        IEnumerator UnParent()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.SetParent(null);
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
            Destroy(gameObject, 10);
        }
    }
}
