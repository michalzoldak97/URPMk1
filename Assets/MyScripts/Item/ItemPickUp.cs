using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class RB
    {
        public bool isRbKinematic;
        public float rbMass;
    }
    public class ItemPickUp : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private Transform myTransform;
        private Rigidbody[] myRigidbodies;
        private Collider[] myColliders;
        private Collider[] childColliders;
        private RB[] rbContainer;
        void Start()
        {
            myTransform = transform;
        }

        private void OnEnable()
        {
            itemMaster = GetComponent<ItemMaster>();
            itemMaster.EventPickupRequested += PickupAction;
            itemMaster.EventObjectThrow += RestoreCollidersRB;
        }

        private void OnDisable()
        {
            itemMaster.EventPickupRequested -= PickupAction;
            itemMaster.EventObjectThrow -= RestoreCollidersRB;
        }

        void PickupAction(Transform tran)
        {
            GetRigidbodies();
            DeactivateAllRigidbodies();
            myTransform.SetParent(tran);
            tran.gameObject.GetComponentInParent<PlayerMaster>().CallEventInventoryChanged();
            if (!itemMaster.shouldStayActive)
            {
                GetColliders();
                itemMaster.CallEventObjectPickup();
                gameObject.SetActive(false);
            }
            else
                itemMaster.CallEventObjectPickup();
        }

        void GetRigidbodies()
        {
            myRigidbodies = GetComponents<Rigidbody>();
            rbContainer = new RB[myRigidbodies.Length];
            for (int i = 0; i < rbContainer.Length; i++)
            {
                rbContainer[i] = new RB();
            }
            if (rbContainer != null && myRigidbodies.Length>0)
            {
                //Debug.Log("Not null" + rbContainer[0].rbMass);
                for (int i = 0; i < myRigidbodies.Length; i++)
                {
                    rbContainer[i].rbMass = myRigidbodies[i].mass;
                    rbContainer[i].isRbKinematic = myRigidbodies[i].isKinematic;
                }
            }
        }
        void DeactivateAllRigidbodies()
        {
            for (int i = 0; i < myRigidbodies.Length; i++)
            {
                Destroy(myRigidbodies[i]);
            }
        }
        void GetColliders()
        {
            myColliders = GetComponents<Collider>();
            childColliders = GetComponentsInChildren<Collider>();
            for (int i = 0; i < myColliders.Length; i++)
            {
                myColliders[i].enabled = false;
            }
            for (int i = 0; i < childColliders.Length; i++)
            {
                childColliders[i].enabled = false;
            }
        }
        void RestoreCollidersRB()
        {
            if (myColliders != null && myColliders.Length > 0)
            {
                for (int i = 0; i < myColliders.Length; i++)
                {
                    myColliders[i].enabled = true;
                }
            }
            if (childColliders != null && childColliders.Length > 0)
            {
                for (int i = 0; i < childColliders.Length; i++)
                {
                    childColliders[i].enabled = true;
                }
            }
            if(rbContainer != null && rbContainer.Length>0)
            {
                if (GetComponent<Rigidbody>() != null)
                    DestroyImmediate(GetComponent<Rigidbody>());
                for (int i = 0; i < rbContainer.Length; i++)
                {
                    Rigidbody toAdd = gameObject.AddComponent<Rigidbody>();
                    toAdd.isKinematic = rbContainer[i].isRbKinematic;
                    toAdd.mass = rbContainer[i].rbMass;
                }
                Array.Clear(rbContainer,0,0);
            }
        }
        public float GetMass()
        {
            float currMass = 0;
            for (int i = 0; i < rbContainer.Length; i++)
            {
                currMass += rbContainer[i].rbMass;
            }
            return currMass;
        }
    }
}
