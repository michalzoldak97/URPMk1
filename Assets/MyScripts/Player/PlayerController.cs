using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class PlayerController : MonoBehaviour
    {
		[SerializeField] GameObject camera;
		[SerializeField] GameObject groundChecker;
		[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
		float verticalLookRotation;
		private float currentAxisX;
		private float currentAxisY;
		private bool setMoveAmountToZero;
		private bool hasToZero;
		private bool canJump = true;
		private bool isOnFlight;
		private bool checkIsOnFlight;
		bool grounded;
		bool hasToFreeze;
		Vector3 smoothMoveVelocity;
		Vector3 moveAmount;
		Vector3 moveDir = new Vector3(0, 0, 0);

		Rigidbody rb;
		private PlayerCheckGrounded fpsGC;
		private PlayerMaster playerMaster;

		void Start()
		{
			SetReferences();
		}
		void SetReferences()
		{
			rb = GetComponent<Rigidbody>();
			rb.isKinematic = true;
			fpsGC = groundChecker.GetComponent<PlayerCheckGrounded>();
			playerMaster = GetComponent<PlayerMaster>();
		}

		private void OnEnable()
		{
			SetReferences();
			playerMaster.EventControllerFreeze += FreezeUnfreeze;
		}
		private void OnDisable()
		{
			playerMaster.EventControllerFreeze -= FreezeUnfreeze;
		}

		void Update()
		{
			if (!hasToFreeze)
			{
				Look();
				Move();
				Jump();
			}
		}

		void Look()
		{
			if (Input.GetAxisRaw("Mouse X") != currentAxisX)
			{
				currentAxisX = Input.GetAxisRaw("Mouse X");
				transform.Rotate(Vector3.up * currentAxisX * mouseSensitivity);
				//Debug.Log("X event happen");
			}
			if (Input.GetAxisRaw("Mouse Y") != currentAxisY)
			{
				currentAxisY = Input.GetAxisRaw("Mouse Y");
				verticalLookRotation += currentAxisY * mouseSensitivity;
				verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

				camera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
				//Debug.Log("Y event happen");
			}
		}

		void Move()
		{
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
			{
				moveDir.x = Input.GetAxisRaw("Horizontal");
				moveDir.z = Input.GetAxisRaw("Vertical");
				moveDir = moveDir.normalized;
				moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
			
				setMoveAmountToZero = true;
			}
			else if (setMoveAmountToZero)
			{
				hasToZero = true;
				setMoveAmountToZero = false;
			}
		}

		void Jump()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				/*if(grounded)
				{
					rb.AddForce(transform.up * jumpForce);
				}*/
				if (canJump)
				{
					canJump = false;
					StartCoroutine(CheckGroundedState());
				}

			}
			if (checkIsOnFlight)
			{
				if (!fpsGC.isGrounded)
				{
					isOnFlight = true;
				}
				else
				{
					canJump = true;
					checkIsOnFlight = false;
					isOnFlight = false;
					groundChecker.SetActive(false);
					rb.isKinematic = true;
				}
			}
		}
		IEnumerator CheckGroundedState()
		{
			groundChecker.SetActive(true);
			yield return new WaitForSecondsRealtime(0.1f);
			if (fpsGC.isGrounded)
			{
				Debug.Log("Space down, is grounded: " + fpsGC.isGrounded);
				rb.isKinematic = false;
				rb.AddForce(transform.up * jumpForce);
				yield return new WaitForSecondsRealtime(0.1f);
				checkIsOnFlight = true;
			}
		}
		public void SetGroundedState(bool _grounded)
		{
			grounded = _grounded;
		}

		void FixedUpdate()
		{
			if (setMoveAmountToZero)
				rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
			else if(hasToZero)
			{
				moveAmount = Vector3.zero;
				hasToZero = false;
			}
			
		}
		void FreezeUnfreeze(bool toState)
		{
			if (toState)
				hasToFreeze = true;
			else
				hasToFreeze = false;
		}
	}
}