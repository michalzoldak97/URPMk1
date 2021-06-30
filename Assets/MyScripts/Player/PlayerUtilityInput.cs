using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class PlayerUtilityInput : MonoBehaviour
    {
        [SerializeField] GameObject canvasInventory;
        private PlayerMaster playerMaster;
        private void Start()
        {
            playerMaster = GetComponent<PlayerMaster>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                CanvasOnOf();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                CursorOnOff();
            }
        }
        private void CanvasOnOf()
        {
            if (canvasInventory.activeSelf)
            {
                playerMaster.CallEventControllerFreeze(false);
                playerMaster.CallEventCursorOnOff(false);
                playerMaster.isInventoryOn = false;
                canvasInventory.SetActive(false);
            }
            else
            {
                playerMaster.CallEventControllerFreeze(true);
                playerMaster.CallEventCursorOnOff(true);
                playerMaster.isInventoryOn = true;
                canvasInventory.SetActive(true);
            }
        }
        private void CursorOnOff()
        {
            playerMaster.CallEventCursorOnOff(Cursor.lockState == CursorLockMode.Locked);
            playerMaster.CallEventControllerFreeze(!(Cursor.lockState == CursorLockMode.Locked));
            playerMaster.isInventoryOn = !(Cursor.lockState == CursorLockMode.Locked);
        }
    }
}

