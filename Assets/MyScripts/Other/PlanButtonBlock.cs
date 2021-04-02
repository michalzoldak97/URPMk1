using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace U1
{
    public class PlanButtonBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private PlanManager planManager;
        private void Awake()
        {
            planManager =  GameObject.FindGameObjectWithTag("GEC").GetComponent<PlanManager>();
        }
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            planManager.SetCanGetClick(false);
        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            planManager.SetCanGetClick(true);
        }
    }
}
