using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class ItemDeactivateAfterUsage : MonoBehaviour
    {
        private GunMaster gunMaster;
        private ItemMaster itemMaster;

        private void OnEnable()
        {
            gunMaster = GetComponent<GunMaster>();
            itemMaster = GetComponent<ItemMaster>();
            gunMaster.EventGunShoot += StartDeactivate;
        }
        private void OnDisable()
        {
            gunMaster.EventGunShoot -= StartDeactivate;
        }
        private void StartDeactivate()
        {
            StartCoroutine(Deactivate());
        }
        private IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(0.5f);
            itemMaster.CallEventObjectThrowRequest();
            gameObject.layer = 0;
            yield return new WaitForSeconds(2f);
            Destroy(gameObject, 12);
            gameObject.SetActive(false);
        }
    }
}
