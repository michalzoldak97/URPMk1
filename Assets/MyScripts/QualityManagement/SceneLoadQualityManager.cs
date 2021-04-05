using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class SceneLoadQualityManager : MonoBehaviour
    {
        [SerializeField] private int numOfFramesMenu;
        [SerializeField] private int numOfFramesGame;
        private SceneIndex choosenGameIndex = SceneIndex.GAME_BEST;
        public bool isVSync { get; set; }
        private SceneStartManager startManager;
        private void OnEnable()
        {
            startManager = GetComponent<SceneStartManager>();
            startManager.EventStartPlan += SetPlanQuality;
            startManager.EventStartGame += SetGameQuality;
        }
        private void OnDisable()
        {
            startManager.EventStartPlan -= SetPlanQuality;
            startManager.EventStartGame -= SetGameQuality;
        }
        private void SetPlanQuality()
        {
            if(!isVSync)
            {
                Application.targetFrameRate = numOfFramesMenu;
            }
        }
        private void SetGameQuality()
        {
            if (!isVSync)
            {
                Application.targetFrameRate = numOfFramesGame;
            }
        }
        public void LoadGameScene()
        {
            startManager.ChangeScene(choosenGameIndex);
        }
    }
}