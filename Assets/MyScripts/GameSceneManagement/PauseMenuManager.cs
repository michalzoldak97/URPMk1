using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace U1
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject menuCanvas, panelConfirm;
        private SceneIndex indexToGo;
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
            indexToGo = SceneIndex.MENU;
        }
        public void AttemptGiveUp()
        {
            panelConfirm.SetActive(true);
            indexToGo = SceneIndex.PLAN;
        }
        public void ResignQuiting()
        {
            panelConfirm.SetActive(false);
        }
        public void QuitGameScene()
        {
            startManager.ChangeScene(indexToGo);
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