using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] GameObject levelButton;
        [SerializeField] Transform levelButtonsTransform;
        [SerializeField] GameObject canvasLevel;
        private SceneStartManager sceneManager;

        private void Start()
        {
            sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            SetupUI();
        }
        public void LoadLevelSelection()
        {
            canvasLevel.SetActive(true);
        }
        void LoadNextLevel(int index)
        {
            sceneManager.SetCurrentLevel(index);
            sceneManager.ChangeScene(1);
        }
        void SetupUI()
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
