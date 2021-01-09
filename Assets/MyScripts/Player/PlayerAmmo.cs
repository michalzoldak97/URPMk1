using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    [System.Serializable]
    public class AmmoType
    {
        public string ammoName;
        public int currentQuantity;
        public int maxQuantity;
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
                ammoDictionary.Add(ammoTypes[i].ammoName, ammoTypes[i]);
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
            //Debug.Log(ammoDictionary[ammoType].ammoName + " to add " + quantity);
            if (ammoDictionary[ammoType].currentQuantity + quantity > ammoDictionary[ammoType].maxQuantity)
                ammoDictionary[ammoType].currentQuantity = ammoDictionary[ammoType].maxQuantity;
            else
                ammoDictionary[ammoType].currentQuantity += quantity;

            //Debug.Log(ammoDictionary[ammoType].ammoName + " " + ammoDictionary[ammoType].currentQuantity);
        }
        public int TakeAmmo(string ammoType, int quantityRequested)
        {
            if(quantityRequested> ammoDictionary[ammoType].currentQuantity)
            {
                int quantityAvailable = ammoDictionary[ammoType].currentQuantity;
                ammoDictionary[ammoType].currentQuantity = 0;
                return quantityAvailable;
            }
            else
            {
                ammoDictionary[ammoType].currentQuantity -= quantityRequested;
                return quantityRequested;
            }
        }
        public int GetAmmoNum(string ammoType)
        {
            return ammoDictionary[ammoType].currentQuantity;
        }
    }
}
