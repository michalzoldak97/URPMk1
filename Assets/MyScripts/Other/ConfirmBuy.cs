using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    public class ConfirmBuy : MonoBehaviour
    {
        public int POIndex { get; private set; }
        public void SetPOIndex(int toSet)
        {
            POIndex = toSet;
        }
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private GameObject buyButton;
        [SerializeField] private ShopManager shopManager;
        
        public void CancelBuying()
        {
            POIndex = 999999999;
            StopAllCoroutines();
            buyButton.SetActive(true);
            infoText.text = "Are you sure?";
            gameObject.SetActive(false);
        }
        public void CallBuyPO()
        {
            shopManager.BuyPO(POIndex);
            POIndex = 999999999;
        }
        public void StartDisplayErrorMessage(string toDisplay)
        {
            StartCoroutine(DisplayErrorMessage(toDisplay));
        }
        IEnumerator DisplayErrorMessage(string toDisplay)
        {
            buyButton.SetActive(false);
            infoText.text = toDisplay;
            yield return new WaitForSeconds(3);
            buyButton.SetActive(true);
            infoText.text = "Are you sure?";
            gameObject.SetActive(false);
        }
    }
}
