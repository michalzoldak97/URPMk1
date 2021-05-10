using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemSetName : MonoBehaviour
    {
        [SerializeField] private string name;
       private void Start()
       {
            gameObject.name = name;
       }
    }
}
