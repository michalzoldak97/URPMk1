using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunHitEffects : MonoBehaviour
    {
        [SerializeField] private float hitForce;
        [SerializeField] private SoundsContainerSO mySoundsContainer;
        private Transform myTransform;
        private LayerMask stoneLayer, metalLayer, woodLayer;
        private string stoneTag, metalTag, woodTag;
        private GunMaster gunMaster;
        ObjectPooler objectPooler;
        void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            myTransform = transform;
            objectPooler = ObjectPooler.Instance;
        }
        private void Start()
        {
            SetInitials();
            GlobalReferencesSO globalReferences = GameObject.FindGameObjectWithTag("GEC").GetComponent<GlobalReferencesSO>();
            stoneLayer = globalReferences.stoneLayers;
            metalLayer = globalReferences.metalLayers;
            woodLayer = globalReferences.woodLayers;
            stoneTag = globalReferences.stoneTag;
            metalTag = globalReferences.metalTag;
            woodTag = globalReferences.woodTag;
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
                Quaternion quatAngle = Quaternion.LookRotation(hitPosition.normal);
                objectPooler.SpawnFromPoolHitEffect(stoneTag, hitPosition.point, quatAngle, hitTransform, 5);
                PlayHitSound(0, hitPosition);
            }
            else if ((metalLayer.value & (1 << layer)) > 0)
            {
                Quaternion quatAngle = Quaternion.LookRotation(hitPosition.normal);
                objectPooler.SpawnFromPoolHitEffect(metalTag, hitPosition.point, quatAngle, hitTransform, 5);
                PlayHitSound(1, hitPosition);
            }
            else if (woodLayer == (woodLayer | (1 << (layer))))
            {
                Quaternion quatAngle = Quaternion.LookRotation(hitPosition.normal);
                objectPooler.SpawnFromPoolHitEffect(woodTag, hitPosition.point, quatAngle, hitTransform, 5);
                PlayHitSound(2, hitPosition);
            }
        }
        private void ForceHit(RaycastHit hitPosition, Transform hitTransform, int layer)
        {
            if(hitTransform.GetComponent<Rigidbody>()!=null)
                hitTransform.GetComponent<Rigidbody>().AddForce(myTransform.forward * hitForce, ForceMode.Impulse);
        }
        private void PlayHitSound(int type, RaycastHit hitPosition)
        {
            int randomNum;
            if(type == 0)
            {
                randomNum = Random.Range(0, mySoundsContainer.stoneSounds.Length);
                AudioSource.PlayClipAtPoint(mySoundsContainer.stoneSounds[randomNum], hitPosition.point, 1);
            }
            else if(type == 1)
            {
                randomNum = Random.Range(0, mySoundsContainer.metalSounds.Length);
                AudioSource.PlayClipAtPoint(mySoundsContainer.metalSounds[randomNum], hitPosition.point, 1);
            }
            else if(type == 2)
            {
                randomNum = Random.Range(0, mySoundsContainer.woodSounds.Length);
                AudioSource.PlayClipAtPoint(mySoundsContainer.woodSounds[randomNum], hitPosition.point, 1);
            }
        }
    }
}