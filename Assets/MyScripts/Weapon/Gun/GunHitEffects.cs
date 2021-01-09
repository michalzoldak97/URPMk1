using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunHitEffects : MonoBehaviour
    {
        private GunMaster gunMaster;
        private Transform myTransform;
        private int randomNum;
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
            if(stoneLayer == (stoneLayer | (1 << (layer))))
            {
                quatAngle = Quaternion.LookRotation(hitPosition.normal);
                //quatAngle = Quaternion.FromToRotation(Vector3.up, hitPosition.normal);
                //Debug.Log("hitPosition.point equalz:  " + hitPosition.point);
                objectPooler.SpawnFromPool(stoneTag, hitPosition.point, quatAngle, hitTransform);
                PlayHitSound(0, hitPosition);
            }
            else if (metalLayer == (metalLayer | (1 << (layer))))
            {
                quatAngle = Quaternion.LookRotation(hitPosition.normal);
                //Debug.Log("hitPosition.point equalz:  " + hitPosition.point);
                objectPooler.SpawnFromPool(metalTag, hitPosition.point, quatAngle, hitTransform);
                PlayHitSound(1, hitPosition);
            }
            else if (woodLayer == (woodLayer | (1 << (layer))))
            {
                quatAngle = Quaternion.LookRotation(hitPosition.normal);
                //Debug.Log("hitPosition.point equalz:  " + hitPosition.point);
                objectPooler.SpawnFromPool(woodTag, hitPosition.point, quatAngle, hitTransform);
                PlayHitSound(2, hitPosition);
            }
        }
        void ForceHit(RaycastHit hitPosition, Transform hitTransform, int layer)
        {
            if(hitTransform.GetComponent<Rigidbody>()!=null)
                hitTransform.GetComponent<Rigidbody>().AddForce(myTransform.forward * hitForce, ForceMode.Impulse);
        }
        void PlayHitSound(int type, RaycastHit hitPosition)
        {
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