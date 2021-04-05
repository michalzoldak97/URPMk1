using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
namespace U1
{
    public class DatabaseConnectionManager : MonoBehaviour
    {
        [SerializeField] string loadDataOnLogInURL;
        [SerializeField] string updateTaskStatusesURL;
        [SerializeField] string saveMaxLevelURL;
        [SerializeField] string updatePOURL;
        [SerializeField] string buyPOURL;
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
            DCMLoadOnLogin LoadDataOnLogIn = new DCMLoadOnLogin(this, this, startManager, dataToPass);
        }
        private void LaunchUpdateTaskStatuses(string dummy)
        {
            DCMUpdateTaskStatuses UpdateTaskStatuses = new DCMUpdateTaskStatuses(this, this, startManager, updateTaskStatusesURL);
        }
        private void LaunchSaveMaxLevel(string maxLevelToSet)
        {
            string[] dataToPass = new string[2];
            dataToPass[0] = maxLevelToSet;
            dataToPass[1] = saveMaxLevelURL;
            DCMSaveMaxLevel SaveMaxLevel = new DCMSaveMaxLevel(this, this, dataToPass);
            //StartCoroutine(SaveMaxLevel(maxLevelToSet));
        }
        IEnumerator SaveMaxLevel(string maxLevelToSet)
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("player_id", myPlayerID.ToString());
            wFrom.AddField("player_max_level", maxLevelToSet);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(saveMaxLevelURL, wFrom))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                }
                else if (webRequest.downloadHandler.text == "0")
                {
                    Debug.Log("Sth went wrong with php: " + webRequest.error);
                }
                else if (webRequest.downloadHandler.text == "1")
                {
                    Debug.Log("Sucessfull Level Saved");
                }
                else
                {
                    Debug.Log("Error:  " + webRequest.downloadHandler.text);
                }
            }
        }
        private void LaunchPOUpdate(string dummy)
        {
            StartCoroutine(POUpdate());
        }
        IEnumerator POUpdate()
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("player_id", myPlayerID.ToString());
            wFrom.AddField("player_placeable_object", CreatePlaceableObjects());
            using (UnityWebRequest webRequest = UnityWebRequest.Post(updatePOURL, wFrom))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                }
                else if (webRequest.downloadHandler.text == "0")
                {
                    Debug.Log("Sth went wrong with php: " + webRequest.error);
                }
                else if (webRequest.downloadHandler.text == "1")
                {
                    Debug.Log("Sucessfull PO Update  ");
                }
                else
                {
                    Debug.Log("Error:  " + webRequest.downloadHandler.text);
                }
            }
        }
        private string CreatePlaceableObjects()
        {
            string placeableObjInfoToPass = "";
            PlaceableObject[] myPlaceableObjects = startManager.GetPlaceableObjects();
            int placeableObjectsLength = myPlaceableObjects.Length;
            for (int i = 0; i < placeableObjectsLength; i++)
            {
                placeableObjInfoToPass += (i.ToString() + '/' + myPlaceableObjects[i].numOfOwnedObjects.ToString() + '/' + myPlaceableObjects[i].maxNumOfOwnedObjects.ToString()
                    + '/' + ReturnObjOnStack(myPlaceableObjects[i].numOfObjOnStack) + '/' +    BoolToString(myPlaceableObjects[i].isAvailable) + '/' + BoolToString(myPlaceableObjects[i].isAddedToStack));
                if(i < placeableObjectsLength - 1)
                {
                    placeableObjInfoToPass += '|';
                }
            }
            return placeableObjInfoToPass;
        }
        private void LaunchPOBuy(string dummy)
        {
            StartCoroutine(POBuy());
        }
        IEnumerator POBuy()
        {
            WWWForm wFrom = new WWWForm();
            string dataToSend = startManager.playerCoins.ToString() + '^' + CreatePlaceableObjects();
            Debug.Log(dataToSend);
            wFrom.AddField("player_id", myPlayerID.ToString());
            wFrom.AddField("data_to_send", dataToSend);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(buyPOURL, wFrom))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                }
                else if (webRequest.downloadHandler.text == "0")
                {
                    Debug.Log("Sth went wrong with php: " + webRequest.error);
                }
                else if (webRequest.downloadHandler.text == "1")
                {
                    Debug.Log("PO bought");
                    startManager.CallEventPOUTransactionResult("1");
                }
                else
                {
                    Debug.Log("Error:  " + webRequest.downloadHandler.text);
                }
            }
        }
        private string ReturnObjOnStack(int objOnStack)
        {
            if (objOnStack > 1)
                return "1";
            else
                return objOnStack.ToString();
        }
    }
}