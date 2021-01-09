using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DeactivateForPool : MonoBehaviour
    {
        private ParticleSystem myParticleSystem;
        public void SetInits()
        {
            myParticleSystem = GetComponent<ParticleSystem>();
        }
        public void OnObjectSpwan()
        {
            myParticleSystem.Play();
        }
    }
}
