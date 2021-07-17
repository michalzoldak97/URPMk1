using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    public class TextDmg : MonoBehaviour
    {
        private Transform myTransform, cameraTransform;
        private TMP_Text myText;
        private WaitForSeconds delay = new WaitForSeconds(0.05f);
        public void SetText(string toSet)
        {
            myText.text = toSet;
        }
        private void Awake()
        {
            SetInit();
        }
        private void OnEnable()
        {
            StartCoroutine(TextLifespan());
        }
        private void SetInit()
        {
            myTransform = transform;
            myText = GetComponent<TMP_Text>();
            cameraTransform = Camera.main.transform;
        }
        private IEnumerator TextLifespan()
        {
            Color32 startColor = new Color32(255, 255, 255, 255);
            Vector3 posToGo = myTransform.position;
            posToGo.y += 4;
            posToGo = posToGo + Random.insideUnitSphere * 3;
            for (int i = 0; i < 60; i++)
            {
                yield return delay;
                myTransform.LookAt(cameraTransform);
                myTransform.rotation = Quaternion.LookRotation(cameraTransform.forward);
                myTransform.position = Vector3.Lerp(myTransform.position, posToGo, 0.1f);
                if (startColor.a > 8)
                {
                    startColor.a -= 8;
                    myText.color = startColor;
                }
                else if(startColor.a > 0)
                {
                    startColor.a = 0;
                    myText.color = startColor; 
                    StopCoroutine(TextLifespan());
                }
            }
        }
    }
}
