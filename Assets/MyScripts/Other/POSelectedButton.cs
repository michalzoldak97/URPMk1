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
        int myIndex;
        private ShopManager shopManager;

        public void SetUpButton(Sprite imageToSet, string textToSet, int indexToSet, ShopManager myManager)
        {
            myPOImage.sprite = imageToSet;
            myPOText.text = textToSet;
            myIndex = indexToSet;
            shopManager = myManager;
        }
        public void RemoveMe()
        {
            shopManager.RemoveButtonFromStack(myIndex);
        }
    }
}
