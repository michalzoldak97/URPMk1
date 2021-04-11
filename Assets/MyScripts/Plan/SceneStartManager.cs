using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace U1
{
    public class SceneStartManager : MonoBehaviour
    {
        [SerializeField] private PlaceableObject[] placeableObjects;
        public PlaceableObject[] GetPlaceableObjects()
        {
            return placeableObjects;
        }
        public void SetPlaceableObjects(PlaceableObject[] passedObjs)
        {
            placeableObjects = passedObjs;
        }
        [SerializeField] private List<int> gameScenesIndex = new List<int>();
        [SerializeField] private GameObject sceneLoadScreen;
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
        public int playerMaxSlots { get; private set; }
        public void SetPlayerMaxSlots(int toSet)
        {
            if (toSet >= 3)
                playerMaxSlots = toSet;
        }
        public int maxLevel { get; private set; }
        public int maxAllowLevel { get; private set; }
        public void SetMaxAllowLevel(int toSet)
        {
            if (maxAllowLevel < maxLevel)
                maxAllowLevel = toSet;
        }
        public int currLevel { get; private set; }
        public void SetCurrentLevel(int toSet)
        {
            if (toSet <= maxAllowLevel) 
                currLevel = toSet;
        }
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
        public delegate void DatabaseEventHandler(string username);
        public event DatabaseEventHandler EventLoggedIn;
        public event DatabaseEventHandler EventTaskUpdate;
        public event DatabaseEventHandler EventSaveMaxLevel;
        public event DatabaseEventHandler EventPOUpdate;
        public event DatabaseEventHandler EventPOUBuy;
        public event DatabaseEventHandler EventPOUTransactionResult;

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
        public void CallEventPOUpdate(string dummy)
        {
            if (EventPOUpdate != null)
            {
                EventPOUpdate(dummy);
            }
        }
        public void CallEventPOUBuy(string dummy)
        {
            if (EventPOUBuy != null)
            {
                EventPOUBuy(dummy);
            }
        }
        public void CallEventPOUTransactionResult(string dummy)
        {
            if (EventPOUTransactionResult != null)
            {
                EventPOUTransactionResult(dummy);
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
        public void ChangeScene(SceneIndex index)
        {
            OnEnd();
            if(!gameScenesIndex.Contains((int)index))
                SceneManager.LoadScene((int)index);
            else
            {
                LoadGameScene((int)index);
            }
        }
        private AsyncOperation loadingOperation;
        private void LoadGameScene(int index)
        {
            sceneLoadScreen.SetActive(true);
            Time.timeScale = 0;
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            loadingOperation = SceneManager.LoadSceneAsync(index);
            StartCoroutine(GetGameSceneLoadProgress());
        }
        IEnumerator GetGameSceneLoadProgress()
        {
            while(!loadingOperation.isDone)
            {
                yield return null;
            }
            Time.timeScale = 1;
            sceneLoadScreen.SetActive(false);
            Debug.Log("Finished loading");
        }
        void OnStart(Scene dummy, LoadSceneMode dummyMode)
        {
            SSMStartEndEvent OnStart = new SSMStartEndEvent(this, SceneManager.GetActiveScene().buildIndex);
            OnStart.OnStart();
        }
        void OnEnd()
        {
            SSMStartEndEvent OnEnd = new SSMStartEndEvent(this, SceneManager.GetActiveScene().buildIndex);
            OnEnd.OnEnd();
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
