using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemSounds : MonoBehaviour
    {
        [SerializeField] AudioClip[] hitSounds;
        [SerializeField] AudioClip throwSound;
        private ItemMaster itemMaster;
        private int soundNum;
        private bool isItem;

        private void Awake()
        {
            if (GetComponent<ItemMaster>() != null)
            {
                itemMaster = GetComponent<ItemMaster>();
                isItem = true;
            }
        }
        private void OnEnable()
        {
            if(isItem)
                itemMaster.EventObjectThrow += PlayThrowSound;
        }
        private void OnDisable()
        {
            if(isItem)
                itemMaster.EventObjectThrow -= PlayThrowSound;
        }
       
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != 8)
            {
                soundNum = Random.Range(0, hitSounds.Length - 1);
                AudioSource.PlayClipAtPoint(hitSounds[soundNum], transform.position);
            }
        }
        private void PlayThrowSound()
        {
            AudioSource.PlayClipAtPoint(throwSound, transform.position);
        }
    }
}
