using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace U1
{
    public class DatabaseConnectionManager : MonoBehaviour
    {
        [SerializeField] string loadDataOnLogInURL;
        private SceneStartManager sceneStartManager;

        private void OnEnable()
        {
            sceneStartManager = GetComponent<SceneStartManager>();
            sceneStartManager.EventLoggedIn += LoadDataOnLogIn;
        }
        private void OnDisable()
        {
            sceneStartManager.EventLoggedIn -= LoadDataOnLogIn;
        }

        private void LoadDataOnLogIn(string usernameToPass)
        {
            StartCoroutine(LoadDataLogIn(usernameToPass));
        }
        IEnumerator LoadDataLogIn(string usernameToPass)
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
                    Debug.Log("Data recieved: " + webRequest.downloadHandler.text);
                    string[] recievedArray = webRequest.downloadHandler.text.Split('|');
                    Debug.Log("PlayerID = " + recievedArray[0] + " PlayerMaxLevel = " + recievedArray[1]);
                }
            }
        }
    }
}