using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace U1
{
    [System.Serializable]
    public struct Task
    {
        public Sprite taskImage;
        public string taskSolution;
        public bool isCompleted;
    }
    [System.Serializable]
    public class TaskArray
    {
        [SerializeField] private Task[] taskArray;
        public int GetArrayLength()
        {
            return taskArray.Length;
        }
        public Task GetArrayMember(int index)
        {
            return taskArray[index];
        }
        public void SetCompletedValue(int index, bool toSet)
        {
            taskArray[index].isCompleted = toSet;
        }
    }
}