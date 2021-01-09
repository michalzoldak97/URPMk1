using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class PlayerDetectItem : MonoBehaviour
    {
        [SerializeField] LayerMask layerToDetect;
        public Transform requestTransform;
        [Tooltip("Input button to pick up")]
        private Transform itemInRange;
        private RaycastHit hit;
        public float detectRange { get; set; }
        public float rayRadius { get; set; }
        private bool IsInRange;

        [SerializeField]private float labelWidth = 200f;
        [SerializeField]private float labelHeight = 50f;

        private void Start()
        {
            detectRange = 3;
            rayRadius = 0.7f;
        }

        private void Update()
        {
            CastRayForDetectingItems();
            CheckForItemPickUpAttempt();
        }
        void CastRayForDetectingItems()
        {
            if (Physics.SphereCast(requestTransform.position, rayRadius, requestTransform.forward, out hit, detectRange, layerToDetect))
            {
                itemInRange = hit.transform;
                IsInRange = true;
            }
            else
            {
                IsInRange = false;
            }
        }
        void CheckForItemPickUpAttempt()
        {
            if (Input.GetKeyDown(KeyCode.E) && Time.timeScale > 0 && IsInRange && !itemInRange.root.CompareTag("Player"))
            {
                //Debug.Log("Pickup attempted");
                if (requestTransform != null)
                {
                    itemInRange.GetComponent<ItemMaster>().CallEventPickupRequested(requestTransform);
                }
            }
        }

        void OnGUI()
        {
            if (IsInRange && itemInRange != null)
            {
                GUI.Label(new Rect(Screen.width / 2 - labelWidth, Screen.height / 2, labelWidth, labelHeight), itemInRange.name + " " + itemInRange.GetComponent<ItemMaster>().displayOnGui);
            }
        }
    }
}
