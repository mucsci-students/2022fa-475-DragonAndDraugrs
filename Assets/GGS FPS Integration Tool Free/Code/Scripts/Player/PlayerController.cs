using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.Player
{
    [RequireComponent(typeof(CharacterController), typeof(AudioSource))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Camera _CharacterEnvironmentCamera;

        [Header("Movement Inputs")]
        [SerializeField] string _MovementXAxisName;
        [SerializeField] string _MovementYAxisName;
        [SerializeField] string _JumpButtonName;
        [SerializeField] string _RunButtonName;

        [Header("Mouse Inputs")]
        [SerializeField] string _MouseXAxisName;
        [SerializeField] string _MouseYAxisName;

        [Header("Movement Speeds")]
        [SerializeField] float _WalkingSpeed = 5f;
        [SerializeField] float _RunningSpeed = 10f;

        [Header("Mouse Sensitivities")]
        [SerializeField] float _MouseXSensitivity = 1f;
        [SerializeField] float _MouseYSensitivity = 1f;

        [Header("Aero Mobility")]
        [SerializeField] float _JumpForce = 6f;
        [SerializeField] float _AeroMobilityMultiplier = 1f;
        [SerializeField] float _GravityMultiplier = 2f;

        [Header("Camera Bobbing Effects")]
        [SerializeField] float _WalkCycleRate = 2f;
        [SerializeField] bool _IsCameraBobbingEnabled = true;
        [SerializeField] float _CameraMovementBobIntensity = 0.05f;
        [SerializeField] float _CameraLandingBobIntensity = 0.2f;

        [Header("Audio")]
        [SerializeField] AudioClip _FirstFootstepAudio;
        [SerializeField] AudioClip _SecondFootstepAudio;
        [SerializeField] AudioClip _JumpingAudio;
        [SerializeField] AudioClip _LandingAudio;

        [Header("Layers")]
        [SerializeField] LayerMask _NonJumpSurfaceLayers;

        Utilities.PlayerController.Manager PlayerControllerManager { get; set; }

        void Awake()
        {
            Initialise();
        }

        void Update()
        {
            PlayerControllerManager.Update();
        }

        public void Initialise()
        {
            PlayerControllerManager = new Utilities.PlayerController.Manager(
                GetComponent<CharacterController>(),
                GetComponent<AudioSource>(),
                _CharacterEnvironmentCamera,
                _MovementXAxisName,
                _MovementYAxisName,
                _MouseXAxisName,
                _MouseYAxisName,
                _JumpButtonName,
                _RunButtonName,
                _WalkingSpeed,
                _RunningSpeed,
                _MouseXSensitivity,
                _MouseYSensitivity,
                _JumpForce,
                _AeroMobilityMultiplier,
                _GravityMultiplier,
                _WalkCycleRate,
                _IsCameraBobbingEnabled,
                _CameraMovementBobIntensity,
                _CameraLandingBobIntensity,
                _FirstFootstepAudio,
                _SecondFootstepAudio,
                _JumpingAudio,
                _LandingAudio,
                _NonJumpSurfaceLayers
                );
        }
    }
}