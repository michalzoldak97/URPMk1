using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace U1
{
    public class PlayerCheckGrounded : MonoBehaviour
    {
		[SerializeField] GameObject myPlayer;
		private PlayerController playerController;
		public bool isGrounded { get; private set; }

		void Start()
		{
			playerController = myPlayer.GetComponent<PlayerController>();
		}
		void OnTriggerEnter(Collider other)
		{
			//Debug.Log("OnTriggerEnter");
			if (other.gameObject == myPlayer)
				return;

			isGrounded = true;
			//playerController.SetGroundedState(isGrounded);
			//Debug.Log("Set grounded state true");
		}

		void OnTriggerExit(Collider other)
		{
			//Debug.Log("OnTriggerExit");
			if (other.gameObject == myPlayer)
				return;

			isGrounded = false;
			//playerController.SetGroundedState(isGrounded);
			//Debug.Log("Set grounded state false");
		}

		void OnTriggerStay(Collider other)
		{
			Debug.Log("OnTriggerStay");
			if (other.gameObject == myPlayer)
				return;

			isGrounded = true;
			//playerController.SetGroundedState(isGrounded);
			//Debug.Log("Set grounded state true");
		}
	}
}
