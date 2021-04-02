using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject menuCanvas, panelConfirm;
        private int sceneToQuit = 0;
        private SceneStartManager startManager;
        private void Awake()
        {
            try
            {
                startManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            }
            catch
            {
                Debug.Log("No startManagerFound");
            }
        }
        public void DisplayMenu()
        {
            menuCanvas.SetActive(true);
            OnOffPauseScene(true);
        }
        public void HideMenu()
        {
            menuCanvas.SetActive(false);
            OnOffPauseScene(false);
        }
        public void AttemptQuit()
        {
            panelConfirm.SetActive(true);
            sceneToQuit = 0;
        }
        public void AttemptGiveUp()
        {
            panelConfirm.SetActive(true);
            sceneToQuit = 3;
        }
        public void ResignQuiting()
        {
            panelConfirm.SetActive(false);
        }
        public void QuitGameScene()
        {
            startManager.ChangeScene(sceneToQuit);
        }
        private void OnOffPauseScene(bool shouldPause)
        {
            if (shouldPause)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }
}