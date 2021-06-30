using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class ItemPhysics : MonoBehaviour
    {
        public void RigidbodiesActivateDeactivate(bool toState)
        {
            foreach(Rigidbody rb in GetComponents<Rigidbody>())
            {
                rb.isKinematic = !toState;
                rb.useGravity = toState;
            }
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = !toState;
                rb.useGravity = toState;
            }
        }
        public void CollidersActivateDeactivate(bool toState)
        {
            foreach (Collider col in GetComponents<Collider>())
            {
                col.isTrigger = !toState;
            }
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.isTrigger = !toState;
            }
        }
    }
}
