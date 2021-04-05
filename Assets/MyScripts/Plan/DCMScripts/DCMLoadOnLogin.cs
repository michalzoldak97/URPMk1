using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace U1
{
    public class DCMLoadOnLogin  
    {
        private DatabaseConnectionManager connectionManager;
        private SceneStartManager startManager;
        public DCMLoadOnLogin(DatabaseConnectionManager connectionManager, SceneStartManager startManager, string[] dataToPass)
        {
            this.connectionManager = connectionManager;
            this.startManager = startManager;
            connectionManager.StartCoroutine(LoadDataOnLogIn(dataToPass[0], dataToPass[1]));
        }
        private IEnumerator LoadDataOnLogIn(string usernameToPass, string loadDataOnLogInURL)
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("username", usernameToPass);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(loadDataOnLogInURL, wFrom))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                    Debug.Log(": Error: " + webRequest.error);
                else if (webRequest.downloadHandler.text == "0")
                    Debug.Log("Sth went wrong with php: " + webRequest.error);
                else
                    UpdateDataOnLogIn(webRequest.downloadHandler.text);
            }
        }
        private void UpdateDataOnLogIn(string webData)
        {
            string[] firstDataSplit = webData.Split('^');

            string[] playerIDAndLevel = firstDataSplit[0].Split('/');
            try
            {
                int playerId = Int32.Parse(playerIDAndLevel[0]);
                int playerMaxLevel = Int32.Parse(playerIDAndLevel[1]);
                int playerCoins = Int32.Parse(playerIDAndLevel[2]);
                int playerExperience = Int32.Parse(playerIDAndLevel[3]);
                int playerMaxSlots = Int32.Parse(playerIDAndLevel[4]);
                connectionManager.SetPlayerID(playerId);
                startManager.SetMaxAllowLevel(playerMaxLevel);
                startManager.SetPlayerCoins(playerCoins);
                startManager.SetPlayerExperience(playerExperience);
                startManager.SetPlayerMaxSlots(playerMaxSlots);
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
                try
                {
                    int indexOne = Int32.Parse(oneTaskInfo[0]);
                    int indexTwo = Int32.Parse(oneTaskInfo[1]);
                    bool valueToSet = (oneTaskInfo[2] == "1");
                    startManager.SetTaskStatuses(indexOne, indexTwo, valueToSet);
                }
                catch
                {
                    Debug.Log("Failed to parse to int");
                }
            }
            try
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
                            if (objIndex == j)
                            {
                                myPlaceableObjects[j].numOfOwnedObjects = Int32.Parse(singleObjectInfo[1]);
                                myPlaceableObjects[j].maxNumOfOwnedObjects = Int32.Parse(singleObjectInfo[2]);
                                myPlaceableObjects[j].numOfObjOnStack = Int32.Parse(singleObjectInfo[3]);
                                myPlaceableObjects[j].isAvailable = connectionManager.StringToBool(singleObjectInfo[4]);
                                myPlaceableObjects[j].isAddedToStack = connectionManager.StringToBool(singleObjectInfo[5]);
                                break;
                            }
                        }
                    }
                    startManager.SetPlaceableObjects(myPlaceableObjects);
                }
                else
                    Debug.Log("no objects data");
            }
            catch
            {
                Debug.Log("Placeable process messed up");
            }
        }
    }
}
