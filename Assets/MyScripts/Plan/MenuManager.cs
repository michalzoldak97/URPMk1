using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelButton;
        [SerializeField] private Transform levelButtonsTransform;
        [SerializeField] private GameObject canvasLevel, panelMenu, panelLogIn, panelSignUp;
        private SceneStartManager startManager;

        public SceneStartManager GetSceneManager()
        {
            return startManager;
        }
        private void Start()
        {
            startManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            LogInAttempt();
        }
        public void LoadLevelSelection()
        {
            canvasLevel.SetActive(true);
        }
        public void LogInAttempt()
        {
            if(startManager.isLoggedIn)
            {
                panelLogIn.SetActive(false);
                panelSignUp.SetActive(false);
                panelMenu.SetActive(true);
                SetupLevelsSelection("");
            }
            else
            {
                panelLogIn.SetActive(true);
                panelMenu.SetActive(false);
                panelSignUp.SetActive(false);
            }
        }
        public void ActivateSignUpPanel()
        {
            panelSignUp.SetActive(true);
            panelLogIn.SetActive(false);
            panelMenu.SetActive(false);
        }
        public void DeactivateSignUpPanel()
        {
            panelSignUp.SetActive(false);
            panelLogIn.SetActive(true);
            panelMenu.SetActive(false);
        }
        void LoadNextLevel(int index)
        {
            startManager.SetCurrentLevel(index);
            startManager.ChangeScene(SceneIndex.TASK);
        }
        void SetupLevelsSelection(string dummy)
        {
            for (int i = 0; i < startManager.maxAllowLevel; i++)
            {
                int x = i+1;
                GameObject objUI = Instantiate(levelButton, levelButtonsTransform);
                objUI.GetComponent<Button>().onClick.AddListener(delegate { LoadNextLevel(x); });
                objUI.GetComponentInChildren<TMP_Text>().text = "Level " + x.ToString();
            }
        }
    }
}
