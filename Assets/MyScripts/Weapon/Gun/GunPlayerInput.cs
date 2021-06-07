using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    [System.Serializable]public class BurstFireSetting
    {
        public bool hasABurstMode;
        public bool isShootingBurst;
        public int shootsInBurst;
        public float burstFireRate;
    }
    public class GunPlayerInput : MonoBehaviour
    {
        [SerializeField] bool isAutomatic;
        [SerializeField] float shootRate;
        [SerializeField] float aimSpeed;
        public BurstFireSetting myBurtsFire = new BurstFireSetting();
        private float shootDelay, nextCheck, currFPSSpeed;
        private GunMaster gunMaster;
        private PlayerMaster playerMaster;
        private FPSController fpsController;
        private bool shouldChangeSpeed;
        private bool isOnPlayer;
        void SetInitials()
        {
            gunMaster = GetComponent<GunMaster>();
            playerMaster = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMaster>();
            fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
            shootDelay = 60 / shootRate;
        }
        private void Start()
        {
            BurstOnOff(); BurstOnOff();
            AutomaticOnOff(); AutomaticOnOff();
        }
        private void OnEnable()
        {
            SetInitials();
            SetIsOnPlayer();
        }
        private void OnDisable()
        {
            SetIsOnPlayer();
        }

        private void Update()
        {
            if(isOnPlayer)
            {
                CheckForShoot();
                CheckForReload();
                CheckForAim();
            }
        }

        void CheckForShoot()
        {
            if (!playerMaster.isInventoryOn)
            {
                if (Input.GetKey(KeyCode.Mouse0) && isAutomatic && Time.timeScale > 0 && Time.time > nextCheck && !myBurtsFire.hasABurstMode)
                {
                    nextCheck = Time.time + shootDelay;
                    gunMaster.CallEventShootRequest();
                }
                else if (Input.GetKeyDown(KeyCode.Mouse0) && !isAutomatic && Time.timeScale > 0 && Time.time > nextCheck && !myBurtsFire.hasABurstMode)
                {
                    nextCheck = Time.time + shootDelay;
                    gunMaster.CallEventShootRequest();
                    Debug.Log("Shoot requested");
                }
                else if(Input.GetKeyDown(KeyCode.Mouse0) && Time.timeScale > 0 && Time.time > nextCheck && myBurtsFire.hasABurstMode && !myBurtsFire.isShootingBurst)
                {
                    StartCoroutine(ShootBurstFire());
                }
            }
        }
        void CheckForReload()
        {
            if (Input.GetKeyDown(KeyCode.R) && Time.timeScale > 0)
            {
                gunMaster.CallEventReloadRequest();
                //Debug.Log("Reload requested");
            }
        }
        void CheckForAim()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && Time.timeScale > 0)
            {
                gunMaster.CallEventAimRequest();
                float[] playerWalkSpeed = fpsController.GetWalkSpeed();
                if (playerWalkSpeed[0] > (playerWalkSpeed[1] - (aimSpeed * playerWalkSpeed[1])))
                {
                    //Debug.Log("Aim requested");
                    //shouldChangeSpeed = true;
                    currFPSSpeed = fpsController.GetWalkSpeed()[0];
                    fpsController.SetMotionParams(aimSpeed, aimSpeed, aimSpeed);
                }
                //else
                    //shouldChangeSpeed = false;
            }
            else if(Input.GetKeyUp(KeyCode.Mouse1) && Time.timeScale > 0)
            {
                gunMaster.CallEventUnAim();
                //if (shouldChangeSpeed)
                //{
                    currFPSSpeed = (1 - (currFPSSpeed / fpsController.GetWalkSpeed()[1]));
                    fpsController.SetMotionParams(currFPSSpeed, currFPSSpeed, currFPSSpeed);
                //}
                //Debug.Log("Aim not requested");
            }
        }
        void SetIsOnPlayer()
        {
            if (gameObject.transform.root.CompareTag("Player"))
                isOnPlayer = true;
            else
                isOnPlayer = false;
        }

        IEnumerator ShootBurstFire()
        {
            myBurtsFire.isShootingBurst = true;
            for (int i = 0; i < myBurtsFire.shootsInBurst; i++)
            {
                nextCheck = Time.time + myBurtsFire.burstFireRate;
                if (isOnPlayer)
                    gunMaster.CallEventShootRequest();
                else
                {
                    myBurtsFire.isShootingBurst = false;
                    break;
                }
                yield return new WaitForSecondsRealtime(myBurtsFire.burstFireRate);
            }
            myBurtsFire.isShootingBurst = false;
        }
        public void BurstOnOff()
        {
            myBurtsFire.hasABurstMode = !myBurtsFire.hasABurstMode;
            if(GetComponent<GunUI>()!= null)
            {
                GetComponent<GunUI>().BurstModeChanged(myBurtsFire.hasABurstMode);
            }
        }
        public void AutomaticOnOff()
        {
            isAutomatic = !isAutomatic;
            if (GetComponent<GunUI>() != null)
            {
                GetComponent<GunUI>().AutomaticModeChanged(isAutomatic);
            }
        }
    }
}
