using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunShootEffects : MonoBehaviour
    {
        private GunMaster gunMaster;
        private AudioSource myAudio;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] AudioClip shootingSound;
        [SerializeField] AudioClip reloadSound;
        void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            myAudio = GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            SetInitials();
            gunMaster.EventGunShoot += PlayMuzzleFlash;
            gunMaster.EventGunShoot += PlayShootSound;
            gunMaster.EventReload += PlayReloadSound;
        }
        private void OnDisable()
        {
            gunMaster.EventGunShoot -= PlayMuzzleFlash;
            gunMaster.EventGunShoot -= PlayShootSound;
            gunMaster.EventReload -= PlayReloadSound;
        }
        void PlayMuzzleFlash()
        {
            if (muzzleFlash != null)
                muzzleFlash.Play();
        }
        void PlayShootSound()
        {
            if(myAudio!=null)
                myAudio.PlayOneShot(shootingSound, 1f);
        }
        void PlayReloadSound()
        {
            if (myAudio != null)
                myAudio.PlayOneShot(reloadSound, 1f);
        }
    }
}
