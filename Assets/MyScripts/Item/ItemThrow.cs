using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemThrow : MonoBehaviour
    {
        [SerializeField] private bool shouldNotBeThrown;
        private Transform parentTransform;
        private Transform rootTransform;
        private ItemMaster itemMaster;
        private void OnEnable()
        {
            itemMaster = GetComponent<ItemMaster>();
            itemMaster.EventObjectThrowRequest += Throw;
        }
        private void OnDisable()
        {
            itemMaster.EventObjectThrowRequest -= Throw;
        }
        void Throw()
        {
            if (!shouldNotBeThrown)
            {
                rootTransform = transform.root.transform;
                parentTransform = transform.parent.transform;
                transform.SetParent(null);
                itemMaster.CallEventObjectThrow();
                rootTransform.GetComponent<PlayerMaster>().CallEventInventoryChanged();
                gameObject.GetComponent<Rigidbody>().AddForce(parentTransform.forward * itemMaster.GetItemSO().throwForce, ForceMode.Impulse);
            }
        }
    }
}
