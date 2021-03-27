using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class GetCameraClick : MonoBehaviour
    {
        Ray ray;
        RaycastHit hit;
        Camera myCamera;
        Vector3 emptySpace = new Vector3(-100, -100, -100);
        [SerializeField] LayerMask clickableLayer;
        void Start()
        {
            myCamera = Camera.main;
        }

        public Vector3 CheckClick()
        {
            ray = myCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, clickableLayer))
            {
                if (clickableLayer == (clickableLayer | (1 << (hit.transform.gameObject.layer))))
                {
                    Debug.Log(hit.point);
                    return hit.point;
                }
                else
                {
                    Debug.Log("layer not gut Click");
                    return emptySpace;
                }
            }
            {
                Debug.Log("no collider");
                return emptySpace;
            }
        }
    }
}


