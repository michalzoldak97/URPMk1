using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace U1
{
    
    public class PlanManager : MonoBehaviour
    {
        [SerializeField] Transform uiObjParentTransform;
        PlaceableObject[] myObjects;
        List<GameObject> uiObjects = new List<GameObject>();
        List<GameObject> mapAliases = new List<GameObject>();
        List<Vector3> worldPosToPas = new List<Vector3>();
        GetCameraClick cameraClick;
        SceneStartManager sceneManager;
        bool isOptionSelected;
        int buttonNumSelected;
        int count;

        void SetInit()
        {
            cameraClick = GetComponent<GetCameraClick>();
            sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStartManager>();
        }

        private void OnEnable()
        {
            SetInit();
            sceneManager.EventStartPlan += SetPlaceableObjects;
            sceneManager.EventEndPlan += PassCurrentPossitions;
        }
        private void OnDisable()
        {
            sceneManager.EventStartPlan -= SetPlaceableObjects;
            sceneManager.EventEndPlan -= PassCurrentPossitions;
        }

        public void SetPlaceableObjects()
        {
            myObjects = sceneManager.GetPlaceableObjects();
            CreateObjectsUI();
        }
        void CreateObjectsUI()
        {
            for (int i = 0; i < myObjects.Length; i++)
            {
                for (int j = 0; j < myObjects[i].numOfOwnedObjects; j++)
                {
                    /*GameObject objUI = Instantiate(myObjects[i].objButon, uiObjParentTransform);
                    uiObjects.Add(objUI);*/
                    GameObject alias = Instantiate(myObjects[i].mapAlias, Vector3.zero, Quaternion.Euler(0,0,0));
                    mapAliases.Add(alias);
                    worldPosToPas.Add(alias.transform.position);
                }
            }
            for (int i = 0; i < uiObjects.Count; i++)
            {
                int x = i;
                uiObjects[i].GetComponent<Button>().onClick.AddListener(delegate { SetCurrentSelection(x); });
            }
        }
        void SetCurrentSelection(int index)
        {
            //Debug.Log("Index called with " + index);
            isOptionSelected = true;
            buttonNumSelected = index;
            for (int i = 0; i < uiObjects.Count; i++)
            {
                uiObjects[i].GetComponentInChildren<Image>().enabled = false;
            }
            uiObjects[buttonNumSelected].GetComponentInChildren<Image>().enabled = true;
        }
        private void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                sceneManager.ChangeScene(1);
            }
            if(Input.GetMouseButtonDown(0))
            {
                if (isOptionSelected)
                {
                    CheckForClick();
                }
                else
                    SetCurrentSelection(0);
            }
        }

        void CheckForClick()
        {
            Vector3 tempClick = cameraClick.CheckClick();
            if (tempClick != Vector3.zero)
            {
                mapAliases[buttonNumSelected].transform.position = tempClick;
                worldPosToPas[buttonNumSelected] = tempClick;
            }
        }
        void PassCurrentPossitions()
        {
            count = 0;
            for (int i = 0; i < myObjects.Length; i++)
            {
            myObjects[i].worldPositions = new Vector3[myObjects[i].numOfOwnedObjects];
                for (int j = 0; j < myObjects[i].numOfOwnedObjects; j++)
                {
                    myObjects[i].worldPositions[j] = worldPosToPas[count];
                    count++;
                }
            }
            /*for (int i = 0; i < myObjects.Length; i++)
            {
                Debug.Log(" pass" + myObjects[i].worldPositions[i]);
            }*/
            sceneManager.SetPlaceableObjects(myObjects);
        }
    }
}
