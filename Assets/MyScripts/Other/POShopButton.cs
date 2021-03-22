using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace U1
{
    public class POShopButton : MonoBehaviour
    {
        private int myPOIndex;
        [SerializeField] private TMP_Text objectName, objOwned, objAvailable, objPrice;
        [SerializeReference] private GameObject imageAvailability;
        private ShopManager shopManager;

        public void SetUpPOButton(int index, PlaceableObject myPO, ShopManager myShopManager)
        {
            shopManager = myShopManager;
            myPOIndex = index;
            objectName.text = myPO.objectName;
            objOwned.text = myPO.numOfOwnedObjects.ToString();
            objAvailable.text = myPO.maxNumOfOwnedObjects.ToString();
            objPrice.text = myPO.coinsPrice.ToString();
        }
        public void SetAvailabilityImage(bool imageStateToSet)
        {
            imageAvailability.SetActive(imageStateToSet);
        }
        public void InformOnInfoEnter(Transform toPass)
        {
            shopManager.OnInfoButtonEnter(myPOIndex, toPass);
        }
        public void InformOnInfoExit()
        {
            shopManager.OnInfoButtonExit(myPOIndex);
        }
        public void BuyObject()
        {
            shopManager.BuyPO(myPOIndex);
        }
        public void AddButtonToStack()
        {
            shopManager.AddButtonToStack(myPOIndex);
        }
    }
}
