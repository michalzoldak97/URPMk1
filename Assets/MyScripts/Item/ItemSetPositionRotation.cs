using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemSetPositionRotation : MonoBehaviour
    {
        [SerializeField] private Vector3 positionToSet;
        [SerializeField] private Vector3 rotationToSet;
        private ItemMaster itemMaster;
        void Start()
        {
            Initialize();
        }
        private void OnEnable()
        {
            Initialize();
            itemMaster.EventObjectPickup += SetPosition;
            itemMaster.EventObjectPickup += SetRotation;
        }
        private void OnDisable()
        {
            itemMaster.EventObjectPickup -= SetPosition;
            itemMaster.EventObjectPickup -= SetRotation;
        }
        void Initialize()
        {
            itemMaster = GetComponent<ItemMaster>();
            if(transform.root.CompareTag("Player"))
            {
                SetPosition();
                SetRotation();
            }
        }
        void SetPosition()
        {
            transform.localPosition = positionToSet;
        }
        void SetRotation()
        {
            transform.localRotation = Quaternion.Euler(rotationToSet);
        }
    }
}
