using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private Transform PObuttonsTransform;
        [SerializeField] private GameObject infoPanel, poShopPanel;
        private Camera myMainCamera;
        private SceneStartManager sceneStartManager;

        private void Start()
        {
            myMainCamera = Camera.main;
            sceneStartManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            SetUpPOButtons();
            infoPanel.SetActive(false);
        }
        private void SetUpPOButtons()
        {
            PlaceableObject[] placeableObjects = sceneStartManager.GetPlaceableObjects();
            int POlength = placeableObjects.Length;
            for (int i = 0; i < POlength; i++)
            {
                GameObject POPanel = Instantiate(poShopPanel, PObuttonsTransform);
                POPanel.GetComponent<POShopButton>().SetUpPOButton(i, placeableObjects[i], this);
            }
        }
        public void OnInfoButtonEnter(int POindex, Transform infoButtonTransform)
        {
            infoPanel.SetActive(true);
            int x, y;
            if(Input.mousePosition.x <= Screen.width/2)
            {
                Debug.Log(Input.mousePosition.x + " is less than: " + Screen.width / 2);
                Debug.Log("Params: " + Screen.width + Screen.height);
                x = (int)(infoButtonTransform.position.x + (Screen.width / 10)*1.1f);
            }
            else
            {
                Debug.Log(Input.mousePosition.x + " is greater than: " + Screen.width / 2);
                Debug.Log("Params: " + Screen.width + Screen.height);
                x = (int)infoButtonTransform.position.x - Screen.height / 10;
            }
            if(Input.mousePosition.y >= Screen.height / 2)
            {
                y = (int)infoButtonTransform.position.y - Screen.height / 3;
            }
            else
            {
                y = (int)infoButtonTransform.position.y + Screen.height / 3;
            }
            Vector3 infoPosition = infoPanel.transform.position;
            infoPosition.x = x; infoPosition.y = y;
            infoPanel.transform.position = infoPosition;
            infoPanel.GetComponent<ShopInfoPanel>().SetInfoPanel(sceneStartManager.GetPlaceableObjects()[POindex].objImage, sceneStartManager.GetPlaceableObjects()[POindex].objectInfo);
        }
        public void OnInfoButtonExit(int POindex)
        {
            infoPanel.SetActive(false);
        }
    }
}
