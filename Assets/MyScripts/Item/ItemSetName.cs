using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemSetName : MonoBehaviour
    {
        private ItemMaster itemMaster;
       private void Start()
       {
            itemMaster = GetComponent<ItemMaster>();
            gameObject.name = itemMaster.GetItemSO().itemName;
       }
    }
}
