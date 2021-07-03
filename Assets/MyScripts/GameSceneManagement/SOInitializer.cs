using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class SOInitializer : MonoBehaviour
    {
        [SerializeField] private ItemSO testItemSO;
        private void Start()
        {
            testItemSO.throwForce = 1000;
        }
    }
}
