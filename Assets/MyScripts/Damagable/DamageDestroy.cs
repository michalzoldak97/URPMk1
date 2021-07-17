using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamageDestroy : MonoBehaviour
    {
        private DamageMaster dmgMaster;
        private byte destrTime = 15;

        void SetInits()
        {
            dmgMaster = GetComponent<DamageMaster>();
        }
        private void OnEnable()
        {
            SetInits();
            dmgMaster.EventDestruction += DestroyMe;
        }
        private void OnDisable()
        {
            dmgMaster.EventDestruction -= DestroyMe;
        }
        private void DestroyMe()
        {
            dmgMaster.CallEventDestroyEffects();
            Destroy(gameObject, destrTime);
            gameObject.SetActive(false);
        }
    }
}
