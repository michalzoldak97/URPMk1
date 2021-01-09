using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ColorDamagable : MonoBehaviour
    {
        private DamageMaster dmgMaster;
        private Renderer myRenderer;
        private float health = 100;
        private void Start()
        {
            myRenderer = GetComponent<Renderer>();
        }
        private void OnEnable()
        {
            dmgMaster = GetComponent<DamageMaster>();
            dmgMaster.EventShootByGun += ChangeColor;
        }
        void OnDisable()
        {
            dmgMaster.EventShootByGun -= ChangeColor;
        }
        void ChangeColor(float a, float b)
        {
            myRenderer.material.color = Color.green;
            health -= a;
            if (health < 0)
            {
                Destroy(gameObject, Random.Range(8,12));
                gameObject.SetActive(false);
            }
        }
    }
}
