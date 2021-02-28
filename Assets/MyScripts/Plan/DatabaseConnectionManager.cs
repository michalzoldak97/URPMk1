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
        private int myPlayerID;
        private SceneStartManager sceneStartManager;

        private void OnEnable()
        {
            sceneStartManager = GetComponent<SceneStartManager>();
            sceneStartManager.EventLoggedIn += LaunchLoadDataOnLogIn;
            sceneStartManager.EventTaskUpdate += LaunchUpdateTaskStatuses;
        }
        private void OnDisable()
        {
            sceneStartManager.EventLoggedIn -= LaunchLoadDataOnLogIn;
            sceneStartManager.EventTaskUpdate -= LaunchUpdateTaskStatuses;
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
            string[] firstDataSplit = webData.Split('^');

            string[] playerIDAndLevel = firstDataSplit[0].Split('/');
            try
            {
                int playerId = Int32.Parse(playerIDAndLevel[0]);
                int playerMaxLevel = Int32.Parse(playerIDAndLevel[1]);
                myPlayerID = playerId;
                sceneStartManager.SetMaxAllowLevel(playerMaxLevel);
                Debug.Log("Id sucessfully converted to: " + playerId);
            }
            catch
            {
                Debug.Log("Id failed to parse");
            }
            string[] allTasksInfo = firstDataSplit[1].Split('|');
            for (int i = 0; i < allTasksInfo.Length; i++)
            {
                string[] oneTaskInfo = allTasksInfo[i].Split('/');
                Debug.Log("Index I  = " + oneTaskInfo[0] + " index J = " + oneTaskInfo[1] + " value = " + oneTaskInfo[2]);
                try
                {
                    int indexOne = Int32.Parse(oneTaskInfo[0]);
                    int indexTwo = Int32.Parse(oneTaskInfo[1]);
                    bool valueToSet = (oneTaskInfo[2]=="1");
                    sceneStartManager.SetTaskStatusesOnLog(indexOne, indexTwo, valueToSet);
                }
                catch
                {
                    Debug.Log("Failed to parse to int");
                }
            }
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
            int firstDimmLength = sceneStartManager.GetTaskStatuses().GetLength(0);
            int secondDimmLength = sceneStartManager.GetTaskStatuses().GetLength(1);
            for (int i = 0; i < firstDimmLength; i++)
            {
                for (int j = 0; j < secondDimmLength; j++)
                {
                    taskInfoToPass += (i.ToString() + '/' + j.ToString() + '/' + BoolToString(sceneStartManager.GetTaskStatuses()[i, j]));
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
    }
}