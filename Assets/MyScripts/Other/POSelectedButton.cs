using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    public class POSelectedButton : MonoBehaviour
    {
        [SerializeField] Image myPOImage;
        [SerializeField] TMP_Text myPOText;

        public void SetUpButton(Sprite imageToSet, string textToSet)
        {
            myPOImage.sprite = imageToSet;
            myPOText.text = textToSet;
        }
    }
}
