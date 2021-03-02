using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace WEB1
{
    public class TestWeb1 : MonoBehaviour
    {
        [SerializeField] private string webAddress;
        public void LaunchWebRequest()
        {
            StartCoroutine(GetRequest(webAddress));
        }
        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                byte[] dataRecieved = webRequest.downloadHandler.data;

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    string[] recieved = webRequest.downloadHandler.text.Split('/');
                    for (int i = 0; i < recieved.Length; i++)
                    {
                        Debug.Log(recieved[i]);
                    }
                }
            }
        }
    }
}