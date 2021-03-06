using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    public class ShopInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image objImage;
        [SerializeField] private TMP_Text objInfoText;

        public void SetInfoPanel(Sprite imageToSet, string textToSet)
        {
            objImage.sprite = imageToSet;
            objInfoText.text = textToSet;
        }
    }
}
