using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    [System.Serializable]
    public class AmmoType
    {
        [SerializeField] private string ammoName;
        [SerializeField] private int currentQuantity;
        [SerializeField] private int maxQuantity;
        public void AddQuantity(int toAdd)
        {
            currentQuantity += toAdd;
        }
        public void SetQuantity(int toSet)
        {
            currentQuantity = toSet;
        }
        public int GetQuantity()
        {
            return currentQuantity;
        }
        public int GetMaxQuantity()
        {
            return maxQuantity;
        }
        public string GetName()
        {
            return ammoName;
        }
    }
    public class PlayerAmmo : MonoBehaviour
    {
        Dictionary<string, AmmoType> ammoDictionary = new Dictionary<string, AmmoType>();
        [SerializeField] private AmmoType[] ammoTypes;
        private PlayerMaster playerMaster;
        void Awake()
        {
            if (ammoDictionary.Count>0)
            {
                ammoDictionary.Clear();
            }
            for (int i = 0; i < ammoTypes.Length; i++)
            {
                ammoDictionary.Add(ammoTypes[i].GetName(), ammoTypes[i]);
            }
        }
        private void OnEnable()
        {
            playerMaster = GetComponent<PlayerMaster>();
            playerMaster.EventPickedUpAmmo += AddAmmo;
        }
        private void OnDisable()
        {
            playerMaster.EventPickedUpAmmo -= AddAmmo;
        }

        public void AddAmmo(string ammoType, int quantity)
        {
            if (ammoDictionary[ammoType].GetQuantity() + quantity > ammoDictionary[ammoType].GetMaxQuantity())
                ammoDictionary[ammoType].SetQuantity(ammoDictionary[ammoType].GetMaxQuantity()); 
            else
                ammoDictionary[ammoType].AddQuantity(quantity); 
        }
        public int TakeAmmo(string ammoType, int quantityRequested)
        {
            if(quantityRequested> ammoDictionary[ammoType].GetQuantity())
            {
                int quantityAvailable = ammoDictionary[ammoType].GetQuantity();
                ammoDictionary[ammoType].SetQuantity(0);
                return quantityAvailable;
            }
            else
            {
                ammoDictionary[ammoType].AddQuantity(-quantityRequested); 
                return quantityRequested;
            }
        }
        public int GetAmmoNum(string ammoType)
        {
            return ammoDictionary[ammoType].GetQuantity();
        }
    }
}
