using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [CreateAssetMenu(menuName = "ExplosiveSO")]
    public class ExplosiveSO : ScriptableObject
    {
        public float timeToExplode, expRadius, expDamage, expPenetration, expForce, dmgTreshold, splintRange, splintDmg;
        public LayerMask layersToDamage, layersToAffect;
    }
}