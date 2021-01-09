using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemChangeLayer : MonoBehaviour
    {
        private ItemMaster itemMaster;
        [SerializeField] byte changeToLayer;
        private byte originalLayer;

        void Start()
        {
            originalLayer = (byte)gameObject.layer;
        }

        void SetInit()
        {
            itemMaster = GetComponent<ItemMaster>();
        }

        private void OnEnable()
        {
            SetInit();
            itemMaster.EventObjectPickup += ChangeOnPickup;
            itemMaster.EventObjectThrow += ChangeOnThrow;
        }
        private void OnDisable()
        {
            itemMaster.EventObjectPickup -= ChangeOnPickup;
            itemMaster.EventObjectThrow -= ChangeOnThrow;
        }

        void ChangeOnPickup()
        {
            gameObject.layer = changeToLayer;
            foreach(Transform child in transform)
            {
                child.gameObject.layer = changeToLayer;
            }
        }
        void ChangeOnThrow()
        {
            gameObject.layer = originalLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = originalLayer;
            }
        }
    }
}
