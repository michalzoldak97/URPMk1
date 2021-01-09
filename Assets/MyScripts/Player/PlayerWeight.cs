using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace U1
{
    public class PlayerWeight : MonoBehaviour
    {
        [SerializeField] float maxMass;
        [SerializeField] TMP_Text massText;
        private float inventoryMass;
        private float walkSpeed, sprintSpeed, jumpSpeed;
        private FPSController fpsController;
        private void Start()
        {
            fpsController = GetComponent<FPSController>();
        }

        void SetSpeed(float curMass)
        {
            //Debug.Log("Set speed with: " + curMass);
            if(curMass > maxMass)
            {
                fpsController.SetMotionParams(1, 1, 1);
            }
            else if(curMass > (maxMass/10f))
            {
                walkSpeed = (curMass / (maxMass + curMass));
                fpsController.SetMotionParams(walkSpeed, walkSpeed, walkSpeed);
            }
            else
                fpsController.SetMotionParams(0, 0, 0);
        }
        public void ChangeMass(float massToAdd)
        {
            //Debug.Log("Change mass called, inventory mass:  " + inventoryMass + "  To be add is:  " + massToAdd);
            inventoryMass += massToAdd;
            SetSpeed(inventoryMass);
            massText.text = inventoryMass.ToString("N1") + " kg";
        }
    }
}
