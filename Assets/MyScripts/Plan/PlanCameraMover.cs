using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class PlanCameraMover : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Vector3 worldBounds;
        [SerializeField] private float moveSpeed, mouseSensitivity;
        [SerializeField] private Vector3 cameraStablePos;
        private bool shouldMove, shouldRotate;
        private int dir = 9;
        private float currentAxisX, currentAxisY;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                shouldRotate = !shouldRotate;
                if(!shouldRotate)
                {
                    cameraTransform.localEulerAngles = cameraStablePos;
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                SetMoveCamera(true, 0);
            }
            else if(Input.GetKey(KeyCode.S))
            {
                SetMoveCamera(true, 1);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                SetMoveCamera(true, 2);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                SetMoveCamera(true, 3);
            }
            else if(shouldMove)
            {
                SetMoveCamera(false, 9);
            }
            if(shouldRotate)
                Look();
        }
        private void SetMoveCamera(bool shouldMove, int dir)
        {
            this.dir = dir;
            this.shouldMove = shouldMove;
        }
        private void FixedUpdate()
        {
            if(shouldMove)
            {
                MoveCamera(dir);
            }
        }
        private void MoveCamera(int dir)
        {
            switch (dir)
            {
                case (0):
                    if (cameraTransform.position.z < worldBounds.z)
                    {
                        Debug.Log("camera transform: " + cameraTransform.position.z + "  bound: " + worldBounds);
                        cameraTransform.Translate(Vector3.forward * moveSpeed, Space.World);
                    }
                    break;
                case (1):
                    if (cameraTransform.position.z > -worldBounds.z)
                        cameraTransform.Translate(-Vector3.forward * moveSpeed, Space.World);
                    break;
                case (2):
                    if (cameraTransform.position.x > -worldBounds.x)
                        cameraTransform.Translate(Vector3.left * moveSpeed, Space.World);
                    break;
                case (3):
                    if (cameraTransform.position.x < worldBounds.x)
                        cameraTransform.Translate(Vector3.right * moveSpeed, Space.World);
                    break;
                default:
                    break;
            }
        }
        private void Look()
        {
            Vector3 coords = cameraTransform.position;
            currentAxisX = Input.GetAxisRaw("Mouse X");
            cameraTransform.Rotate(cameraTransform.up * (currentAxisX * mouseSensitivity), Space.World);
            //cameraTransform.localEulerAngles = Vector3.up * currentAxisX * currentAxisX;

            currentAxisY = Input.GetAxisRaw("Mouse Y");
            cameraTransform.Rotate(cameraTransform.right * -(currentAxisY * mouseSensitivity), Space.World);
            coords.Set(cameraTransform.localEulerAngles.x, cameraTransform.localEulerAngles.y, 0);
            /*verticalLookRotation += currentAxisY * mouseSensitivity;*/
            //verticalLookRotation = Mathf.Clamp(verticalLookRotation, 0f, -120f);
            cameraTransform.localEulerAngles = coords;
        }
    }
}
