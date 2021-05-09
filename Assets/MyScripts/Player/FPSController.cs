using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;


#pragma warning disable 618, 649
namespace U1
{
    public class FPSController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        private float walkSpeed, runSpeed, jumpSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();

        [SerializeField] private Camera m_Camera;
        [SerializeField] float mouseSensivity = 2f;
        private float currentAxisX, currentAxisY, verticalLookRotation;
        private float m_StepCycle;
        private float m_NextStep;
        private static float characterRadius;
        private bool m_Jump, m_Jumping, m_PreviouslyGrounded;
        private bool hasToFreeze;
        private Vector2 m_Input = Vector2.zero;
        private Vector3 m_MoveDir = Vector3.zero;
        private Vector3 m_OriginalCameraPosition;
        private Vector3 up = Vector3.up;
        private Transform myTransform, myCameraTransform;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private AudioSource m_AudioSource;
        private PlayerMaster playerMaster;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            myTransform = transform;
            myCameraTransform = m_Camera.transform;
            characterRadius = m_CharacterController.radius;
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            playerMaster = GetComponent<PlayerMaster>();
            walkSpeed = m_WalkSpeed; runSpeed = m_RunSpeed; jumpSpeed = m_JumpSpeed;
        }

        private void OnEnable()
        {
            playerMaster.EventControllerFreeze += SwithCanMove;
        }
        private void OnDisable()
        {
            playerMaster.EventControllerFreeze -= SwithCanMove;
        }
        private void Update()
        {
            if(!hasToFreeze)
                Look();
            if (!m_Jump && !hasToFreeze)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }
        private void FixedUpdate()
        {
            if (!hasToFreeze)
            {
                Move();
            }
            //Debug.Log("Speed: " + m_CharacterController.velocity.magnitude);
        }

        private void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
            float speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input.x = horizontal;
            m_Input.y = vertical;
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }
            Vector3 desiredMove = myTransform.forward * vertical + myTransform.right * horizontal;
            RaycastHit hitInfo;
            Physics.SphereCast(myTransform.position, characterRadius, -up, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;

            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;
                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);
        }
        private void UpdateCameraPosition(float speed)
        {
            if (!m_UseHeadBob)
            {
                return;
            }
            Vector3 newCameraPosition;
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                myCameraTransform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = myCameraTransform.localPosition;
                newCameraPosition.y = myCameraTransform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = myCameraTransform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            myCameraTransform.localPosition = newCameraPosition;
        }

        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }
        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }
        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }
        private void Look()
        {
            if (Input.GetAxisRaw("Mouse X") != currentAxisX)
            {
                currentAxisX = Input.GetAxisRaw("Mouse X");
                myTransform.Rotate(up * currentAxisX * mouseSensivity);
            }
            if (Input.GetAxisRaw("Mouse Y") != currentAxisY)
            {
                currentAxisY = Input.GetAxisRaw("Mouse Y");
                verticalLookRotation += currentAxisY * mouseSensivity;
                verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
                myCameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
            }
        }
        void SwithCanMove(bool toState)
        {
            hasToFreeze = toState;
        }
        public void SetMotionParams(float move, float sprint, float jump)
        {
            m_WalkSpeed = walkSpeed-(move*walkSpeed);
            m_RunSpeed = runSpeed - (sprint*runSpeed);
            m_JumpSpeed = jumpSpeed - (jump * runSpeed);
        }
        public float[] GetWalkSpeed()
        {
            float[] speeds = new float[2];
            speeds[0] = m_WalkSpeed;
            speeds[1] = walkSpeed;
            return speeds;
        }
    }
}
