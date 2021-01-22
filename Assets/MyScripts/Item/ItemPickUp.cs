using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class ItemPickUp : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private Transform myTransform;
        void Start()
        {
            myTransform = transform;
        }

        private void OnEnable()
        {
            itemMaster = GetComponent<ItemMaster>();
            itemMaster.EventPickupRequested += PickupAction;
            itemMaster.EventObjectThrow += EnableRigidbodiesColliders;
        }

        private void OnDisable()
        {
            itemMaster.EventPickupRequested -= PickupAction;
            itemMaster.EventObjectThrow -= EnableRigidbodiesColliders;
        }

        void PickupAction(Transform tran)
        {
            DisableRigidbodies();
            myTransform.SetParent(tran);
            tran.gameObject.GetComponentInParent<PlayerMaster>().CallEventInventoryChanged();
            if (!itemMaster.shouldStayActive)
            {
                DisableColliders();
                itemMaster.CallEventObjectPickup();
                gameObject.SetActive(false);
            }
            else
                itemMaster.CallEventObjectPickup();
        }
        void DisableRigidbodies()
        {
            foreach(Rigidbody rb in GetComponents<Rigidbody>())
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
        void DisableColliders()
        {
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
            foreach (Collider childCol in GetComponentsInChildren<Collider>())
            {
                childCol.enabled = false;
            }
        }
        void EnableRigidbodiesColliders()
        {
            foreach (Rigidbody rb in GetComponents<Rigidbody>())
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = true;
            }
            foreach (Collider childCol in GetComponentsInChildren<Collider>())
            {
                childCol.enabled = true;
            }
        }
        public float GetMass()
        {
            return GetComponent<Rigidbody>().mass;
        }
    }
}
