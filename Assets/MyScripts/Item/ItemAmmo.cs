using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemAmmo : MonoBehaviour
    {
        [SerializeField] string ammoName;
        [SerializeField] int ammoQuantity;
        private ItemMaster itemMaster;
        private void OnEnable()
        {
            itemMaster = GetComponent<ItemMaster>();
            itemMaster.EventPickupRequested += AddAmmoToPlayer;
        }
        private void OnDisable()
        {
            itemMaster.EventPickupRequested -= AddAmmoToPlayer;
        }
        void AddAmmoToPlayer(Transform cameraTransform)
        {
            itemMaster.playerTransform.GetComponent<PlayerMaster>().CallEventPickedUpAmmo(ammoName, ammoQuantity);
            Destroy(gameObject);
        }
    }
}
