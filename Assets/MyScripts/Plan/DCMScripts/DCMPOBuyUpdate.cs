using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace U1
{
    public class DCMPOBuyUpdate
    {
        private DatabaseConnectionManager connectionManager;
        private SceneStartManager startManager;
        private string updatePOURL, buyPOURL;
        public DCMPOBuyUpdate(DatabaseConnectionManager connectionManager, SceneStartManager startManager, string[] dataToPass)
        {
            this.connectionManager = connectionManager;
            this.startManager = startManager;
            updatePOURL = dataToPass[0];
            buyPOURL = dataToPass[1];
        }
        public void StartPOUpdate()
        {
            connectionManager.StartCoroutine(POUpdate());
        }
        public void StartPOBuy()
        {
            connectionManager.StartCoroutine(POBuy());
        }
        private IEnumerator POUpdate()
        {
            WWWForm wFrom = new WWWForm();
            wFrom.AddField("player_id", connectionManager.GetPlayerID().ToString());
            wFrom.AddField("player_placeable_object", CreatePlaceableObjects());
            using (UnityWebRequest webRequest = UnityWebRequest.Post(updatePOURL, wFrom))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                    Debug.Log(": Error: " + webRequest.error);
                else if (webRequest.downloadHandler.text == "0")
                    Debug.Log("Sth went wrong with php: " + webRequest.error);
                else if (webRequest.downloadHandler.text == "1")
                    Debug.Log("Sucessfull PO Update  ");
                else
                    Debug.Log("Error:  " + webRequest.downloadHandler.text);
            }
        }
        private IEnumerator POBuy()
        {
            WWWForm wFrom = new WWWForm();
            string dataToSend = startManager.playerCoins.ToString() + '^' + CreatePlaceableObjects();
            wFrom.AddField("player_id", connectionManager.GetPlayerID().ToString());
            wFrom.AddField("data_to_send", dataToSend);
            using (UnityWebRequest webRequest = UnityWebRequest.Post(buyPOURL, wFrom))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                    Debug.Log(": Error: " + webRequest.error);
                else if (webRequest.downloadHandler.text == "0")
                    Debug.Log("Sth went wrong with php: " + webRequest.error);
                else if (webRequest.downloadHandler.text == "1")
                {
                    Debug.Log("PO bought");
                    startManager.CallEventPOUTransactionResult("1");
                }
                else
                    Debug.Log("Error:  " + webRequest.downloadHandler.text);
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
                    + '/' + ReturnObjOnStack(myPlaceableObjects[i].numOfObjOnStack) + '/' + connectionManager.BoolToString(myPlaceableObjects[i].isAvailable) + '/' + connectionManager.BoolToString(myPlaceableObjects[i].isAddedToStack));
                if (i < placeableObjectsLength - 1)
                {
                    placeableObjInfoToPass += '|';
                }
            }
            return placeableObjInfoToPass;
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
