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
        public int playerCoins { get; private set; }
        public void SetPlayerCoins(int toSet)
        {
            if (toSet >= 0)
                playerCoins = toSet;
        }
        public int playerExperience { get; private set; }
        public void SetPlayerExperience(int toSet)
        {
            if (toSet >= 0)
                playerExperience = toSet;
        }
        public int maxLevel { get; private set; }
        public int maxAllowLevel { get; private set; }
        public int currLevel { get; private set; }
        public int signUpAttempts { get; set; }
        public int logInAttempts { get; set; }
        public bool isLoggedIn { get; set; }

        private bool[,] taskStatuses = new bool[5,5];
        public bool[,] GetTaskStatuses()
        {
            return taskStatuses;
        }
        public void SetTaskStatuses(int indexA, int indexB, bool toSet)
        {
            taskStatuses[indexA, indexB] = toSet;
        }
        public enum SceneType
        {
            menu, task, shop, plan, game
        }
        public delegate void DatabaseEventHandler(string username);
        public event DatabaseEventHandler EventLoggedIn;
        public event DatabaseEventHandler EventTaskUpdate;
        public event DatabaseEventHandler EventSaveMaxLevel;

        public delegate void SceneEventHandler();
        public event SceneEventHandler EventStartPlan;
        public event SceneEventHandler EventEndPlan;
        public event SceneEventHandler EventStartGame;
        public event SceneEventHandler EventEndGameScene;
        public event SceneEventHandler EventQuitGame;

        public void CallEventLoggedIn(string usernameToPass)
        {
            if (EventLoggedIn != null)
            {
                EventLoggedIn(usernameToPass);
            }
        }
        public void CallEventTaskUpdate(string dummy)
        {
            if (EventTaskUpdate != null)
            {
                EventTaskUpdate(dummy);
            }
        }
        public void CallEventSaveMaxLevel(string dummy)
        {
            if (EventSaveMaxLevel != null)
            {
                EventSaveMaxLevel(dummy);
            }
        }
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
        public void CallEventEndGameScene()
        {
            if (EventEndGameScene != null)
            {
                EventEndGameScene();
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
            maxAllowLevel = 1;
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
                        CallEventEndGameScene();
                        break;
                    }
            }
        }
        public void SetCurrentLevel(int toSet)
        {
            if (toSet <= maxAllowLevel) // changed from maxLevel
                currLevel = toSet;
            else
                Debug.LogError("Curr level improper value");
        }
        public void IncreaseAllowedLevel()
        {
            if(maxAllowLevel < maxLevel)
                maxAllowLevel++;
        }
        public void SetMaxAllowLevel(int toSet)
        {
            if (maxAllowLevel < maxLevel)
                maxAllowLevel = toSet;
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
