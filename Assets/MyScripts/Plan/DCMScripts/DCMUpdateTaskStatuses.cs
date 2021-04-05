using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace U1
{

    public class DCMUpdateTaskStatuses : MonoBehaviour
    {
        private DatabaseConnectionManager connectionManager;
        private SceneStartManager startManager;
        public DCMUpdateTaskStatuses(MonoBehaviour mono, DatabaseConnectionManager connectionManager, SceneStartManager startManager, string updateTaskStatusesURL)
        {
            this.connectionManager = connectionManager;
            this.startManager = startManager;
            mono.StartCoroutine(UpdateTaskStatuses(updateTaskStatusesURL));
        }
        private IEnumerator UpdateTaskStatuses(string updateTaskStatusesURL)
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("player_id", connectionManager.GetPlayerID().ToString());
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
                    taskInfoToPass += (i.ToString() + '/' + j.ToString() + '/' + connectionManager.BoolToString(startManager.GetTaskStatuses()[i, j]));
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
    }
}
