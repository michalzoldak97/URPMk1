using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class PlayerCursorManager : MonoBehaviour
    {
        private PlayerMaster playerMaster;

        private void Awake()
        {
            playerMaster = GetComponent<PlayerMaster>();
            CursorOnOff(false);
        }
        private void OnEnable()
        {
            playerMaster.EventCursorOnOff += CursorOnOff;
        }
        private void OnDisable()
        {
            playerMaster.EventCursorOnOff -= CursorOnOff;
        }
        void CursorOnOff(bool activate)
        {
            if(activate)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
