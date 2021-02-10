using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1 
{
    public class UpdateBehaviour : MonoBehaviour
    {
        private UpdateManager updateManager;

        private void Awake()
        {
            updateManager =  GameObject.FindGameObjectWithTag("GEC").GetComponent<UpdateManager>();
            updateManager.AddToList(this);
        }
        private void OnDisable()
        {
            updateManager.RemoveFromList(this);
        }
        public virtual void GetUpdate()
        {

        }
    }
}
