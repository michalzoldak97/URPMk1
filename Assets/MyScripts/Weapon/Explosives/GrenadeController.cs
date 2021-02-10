using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace U1
{
    public class GrenadeController : MonoBehaviour
    {
        [SerializeField] GameObject grenade;
        [SerializeField] GameObject cotter;
        [SerializeField] int maxForce;
        [SerializeField] Slider progressBar;
        private GameObject canvas;
        private int currForce;
        private ItemMaster itemMaster;
        private float nextCheck, checkRate = 0.1f;
        private bool isOnPlayer;
        void Start()
        {
            if(progressBar.transform.parent.gameObject != null)
                canvas = progressBar.transform.parent.gameObject;
            progressBar.value = 0;
            progressBar.maxValue = maxForce;
            if (canvas != null)
                canvas.SetActive(false);
        }
        private void OnEnable()
        {
            itemMaster = GetComponent<ItemMaster>();
            SetIsOnPlayer();
            if (canvas != null && isOnPlayer)
            {
                canvas.SetActive(true);
            }
        }
        private void OnDisable()
        {
            if (canvas != null)
                canvas.SetActive(false);
            SetIsOnPlayer();
        }
        private void Update()
        {
            if(isOnPlayer)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    IncreaseForce();
                }
                if(Input.GetKeyUp(KeyCode.Mouse1))
                {
                    Throw();
                }
            }
        }

        void IncreaseForce()
        {
            if(Time.timeScale > 0 && Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                if(cotter.activeSelf)
                    cotter.SetActive(false);
                if(currForce < maxForce)
                {
                    currForce++;
                    if(progressBar != null)
                    {
                        progressBar.value = Mathf.Clamp(currForce, 0, maxForce - 1);
                    }
                }
            }
        }
        /*void ResetForce()
        {
            if (!cotter.activeSelf)
                cotter.SetActive(true);
            currForce = 0;
            if (progressBar != null)
            {
                progressBar.value = Mathf.Clamp(currForce, 0, maxForce - 1);
            }

        }*/
        void Throw()
        {
            if (Time.timeScale > 0)
            {
                GameObject go = Instantiate(grenade, transform.position, transform.rotation);
                go.GetComponent<Rigidbody>().AddForce(transform.parent.transform.forward * currForce, ForceMode.Impulse);
                StartCoroutine(RemoveObject());
            }
        }

        IEnumerator RemoveObject()
        {
            itemMaster.CallEventObjectThrowRequest();
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
        void SetIsOnPlayer()
        {
            if (gameObject.transform.root.CompareTag("Player"))
                isOnPlayer = true;
            else
                isOnPlayer = false;
        }
    }
}
