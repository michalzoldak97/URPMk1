using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class OnOffUpdate : MonoBehaviour
    {
        private ItemMaster itemMaster;
        [SerializeField] MonoBehaviour[] myScripts;

        void SetInit()
        {
            itemMaster = GetComponent<ItemMaster>();
        }
        void Start()
        {
            SetInit();
            StartCoroutine(AtBegin());
        }
        private void OnEnable()
        {
            SetInit();
            itemMaster.EventPickupRequested += OnMono;
            itemMaster.EventObjectThrow += OffMono;
        }
        private void OnDisable()
        {
            itemMaster.EventPickupRequested -= OnMono;
            itemMaster.EventObjectThrow -= OffMono;
        }

        void OnMono(Transform dummy)
        {
            //Debug.Log("On Update");
            for (int i = 0; i < myScripts.Length; i++)
            {
                myScripts[i].enabled = true;
            }
        }
        void OffMono()
        {
            for (int i = 0; i < myScripts.Length; i++)
            {
                myScripts[i].enabled = false;
            }
        }

        IEnumerator AtBegin()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            if (transform.parent == null)
            {
                OffMono();
            }
            else if (!transform.parent.CompareTag("Player"))
            {
                OffMono();
            }
        }
    }
}
