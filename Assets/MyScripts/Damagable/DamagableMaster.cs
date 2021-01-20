using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class DamagableMaster : MonoBehaviour
    {
        Dictionary<Transform, DamageMaster> damagableDict = new Dictionary<Transform, DamageMaster>();
        public void AddToDictionary(Transform objTransform, DamageMaster objDmgMaster)
        {
            damagableDict.Add(objTransform, objDmgMaster);
            Debug.Log(objTransform.name + " registered in dict");
        }
        public void RemoveFromDictionary(Transform objTransform)
        {
            damagableDict.Remove(objTransform);
            //Debug.Log(objTransform.name + " removed from dict");
        }
        public void DamageObjGun(Transform objTransform, float dmgAmount, float penetration)
        {
            if(damagableDict.ContainsKey(objTransform))
                damagableDict[objTransform].CallEventShootByGun(dmgAmount, penetration);
        }
        public void DamageObjExplosion(Transform objTransform, float dmgAmount, float penetration)
        {
            if (damagableDict.ContainsKey(objTransform))
                damagableDict[objTransform].CallHitByExplosion(dmgAmount, penetration);
        }
    }
}
