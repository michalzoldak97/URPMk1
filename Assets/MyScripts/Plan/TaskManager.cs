using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private TaskArray[] tasks;
        [SerializeField] private Image taskImage;
        [SerializeField] private TMP_Text currLevelText, numOfTasksComleted;
        [SerializeField] private GameObject nextLevelButton, taskDoneButton, goodJobButton, badJobButton;
        [SerializeField] private TMP_InputField inputField;
        private SceneStartManager sceneManager;
        private int currentTask;
        void Start()
        {
            sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            LoadTaskStatuses();
            UpdateText();
            SetTask(0);
        }
        int GetNumCompleted()
        {
            int numOfCompleted = 0;
            for (int i = 0; i < tasks[sceneManager.currLevel-1].GetArrayLength(); i++)
            {
                if (tasks[sceneManager.currLevel-1].GetArrayMember(i).isCompleted)
                    numOfCompleted++;
            }
            return numOfCompleted;
        }
        void UpdateText()
        {
            currLevelText.text = "LEVEL " + sceneManager.currLevel.ToString();
            numOfTasksComleted.text = "Completed " + GetNumCompleted().ToString() + " / " + tasks[sceneManager.currLevel-1].GetArrayLength();
        }
        void SetTask(int index)
        {
            taskImage.sprite = tasks[sceneManager.currLevel - 1].GetArrayMember(index).taskImage;
            if (tasks[sceneManager.currLevel - 1].GetArrayMember(index).isCompleted)
                taskDoneButton.GetComponent<Image>().color = Color.green;
            else
                taskDoneButton.GetComponent<Image>().color = Color.red;
            currentTask = index;
        }
        public void NextTask(int a)
        {
            if(a == 0)
            {
                if (currentTask == 0)
                    SetTask(tasks[sceneManager.currLevel - 1].GetArrayLength() - 1);
                else
                    SetTask(currentTask-1);
            }
            else if(a == 1)
            {
                if (currentTask == tasks[sceneManager.currLevel - 1].GetArrayLength() - 1)
                    SetTask(0);
                else
                    SetTask(currentTask + 1);
            }
        }
        public void ResetInputField()
        {
            inputField.text = "";
        }
        IEnumerator OnOffElement(GameObject element)
        {
            element.SetActive(true);
            yield return new WaitForSeconds(1);
            element.SetActive(false);
        }
        public void ValidateTask() 
        {
            Task activeTask = tasks[sceneManager.currLevel - 1].GetArrayMember(currentTask);
            if (inputField.text == activeTask.taskSolution && !activeTask.isCompleted)
            {
                tasks[sceneManager.currLevel - 1].SetCompletedValue(currentTask, true);
                SaveTaskStatuses();
                taskDoneButton.GetComponent<Image>().color = Color.green;
                if (GetNumCompleted() == tasks[sceneManager.currLevel - 1].GetArrayLength() && sceneManager.currLevel != sceneManager.maxLevel)
                {
                    nextLevelButton.SetActive(true);
                    nextLevelButton.GetComponentInChildren<TMP_Text>().text = "Next Level (" + (sceneManager.currLevel + 1).ToString() + ")";
                    sceneManager.IncreaseAllowedLevel();
                }
                StartCoroutine(OnOffElement(goodJobButton));
                ResetInputField();
                UpdateText();
            }
            else if(inputField.text == activeTask.taskSolution)
            {
                StartCoroutine(OnOffElement(goodJobButton));
                ResetInputField();
            }
            else if(inputField.text != activeTask.taskSolution)
            {
                StartCoroutine(OnOffElement(badJobButton));
                ResetInputField();
            }
        }
        public void LoadLevel(int index)
        {
            sceneManager.ChangeScene(index);
        }
        private void LoadTaskStatuses()
        {
            int tasksLenght = tasks.Length;
            if (tasksLenght == sceneManager.GetTaskStatuses().GetLength(0))
            {
                for (int i = 0; i < tasksLenght; i++)
                {
                    int arrayLenght = tasks[i].GetArrayLength();
                    if (arrayLenght == sceneManager.GetTaskStatuses().GetLength(1))
                    {
                        for (int j = 0; j < arrayLenght; j++)
                        {
                            tasks[i].SetCompletedValue(j, sceneManager.GetTaskStatuses()[i, j]);
                        }
                    }
                    else
                    {
                        Debug.Log("Task statuses array not set porperly");
                    }
                }
            }
            else
            {
                Debug.Log("Task statuses array not set porperly");
            }
        }
        private void SaveTaskStatuses()
        {
            int tasksLenght = tasks.Length;
            for (int i = 0; i < tasksLenght; i++)
            {
                int arrayLenght = tasks[i].GetArrayLength();
                for (int j = 0; j < arrayLenght; j++)
                {
                    sceneManager.SetTaskStatuses(i, j, tasks[i].GetArrayMember(j).isCompleted);
                    //Debug.Log("Array num: " + i + " task num: " + j + " task status: " + tasks[i].GetArrayMember(j).isCompleted);
                }
            }
            sceneManager.CallEventTaskUpdate("");
        }

    }
}
