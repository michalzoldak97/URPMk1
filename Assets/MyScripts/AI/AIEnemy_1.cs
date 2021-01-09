using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [CreateAssetMenu(menuName ="AI Enemy 1")]
    public class AIEnemy_1 : ScriptableObject
    {
        public int[] myEnemyIDs;
        public int sightRange;
        public int attackRange;
        public LayerMask sightLayers;
        public LayerMask enemyLayers;
        public float baseCheckRate;
        public float navMeshAgentSpeed;
        public float onDamageStopTime;
    }
}
