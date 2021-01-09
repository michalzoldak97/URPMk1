using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace U1
{
    [System.Serializable]
    public class PlaceableObject
    {
        public GameObject objToSpawn;
        public GameObject objButon;
        public GameObject mapAlias;
        public int objNum;
        public Vector3[] worldPositions;
    }
    public class SceneStartManager : MonoBehaviour
    {
        [SerializeField] PlaceableObject[] placeableObjects;
        [SerializeField] List<int> planScenesIndex = new List<int>();
        [SerializeField] List<int> gameScenesIndex = new List<int>();
        public int maxLevel { get; private set; }
        public int currLevel { get; private set; }
        public enum SceneType
        {
            menu, task, shop, plan, game
        }

        public delegate void SceneEventHandler();
        public event SceneEventHandler EventStartPlan;
        public event SceneEventHandler EventEndPlan;
        public event SceneEventHandler EventStartGame;
        public event SceneEventHandler EventEndGame;
        public event SceneEventHandler EventQuitGame;

        public void CallEventStartPlan()
        {
            if(EventStartPlan != null)
            {
                EventStartPlan();
            }
        }
        public void CallEventEndPlan()
        {
            if (EventEndPlan != null)
            {
                EventEndPlan();
            }
        }
        public void CallEventStartGame()
        {
            if (EventStartGame != null)
            {
                EventStartGame();
            }
        }
        public void CallEventEndGame()
        {
            if (EventEndGame != null)
            {
                EventEndGame();
            }
        }
        public void CallEventQuitGame()
        {
            if (EventQuitGame != null)
            {
                EventQuitGame();
            }
        }
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            //test 
            maxLevel = 5;
        }
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnStart;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnStart;
        }
        public void ChangeScene(int index)
        {
            OnEnd();
            SceneManager.LoadScene(index);
        }
        void OnStart(Scene dummy, LoadSceneMode dummyMode)
        {
            if(planScenesIndex.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                OnSceneLoad(SceneType.plan);
            }
            else if(gameScenesIndex.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                OnSceneLoad(SceneType.game);
            }
        }
        void OnEnd()
        {
            if (planScenesIndex.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                OnSceneUnload(SceneType.plan);
            }
            else if (gameScenesIndex.Contains(SceneManager.GetActiveScene().buildIndex))
            {
                OnSceneUnload(SceneType.game);
            }
        }
        void OnSceneLoad(SceneType sType)
        {
            switch (sType)
            {
                case SceneType.plan:
                    {
                        CallEventStartPlan();
                        break;
                    }
                case SceneType.game:
                    {
                        CallEventStartGame();
                        break;
                    }
            }
        }
        void OnSceneUnload(SceneType sType)
        {
            switch (sType)
            {
                case SceneType.plan:
                    {
                        CallEventEndPlan();
                        break;
                    }
                case SceneType.game:
                    {
                        CallEventEndGame();
                        break;
                    }
            }
        }
        public void SetCurrentLevel(int toSet)
        {
            if (toSet <= maxLevel)
                currLevel = toSet;
            else
                Debug.LogError("Curr level improper value");
        }
        public PlaceableObject[] GetPlaceableObjects()
        {
            return placeableObjects;
        }
        public void SetPlaceablePositions(PlaceableObject[] passedObjs)
        {
            placeableObjects = passedObjs;
        }
        public void SetObjNum(int index, int num)
        {
            placeableObjects[index].objNum = num;
        }
        public void QuitGame()
        {
            CallEventQuitGame();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
            Debug.Log("Quit");
        }
    }
}
