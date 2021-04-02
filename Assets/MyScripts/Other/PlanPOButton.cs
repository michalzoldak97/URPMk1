using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace U1
{
    public class PlanPOButton : MonoBehaviour
    {
        [SerializeField] Image myIcon;
        [SerializeField] TMP_Text myText;
        [SerializeField] GameObject iconToSpawn, warning;
        private GameObject myMapIcon;
        private Image backGroundImage;
        private Color32 initialColor;
        private PlanManager planManager;
        private PlaceableObject myPO;
        private int index, vectorID;
        public void SetUpButton(PlanManager myPlanManager, PlaceableObject poToSet, int idx, int vecID)
        {
            planManager = myPlanManager;
            myPO = poToSet;
            vectorID = vecID;
            backGroundImage = GetComponent<Image>();
            initialColor = backGroundImage.color;
            index = idx;
            myIcon.sprite = poToSet.objIcon;
            myText.text = poToSet.objectName;
        }
        public void SetUpMapIcon(Vector3 pos)
        {
            if (myMapIcon != null)
                Destroy(myMapIcon);
            myMapIcon = Instantiate(iconToSpawn);
            myMapIcon.GetComponentInChildren<Image>().sprite = myIcon.sprite;
            myMapIcon.transform.position = pos;
            //Debug.Log(pos);
            if(pos.x > -100 && pos.y > -100 && pos.z > -100)
                warning.SetActive(false);
        }
        public void SelectButton()
        {
            planManager.SelectUiObj(index, vectorID);
            backGroundImage.color = new Color32(148, 248, 255, 255);
        }
        public void ResetColor()
        {
            backGroundImage.color = initialColor;
        }
        public Sprite GetObjSprite()
        {
            return myPO.objIcon;
        }
        public int[] GetCoordinates()
        {
            int[] toReturn = new int[2];
            toReturn[0] = index;
            toReturn[1] = vectorID;
            return toReturn;
        }
    }
}
