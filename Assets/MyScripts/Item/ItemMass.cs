using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemMass : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private ItemPickUp itemPickUp;
        private PlayerWeight playerWeight;
        private float myMass;
        void Start()
        {
            itemMaster = GetComponent<ItemMaster>();
            itemPickUp = GetComponent<ItemPickUp>();
            if(transform.root.GetComponent<PlayerWeight>()!=null)
            {
                playerWeight = transform.root.GetComponent<PlayerWeight>();
                playerWeight.ChangeMass(GetComponent<Rigidbody>().mass);
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
            myMass = itemPickUp.GetMass();
            playerWeight = transform.root.GetComponent<PlayerWeight>();
            playerWeight.ChangeMass(myMass);
        }
        void RemoveMass()
        {
            playerWeight.ChangeMass(-myMass);
        }
    }
}
