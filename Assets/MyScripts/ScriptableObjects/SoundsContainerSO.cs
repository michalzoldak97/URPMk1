using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    [CreateAssetMenu(menuName = "SoundsContainer")]
    public class SoundsContainerSO : ScriptableObject
    {
        public AudioClip[] metalSounds;
        public AudioClip[] stoneSounds;
        public AudioClip[] woodSounds;
    }
}
