using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class PlanManager : MonoBehaviour
    {
        [SerializeField] private Transform uIContentTransform;
        [SerializeField] GameObject planButton;
        private Vector3 startPosition = new Vector3(-100, -100, -100);
        private int[] currSelectedID = new int[2];
        private bool canGetClick = true;
        public void SetCanGetClick(bool toSet)
        {
            canGetClick = toSet;
        }
        private PlaceableObject[] myPlaceableObj;
        private List<PlanPOButton> planButtons = new List<PlanPOButton>();
        private GetCameraClick cameraClick;
        private SceneStartManager startManager;
        private SceneLoadQualityManager sceneLoadManager;
        private void SetInitials()
        {
            startManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
            sceneLoadManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoadQualityManager>();
            myPlaceableObj = startManager.GetPlaceableObjects();
            cameraClick = GetComponent<GetCameraClick>();
            ClearPOPositions();
            SetUpUi();
            if (planButtons.Count > 0)
            {
                planButtons[0].SelectButton();
            }
            else
                currSelectedID[0] = 999999;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckForClick();
            }
        }
        private void OnEnable()
        {
            SetInitials();
            startManager.EventEndPlan += SetNewPOToManager;
        }
        private void OnDisable()
        {
            startManager.EventEndPlan -= SetNewPOToManager;
        }
        void CheckForClick()
        {
            Vector3 tempClick = cameraClick.CheckClick();
            if (tempClick != startPosition && currSelectedID[0] != 999999 && canGetClick)
            {
                myPlaceableObj[currSelectedID[0]].worldPositions[currSelectedID[1]] = tempClick;
                for (int i = 0; i < planButtons.Count; i++)
                {
                    if (planButtons[i].GetCoordinates()[0] == currSelectedID[0] && planButtons[i].GetCoordinates()[1] == currSelectedID[1])
                    {
                        planButtons[i].SetUpMapIcon(tempClick);
                    }
                }
            }
        }
        private void ClearPOPositions()
        {
            for (int i = 0; i < myPlaceableObj.Length; i++)
            {
                myPlaceableObj[i].worldPositions = new Vector3[myPlaceableObj[i].numOfObjOnStack];
                for (int j = 0; j < myPlaceableObj[i].worldPositions.Length; j++)
                {
                    myPlaceableObj[i].worldPositions[j] = startPosition;
                }
            }
        }
        private void SetUpUi()
        {
            planButtons.Clear();
            foreach (Transform child in uIContentTransform)
            {
                Destroy(child.gameObject);
            }
            int POlength = myPlaceableObj.Length;
            for (int i = 0; i < PlaceableObject.numOfObjTypes; i++)
            {
                for (int j = 0; j < POlength; j++)
                {
                    if (myPlaceableObj[j].isAvailable && myPlaceableObj[j].objType == i
                        && myPlaceableObj[j].isAddedToStack && myPlaceableObj[j].numOfObjOnStack > 0)
                    {
                        for (int k = 0; k < myPlaceableObj[j].numOfObjOnStack; k++)
                        {
                            GameObject PPOB = Instantiate(planButton, uIContentTransform);
                            PlanPOButton pButton = PPOB.GetComponent<PlanPOButton>();
                            pButton.SetUpButton(this, myPlaceableObj[j], j, k);
                            pButton.SetUpMapIcon(myPlaceableObj[j].worldPositions[k]);
                            planButtons.Add(pButton);
                        }
                    }
                }
            }
        }
        public void SelectUiObj(int idx, int vIdx)
        {
            currSelectedID[0] = idx;
            currSelectedID[1] = vIdx;
            for (int i = 0; i < planButtons.Count; i++)
            {
                planButtons[i].ResetColor();
            }
        }
        public void LoadPreviousLevel()
        {
            startManager.ChangeScene(2);
        }
        public void LoadNextLevel()
        {
            sceneLoadManager.LoadGameScene();
        }
        private void SetNewPOToManager()//call on level change
        {
            startManager.SetPlaceableObjects(myPlaceableObj);
            for (int i = 0; i < startManager.GetPlaceableObjects().Length; i++)
            {
                if (startManager.GetPlaceableObjects()[i].isAddedToStack)
                {
                    for (int j = 0; j < startManager.GetPlaceableObjects()[i].worldPositions.Length; j++)
                    {
                        Debug.Log("Coords: " + startManager.GetPlaceableObjects()[i].worldPositions[j] + "obj name: " + startManager.GetPlaceableObjects()[i].objectName);
                    }
                }
            }
        }
    }
}
