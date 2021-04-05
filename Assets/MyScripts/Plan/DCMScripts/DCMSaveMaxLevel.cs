using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace U1
{
    public class DCMSaveMaxLevel : MonoBehaviour
    {
        private DatabaseConnectionManager connectionManager;
        public DCMSaveMaxLevel(MonoBehaviour mono, DatabaseConnectionManager connectionManager, string[] dataToPass) 
        {
            this.connectionManager = connectionManager;
            mono.StartCoroutine(SaveMaxLevel(dataToPass[0], dataToPass[1]));
        }
        private IEnumerator SaveMaxLevel(string maxLevelToSet, string saveMaxLevelURL)
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("player_id", connectionManager.GetPlayerID().ToString());
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
    }
}
