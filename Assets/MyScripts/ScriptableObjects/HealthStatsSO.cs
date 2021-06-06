using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [CreateAssetMenu(menuName = "HealthStats")]
    public class HealthStatsSO : ScriptableObject
    {
        public int health;
        public float armor;
        public string dmgText;
    }
}
