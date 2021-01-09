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
            yield return new WaitForSecondsRealtime(0.5f);
            gameObject.transform.SetParent(null);
            Destroy(gameObject, 3);
        }
    }
}
