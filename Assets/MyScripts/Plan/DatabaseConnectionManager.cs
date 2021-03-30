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
        [SerializeField] string saveMavLevelURL;
        [SerializeField] string updatePOURL;
        [SerializeField] string buyPOURL;
        private int myPlayerID;
        private SceneStartManager startManager;

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
            StartCoroutine(LoadDataOnLogIn(usernameToPass));
        }
        IEnumerator LoadDataOnLogIn(string usernameToPass)
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("username", usernameToPass);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(loadDataOnLogInURL, wFrom))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(": Error: " + webRequest.error);
                }
                else if(webRequest.downloadHandler.text == "0")
                {
                    Debug.Log("Sth went wrong with php: " + webRequest.error);
                }
                else
                {
                    //Debug.Log("Data recieved: " + webRequest.downloadHandler.text);
                    UpdateDataOnLogIn(webRequest.downloadHandler.text);
                }
            }
        }
        private void UpdateDataOnLogIn(string webData)
        {
            //Debug.Log(webData);
            string[] firstDataSplit = webData.Split('^');

            string[] playerIDAndLevel = firstDataSplit[0].Split('/');
            try
            {
                int playerId = Int32.Parse(playerIDAndLevel[0]);
                int playerMaxLevel = Int32.Parse(playerIDAndLevel[1]);
                int playerCoins = Int32.Parse(playerIDAndLevel[2]);
                int playerExperience = Int32.Parse(playerIDAndLevel[3]);
                int playerMaxSlots = Int32.Parse(playerIDAndLevel[4]);
                myPlayerID = playerId;
                startManager.SetMaxAllowLevel(playerMaxLevel);
                startManager.SetPlayerCoins(playerCoins);
                startManager.SetPlayerExperience(playerExperience);
                startManager.SetPlayerMaxSlots(playerMaxSlots);
                //Debug.Log("Id sucessfully converted to: " + playerId);
            }
            catch
            {
                Debug.Log("Id failed to parse");
            }
            string[] allTasksInfo = firstDataSplit[1].Split('|');
            int tasksInfoLength = allTasksInfo.Length;
            for (int i = 0; i < tasksInfoLength; i++)
            {
                string[] oneTaskInfo = allTasksInfo[i].Split('/');
                //Debug.Log("Index I  = " + oneTaskInfo[0] + " index J = " + oneTaskInfo[1] + " value = " + oneTaskInfo[2]);
                try
                {
                    int indexOne = Int32.Parse(oneTaskInfo[0]);
                    int indexTwo = Int32.Parse(oneTaskInfo[1]);
                    bool valueToSet = (oneTaskInfo[2]=="1");
                    startManager.SetTaskStatuses(indexOne, indexTwo, valueToSet);
                }
                catch
                {
                    Debug.Log("Failed to parse to int");
                }               
            }
            try//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                if (firstDataSplit.Length > 2)
                {
                    string[] placeableObjInfo = firstDataSplit[2].Split('|');
                    PlaceableObject[] myPlaceableObjects = startManager.GetPlaceableObjects();
                    int objInfoLength = placeableObjInfo.Length;
                    int placeableObjLength = myPlaceableObjects.Length;
                    for (int i = 0; i < objInfoLength; i++)
                    {
                        string[] singleObjectInfo = placeableObjInfo[i].Split('/');
                        int objIndex = Int32.Parse(singleObjectInfo[0]);
                        for (int j = 0; j < placeableObjLength; j++)
                        {
                            //Debug.Log("Looping po");
                            if (objIndex == j)
                            {
                                //Debug.Log("matchnig data for given index: " + objIndex + " po: " + j);
                                myPlaceableObjects[j].numOfOwnedObjects = Int32.Parse(singleObjectInfo[1]);
                                myPlaceableObjects[j].maxNumOfOwnedObjects = Int32.Parse(singleObjectInfo[2]);
                                myPlaceableObjects[j].numOfObjOnStack = Int32.Parse(singleObjectInfo[3]);
                                myPlaceableObjects[j].isAvailable = StringToBool(singleObjectInfo[4]);
                                myPlaceableObjects[j].isAddedToStack = StringToBool(singleObjectInfo[5]);
                                break;
                            }
                        }
                    }
                    startManager.SetPlaceableObjects(myPlaceableObjects);
                }
                else
                {
                    Debug.Log("no objects data");
                }
            }
            catch
            {
                Debug.Log("Placeable process messed up");
            }///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        private void LaunchUpdateTaskStatuses(string dummy)
        {
            StartCoroutine(UpdateTaskStatuses());
        }
        IEnumerator UpdateTaskStatuses()
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("player_id", myPlayerID.ToString());
            wFrom.AddField("task_status", CreateNewTaskStatuses());
            using (UnityWebRequest webRequest = UnityWebRequest.Post(updateTaskStatusesURL, wFrom))
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
                    Debug.Log("Sucessfull Task Update  ");
                }
                else
                {
                    Debug.Log("Error:  " + webRequest.downloadHandler.text);
                }
            }
        }
        private string CreateNewTaskStatuses()
        {
            string taskInfoToPass = "";
            int firstDimmLength = startManager.GetTaskStatuses().GetLength(0);
            int secondDimmLength = startManager.GetTaskStatuses().GetLength(1);
            for (int i = 0; i < firstDimmLength; i++)
            {
                for (int j = 0; j < secondDimmLength; j++)
                {
                    taskInfoToPass += (i.ToString() + '/' + j.ToString() + '/' + BoolToString(startManager.GetTaskStatuses()[i, j]));
                    if (j == secondDimmLength - 1 && i == firstDimmLength - 1)
                    {
                        
                    }
                    else
                    {
                        taskInfoToPass += '|';
                    }
                }
            }
            return taskInfoToPass;
        }
        private string BoolToString(bool toConvert)
        {
            if (toConvert)
                return "1";
            else
                return "0";
        }
        private bool StringToBool(string toConvert)
        {
            if (toConvert == "1")
                return true;
            else
                return false;
        }

        private void LaunchSaveMaxLevel(string maxLevelToSet)
        {
            StartCoroutine(SaveMaxLevel(maxLevelToSet));
        }
        IEnumerator SaveMaxLevel(string maxLevelToSet)
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("player_id", myPlayerID.ToString());
            wFrom.AddField("player_max_level", maxLevelToSet);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(saveMavLevelURL, wFrom))
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