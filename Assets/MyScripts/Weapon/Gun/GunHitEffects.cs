using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunHitEffects : MonoBehaviour
    {
        private GunMaster gunMaster;
        private Transform myTransform;
        ObjectPooler objectPooler;
        Quaternion quatAngle;
        [SerializeField] LayerMask metalLayer;
        [SerializeField] string metalTag;
        [SerializeField] LayerMask stoneLayer;
        [SerializeField] string stoneTag;
        [SerializeField] LayerMask woodLayer;
        [SerializeField] string woodTag;
        [SerializeField] float hitForce;
        [SerializeField] SoundsContainerSO mySoundsContainer;
        void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            myTransform = transform;
            objectPooler = ObjectPooler.Instance;
        }
        private void Start()
        {
            SetInitials();
        }

        void OnEnable()
        {
            SetInitials();
            gunMaster.EventHit += SpawnHitEffect;
            gunMaster.EventHit += ForceHit;
        }
        void OnDisable()
        {
            gunMaster.EventHit -= SpawnHitEffect;
            gunMaster.EventHit -= ForceHit;
        }
        void SpawnHitEffect(RaycastHit hitPosition, Transform hitTransform, int layer)
        {   
            if ((stoneLayer.value & (1  << layer)) > 0)
            {
                quatAngle = Quaternion.LookRotation(hitPosition.normal);
                //Debug.Log("hitPosition.point equalz:  " + hitPosition.point);
                objectPooler.SpawnFromPoolHitEffect(stoneTag, hitPosition.point, quatAngle, hitTransform, 5);
                PlayHitSound(0, hitPosition);
            }
            else if ((metalLayer.value & (1 << layer)) > 0)
            {
                quatAngle = Quaternion.LookRotation(hitPosition.normal);
                //Debug.Log("hitPosition.point equalz:  " + hitPosition.point);
                objectPooler.SpawnFromPoolHitEffect(metalTag, hitPosition.point, quatAngle, hitTransform, 5);
                PlayHitSound(1, hitPosition);
            }/*
            else if (woodLayer == (woodLayer | (1 << (layer))))
            {
                quatAngle = Quaternion.LookRotation(hitPosition.normal);
                //Debug.Log("hitPosition.point equalz:  " + hitPosition.point);
                objectPooler.SpawnFromPoolHitEffect(woodTag, hitPosition.point, quatAngle, hitTransform, 5);
                PlayHitSound(2, hitPosition);
            }*/
        }
        void ForceHit(RaycastHit hitPosition, Transform hitTransform, int layer)
        {
            if(hitTransform.GetComponent<Rigidbody>()!=null)
                hitTransform.GetComponent<Rigidbody>().AddForce(myTransform.forward * hitForce, ForceMode.Impulse);
        }
        void PlayHitSound(int type, RaycastHit hitPosition)
        {
            int randomNum;
            switch (type)
            {
                case (0):
                    randomNum = Random.Range(0, mySoundsContainer.stoneSounds.Length);
                    AudioSource.PlayClipAtPoint(mySoundsContainer.stoneSounds[randomNum], hitPosition.point,1);
                    //Debug.Log("Played: " + randomNum);
                    break;
                case (1):
                    randomNum = Random.Range(0, mySoundsContainer.metalSounds.Length);
                    AudioSource.PlayClipAtPoint(mySoundsContainer.metalSounds[randomNum], hitPosition.point,1);
                    //Debug.Log("Played: " + randomNum);
                    break;
                case (2):
                    randomNum = Random.Range(0, mySoundsContainer.woodSounds.Length);
                    AudioSource.PlayClipAtPoint(mySoundsContainer.woodSounds[randomNum], hitPosition.point,1);
                    //Debug.Log("Played: " + randomNum);
                    break;
                default:
                    break;
            }
        }
    }
}