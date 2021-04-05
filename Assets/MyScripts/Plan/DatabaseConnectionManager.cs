using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class DatabaseConnectionManager : MonoBehaviour
    {
        [SerializeField] string loadDataOnLogInURL, updateTaskStatusesURL, saveMaxLevelURL, updatePOURL, buyPOURL;
        private int myPlayerID;
        public void SetPlayerID(int toSet)
        {
            myPlayerID = toSet;
        }
        public int GetPlayerID()
        {
            return myPlayerID;
        }
        private SceneStartManager startManager;
        public string BoolToString(bool toConvert)
        {
            if (toConvert)
                return "1";
            else
                return "0";
        }
        public bool StringToBool(string toConvert)
        {
            if (toConvert == "1")
                return true;
            else
                return false;
        }
        private void OnEnable()
        {
            startManager = GetComponent<SceneStartManager>();
            startManager.EventLoggedIn += LaunchLoadDataOnLogIn;
            startManager.EventTaskUpdate += LaunchUpdateTaskStatuses;
            startManager.EventSaveMaxLevel += LaunchSaveMaxLevel;
            startManager.EventPOUpdate += LaunchPOUpdate;
            startManager.EventPOUBuy += LaunchPOBuy;
        }
        private void OnDisable()
        {
            startManager.EventLoggedIn -= LaunchLoadDataOnLogIn;
            startManager.EventTaskUpdate -= LaunchUpdateTaskStatuses;
            startManager.EventSaveMaxLevel -= LaunchSaveMaxLevel;
            startManager.EventPOUpdate -= LaunchPOUpdate;
            startManager.EventPOUBuy += LaunchPOBuy;
        }

        private void LaunchLoadDataOnLogIn(string usernameToPass)
        {
            string[] dataToPass = new string[2];
            dataToPass[0] = usernameToPass;
            dataToPass[1] = loadDataOnLogInURL;
            DCMLoadOnLogin LoadDataOnLogIn = new DCMLoadOnLogin(this, startManager, dataToPass);
        }
        private void LaunchUpdateTaskStatuses(string dummy)
        {
            DCMUpdateTaskStatuses UpdateTaskStatuses = new DCMUpdateTaskStatuses(this, startManager, updateTaskStatusesURL);
        }
        private void LaunchSaveMaxLevel(string maxLevelToSet)
        {
            string[] dataToPass = new string[2];
            dataToPass[0] = maxLevelToSet;
            dataToPass[1] = saveMaxLevelURL;
            DCMSaveMaxLevel SaveMaxLevel = new DCMSaveMaxLevel(this, dataToPass);
        }
        private void LaunchPOUpdate(string dummy)
        {
            string[] dataToPass = new string[2];
            dataToPass[0] = updatePOURL;
            dataToPass[1] = buyPOURL;
            DCMPOBuyUpdate POUpdate = new DCMPOBuyUpdate(this, startManager, dataToPass);
            POUpdate.StartPOUpdate();
        }
        private void LaunchPOBuy(string dummy)
        {
            string[] dataToPass = new string[2];
            dataToPass[0] = updatePOURL;
            dataToPass[1] = buyPOURL;
            DCMPOBuyUpdate POUpdate = new DCMPOBuyUpdate(this, startManager, dataToPass);
            POUpdate.StartPOBuy();
        }
    }
}