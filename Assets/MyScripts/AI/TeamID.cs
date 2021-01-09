using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class TeamID : MonoBehaviour
    {
        [SerializeField] int myTeamID;
        private GlobalEnemyChecker GEC;
        void Awake()
        {
            GEC = GameObject.FindGameObjectWithTag("GEC").GetComponent<GlobalEnemyChecker>();
        }
        private void OnEnable()
        {
            GEC.AddToEnemyTransforms(myTeamID, transform);
        }
        private void OnDisable()
        {
            GEC.RemoveFromEnemyTransforms(myTeamID, transform);
        }
    }
}