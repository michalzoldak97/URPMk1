using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GunShootEffects : MonoBehaviour
    {
        private GunMaster gunMaster;
        private AudioSource myAudio;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private AudioClip shootingSound;
        [SerializeField] private AudioClip reloadSound;
        private void SetInitials()
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
        private void PlayMuzzleFlash()
        {
            if (muzzleFlash != null)
                muzzleFlash.Play();
        }
        private void PlayShootSound()
        {
            if(myAudio!=null)
                myAudio.PlayOneShot(shootingSound, 1f);
        }
        private void PlayReloadSound()
        {
            if (myAudio != null)
                myAudio.PlayOneShot(reloadSound, 1f);
        }
    }
}
