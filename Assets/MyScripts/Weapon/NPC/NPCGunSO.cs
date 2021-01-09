using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [CreateAssetMenu(menuName = "NpcGun")]
    public class NPCGunSO : ScriptableObject
    {
        public int numOfShoots;
        public int maxAmmo;
        public int maxRange;
        public int reloadTime;
        public float shootRate;
        public float recoil;
        public LayerMask layersToShoot;
        public float[] dmgEquation; // 1 - multiplayer, 2 - constant
        public float[] penetrationCoeff;
    }
}
