using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemThrow : MonoBehaviour
    {
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
        private void Throw()
        {
            if (!itemMaster.isInTransitionState)
            {
                itemMaster.SetIsInTransitionState(true);
                Transform itemParent = transform.parent;
                transform.parent = null;
                itemMaster.SetItemPhysics(true);
                itemMaster.playerTransform.GetComponent<PlayerInventoryMaster>().CallEventRemoveItem(transform);
                gameObject.GetComponent<Rigidbody>().AddForce(itemParent.forward * itemMaster.GetItemSO().throwForce, ForceMode.Impulse);
                itemMaster.CallEventObjectThrow();
            }
        }
    }
}
