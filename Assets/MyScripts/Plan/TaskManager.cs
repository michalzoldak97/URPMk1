using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace U1
{
    [System.Serializable]
    public class PlayerRequirement
    {
        public int coinsRequired = 0;
        public int experienceRequired = 0;
    }
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private TaskArray[] tasks;
        [SerializeField] private PlayerRequirement[] playerRequirements;
        [SerializeField] private Image taskImage;
        [SerializeField] private TMP_Text currLevelText, numOfTasksComleted, playerCoins, playerExperience;
        [SerializeField] private GameObject nextLevelButton, taskDoneButton, goodJobButton, badJobButton, requirementInfText;
        [SerializeField] private TMP_InputField inputField;
        private SceneStartManager startManager;
        private int currentTask;
        void Start()
        {
            startManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            LoadTaskStatuses();
            UpdateText();
            SetTask(0);
            ValidateIfAllowNextLevelOnStart();
        }
        int GetNumCompleted()
        {
            int numOfCompleted = 0;
            for (int i = 0; i < tasks[startManager.currLevel-1].GetArrayLength(); i++)
            {
                if (tasks[startManager.currLevel-1].GetArrayMember(i).isCompleted)
                    numOfCompleted++;
            }
            return numOfCompleted;
        }
        void UpdateText()
        {
            currLevelText.text = "LEVEL " + startManager.currLevel.ToString();
            numOfTasksComleted.text = "Completed " + GetNumCompleted().ToString() + " / " + tasks[startManager.currLevel-1].GetArrayLength();
            playerCoins.text = "COINS  " + startManager.playerCoins.ToString();
            playerExperience.text = "EXP  " + startManager.playerExperience.ToString();
        }
        void SetTask(int index)
        {
            taskImage.sprite = tasks[startManager.currLevel - 1].GetArrayMember(index).taskImage;
            if (tasks[startManager.currLevel - 1].GetArrayMember(index).isCompleted)
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
                    SetTask(tasks[startManager.currLevel - 1].GetArrayLength() - 1);
                else
                    SetTask(currentTask-1);
            }
            else if(a == 1)
            {
                if (currentTask == tasks[startManager.currLevel - 1].GetArrayLength() - 1)
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
            Task activeTask = tasks[startManager.currLevel - 1].GetArrayMember(currentTask);
            if (inputField.text == activeTask.taskSolution && !activeTask.isCompleted)
            {
                tasks[startManager.currLevel - 1].SetCompletedValue(currentTask, true);
                SaveTaskStatuses();
                taskDoneButton.GetComponent<Image>().color = Color.green;
                if (GetNumCompleted() == tasks[startManager.currLevel - 1].GetArrayLength() && startManager.currLevel != startManager.maxLevel)
                {
                    ActivateNextLevelButton();
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
        public void IncreaseAllowedLevel()
        {
            if(startManager.maxAllowLevel == startManager.currLevel && startManager.maxAllowLevel < startManager.maxLevel)
            {
                int currentMaxLevel = startManager.maxAllowLevel;
                startManager.SetMaxAllowLevel(currentMaxLevel + 1);
                startManager.CallEventSaveMaxLevel((currentMaxLevel + 1).ToString());
            }
            nextLevelButton.GetComponent<Button>().interactable = false;
        }
        public void LoadLevel(int index)
        {
            startManager.ChangeScene((SceneIndex)index);
        }
        private void LoadTaskStatuses()
        {
            int tasksLenght = tasks.Length;
            if (tasksLenght == startManager.GetTaskStatuses().GetLength(0))
            {
                for (int i = 0; i < tasksLenght; i++)
                {
                    int arrayLenght = tasks[i].GetArrayLength();
                    if (arrayLenght == startManager.GetTaskStatuses().GetLength(1))
                    {
                        for (int j = 0; j < arrayLenght; j++)
                        {
                            tasks[i].SetCompletedValue(j, startManager.GetTaskStatuses()[i, j]);
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
                    startManager.SetTaskStatuses(i, j, tasks[i].GetArrayMember(j).isCompleted);
                    //Debug.Log("Array num: " + i + " task num: " + j + " task status: " + tasks[i].GetArrayMember(j).isCompleted);
                }
            }
            startManager.CallEventTaskUpdate("");
        }
        private void ValidateIfAllowNextLevelOnStart()
        {
            if (GetNumCompleted() == tasks[startManager.currLevel - 1].GetArrayLength() && startManager.maxAllowLevel == startManager.currLevel && startManager.maxAllowLevel < startManager.maxLevel && AreRequirementsMet())
            {
                nextLevelButton.SetActive(true);
                nextLevelButton.GetComponentInChildren<TMP_Text>().text = "Upgrade to the Level (" + (startManager.currLevel + 1).ToString() + ")";
            }
            else if(startManager.playerCoins < playerRequirements[startManager.currLevel - 1].coinsRequired || startManager.playerExperience < playerRequirements[startManager.currLevel - 1].experienceRequired)
            {
                requirementInfText.SetActive(true);
                if (startManager.playerCoins < playerRequirements[startManager.currLevel - 1].coinsRequired && startManager.playerExperience < playerRequirements[startManager.currLevel - 1].experienceRequired)
                {
                    requirementInfText.GetComponentInChildren<TMP_Text>().text = "You need to gain " + (playerRequirements[startManager.currLevel - 1].coinsRequired - startManager.playerCoins) + "coins more and " +
                        "\n" + (playerRequirements[startManager.currLevel - 1].experienceRequired - startManager.playerExperience) + " experience more to unlock next level";
                }
                else if (startManager.playerCoins < playerRequirements[startManager.currLevel - 1].coinsRequired)
                {
                    requirementInfText.GetComponentInChildren<TMP_Text>().text = "You need to gain " + (playerRequirements[startManager.currLevel - 1].coinsRequired - startManager.playerCoins) + " coins more to unlock next level";
                }
                else
                {
                    requirementInfText.GetComponentInChildren<TMP_Text>().text = "You need to gain " + (playerRequirements[startManager.currLevel - 1].experienceRequired - startManager.playerExperience) + " experience more to unlock next level";
                }

            }
        }
        private void ActivateNextLevelButton()
        {
            if (AreRequirementsMet())
            {
                nextLevelButton.SetActive(true);
                nextLevelButton.GetComponentInChildren<TMP_Text>().text = "Upgrade to the Level (" + (startManager.currLevel + 1).ToString() + ")";
            }
        }
        private bool AreRequirementsMet()
        {
            if (startManager.playerCoins >= playerRequirements[startManager.currLevel - 1].coinsRequired && startManager.playerExperience >= playerRequirements[startManager.currLevel - 1].experienceRequired)
            {
                return true;
            }
            else
                return false;
        }
    }
}
