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
        private SceneStartManager sceneManager;

        private void Start()
        {
            sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            LogInAttempt();
            SetupLevelsSelection();
        }
        public void LoadLevelSelection()
        {
            canvasLevel.SetActive(true);
        }
        public void LogInAttempt()
        {
            if(sceneManager.isLoggedIn)
            {
                panelLogIn.SetActive(false);
                panelSignUp.SetActive(false);
                panelMenu.SetActive(true);
            }
            else
            {
                panelLogIn.SetActive(true);
                panelMenu.SetActive(false);
                panelSignUp.SetActive(false);
                Debug.Log("Logging in actions");
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
            sceneManager.SetCurrentLevel(index);
            sceneManager.ChangeScene(1);
        }
        void SetupLevelsSelection()
        {
            for (int i = 0; i < sceneManager.maxAllowLevel; i++)
            {
                int x = i+1;
                GameObject objUI = Instantiate(levelButton, levelButtonsTransform);
                objUI.GetComponent<Button>().onClick.AddListener(delegate { LoadNextLevel(x); });
                objUI.GetComponentInChildren<TMP_Text>().text = "Level " + x.ToString();
                Debug.Log("level set = " + x);
            }
        }
    }
}
