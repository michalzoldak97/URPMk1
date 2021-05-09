using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace U1
{
    public class POInfoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Transform myTransform;
        private POShopButton myParentButton;
        void Start()
        {
            myTransform = transform;
            myParentButton = GetComponentInParent<POShopButton>();
        }
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            myParentButton.InformOnInfoEnter(myTransform);
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            myParentButton.InformOnInfoExit();
        }
    }
}
