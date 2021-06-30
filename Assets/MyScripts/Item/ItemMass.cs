using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemMass : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private PlayerWeight playerWeight;
        void Start()
        {
            itemMaster = GetComponent<ItemMaster>();
            if(transform.root.GetComponent<PlayerWeight>()!=null)
            {
                playerWeight = transform.root.GetComponent<PlayerWeight>();
                playerWeight.ChangeMass(itemMaster.GetMass());
            }
        }
        private void OnEnable()
        {
            itemMaster = GetComponent<ItemMaster>();
            itemMaster.EventObjectPickup += AddMass;
            itemMaster.EventObjectThrow += RemoveMass;
        }
        private void OnDisable()
        {
            itemMaster.EventObjectPickup -= AddMass;
            itemMaster.EventObjectThrow -= RemoveMass;
        }
        void AddMass()
        {
            playerWeight = itemMaster.playerTransform.GetComponent<PlayerWeight>();
            playerWeight.ChangeMass(itemMaster.GetMass());
        }
        void RemoveMass()
        {
            playerWeight.ChangeMass(-itemMaster.GetMass());
        }
    }
}
