using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.PlayerController.Movement
{
    // ? separate landing, jumping etc into own classes?
    public class Manager
    {
        Vector3 _MovementDirection = Vector3.zero;

        public float LandingBobDuration { get; set; } = 0.4f;

        CharacterController CharacterControllerProperty { get; set; }
        Camera CharacterEnvironmentCamera { get; set; }
        AudioSource CharacterAudioSource { get; set; }
        AudioClip JumpingAudio { get; set; }
        AudioClip LandingAudio { get; set; }
        string JumpButtonName { get; set; }
        string RunButtonName { get; set; }
        string MovementXAxisName { get; set; }
        string MovementYAxisName { get; set; }
        float WalkingSpeed { get; set; }
        float RunningSpeed { get; set; }
        bool IsCameraBobbingEnabled { get; set; }
        float CameraBobIntensityLanding { get; set; }
        float JumpForce { get; set; }
        float AeroMobilityMultiplier { get; set; }
        float GravityMultiplier { get; set; }
        Vector3 CameraLocalPositionStart { get; set; }
        LayerMask NonJumpSurfaceLayers { get; set; }

        Vector3 MovementDirection {
            get => _MovementDirection;
            set => _MovementDirection = value;
        }
        bool WasGroundedOnProperSurface { get; set; } = true;

        // Use to prevent landing after starting & resuming
        // Set to 10 to prevent landing after starting
        byte AfterPauseFrameCountDown { get; set; } = 10;
        float WalkOnNonPlayEndDelay { get; set; } = 0.1f;

        General.Time.DownCounter LandingBobCountDown { get; set; } = new General.Time.DownCounter();
        General.Time.DownCounter WalkOnNonPlayEndDelayCountDown { get; set; } = new General.Time.DownCounter();

        public Manager(
            CharacterController characterControllerProperty,
            Camera characterEnvironmentCamera,
            AudioSource characterAudioSource,
            AudioClip jumpingAudio,
            AudioClip landingAudio,
            string movementXAxisName,
            string movementYAxisName,
            string jumpButtonName,
            string runButtonName,
            float walkingSpeed,
            float runningSpeed,
            bool isCameraBobbingEnabled,
            float cameraBobIntensityLanding,
            float jumpForce,
            float aeroMobilityMultiplier,
            float gravityMultiplier,
            LayerMask nonJumpSurfaceLayers
            )
        {
            CharacterControllerProperty = characterControllerProperty;
            CharacterEnvironmentCamera = characterEnvironmentCamera;
            CharacterAudioSource = characterAudioSource;
            JumpingAudio = jumpingAudio;
            LandingAudio = landingAudio;
            MovementXAxisName = movementXAxisName;
            MovementYAxisName = movementYAxisName;
            JumpButtonName = jumpButtonName;
            RunButtonName = runButtonName;
            WalkingSpeed = walkingSpeed;
            RunningSpeed = runningSpeed;
            IsCameraBobbingEnabled = isCameraBobbingEnabled;
            CameraBobIntensityLanding = cameraBobIntensityLanding;
            JumpForce = jumpForce;
            AeroMobilityMultiplier = aeroMobilityMultiplier;
            GravityMultiplier = gravityMultiplier;
            CameraLocalPositionStart = CharacterEnvironmentCamera.transform.localPosition;
            NonJumpSurfaceLayers = nonJumpSurfaceLayers;
        }

        public void Update(out Vector3 groundMovementDirection)
        {
            bool isGroundedOnProperSurface = 
                IsGroundedOnProperSurface(CharacterControllerProperty, NonJumpSurfaceLayers);
            
            // Movement across surface
            groundMovementDirection =
                (CharacterControllerProperty.transform.forward * Input.GetAxis(MovementYAxisName)) + 
                (CharacterControllerProperty.transform.right * Input.GetAxis(MovementXAxisName));

            CalculateMovementDirection(ref _MovementDirection, groundMovementDirection, isGroundedOnProperSurface);
            Landing(isGroundedOnProperSurface);
            Jumping(isGroundedOnProperSurface);

            // Apply movement to character controller
            CharacterControllerProperty.Move(MovementDirection * Time.deltaTime);
        }
        
        float GetSpeedMultiplyer(bool isGroundedOnProperSurface)
        {
            float speedMultiplyer = WalkingSpeed;

            if (Input.GetButton(RunButtonName) && Input.GetAxis(MovementYAxisName) > 0f)
            {
                speedMultiplyer = RunningSpeed;
            }

            speedMultiplyer *= isGroundedOnProperSurface ? 1f : AeroMobilityMultiplier;

            return speedMultiplyer;
        }

        void CalculateMovementDirection(
            ref Vector3 movementDirection,
            Vector3 groundMovementDirection,
            bool isGroundedOnProperSurface
            )
        {
            movementDirection =
                new Vector3(groundMovementDirection.x, _MovementDirection.y, groundMovementDirection.z);

            // Forces Player to walk off if on top of NonPlayer
            if (CharacterControllerProperty.isGrounded && !isGroundedOnProperSurface)
            {
                WalkOnNonPlayEndDelayCountDown.Start(WalkOnNonPlayEndDelay);
            }

            if (!WalkOnNonPlayEndDelayCountDown.IsCounting)
            {
                float speedMultiplyer = GetSpeedMultiplyer(isGroundedOnProperSurface);

                movementDirection.x *= speedMultiplyer;
                movementDirection.z *= speedMultiplyer;
            }
            else
            {
                movementDirection = CharacterControllerProperty.gameObject.transform.forward * WalkingSpeed;
                movementDirection.y = -10f; // Needed to make movement work propery
            }

            WalkOnNonPlayEndDelayCountDown.Update();

        }

        void Landing(bool isGroundedOnProperSurface)
        {
            // For preventing landing after resuming
            if (Scripts.UI.PauseMenu.IsGamePaused || Scripts.UI.EndGameMenu.HasGameEnded)
            {
                AfterPauseFrameCountDown = 10;
            }

            // Prevents landing after starting & resuming
            if (AfterPauseFrameCountDown <= 0)
            {
                // Landing
                // When paused (with timeScale), isGrounded automatically sets to false
                if (isGroundedOnProperSurface && !WasGroundedOnProperSurface)
                {
                    CharacterAudioSource.clip = LandingAudio;
                    CharacterAudioSource.Play();

                    LandingBobCountDown.Start(LandingBobDuration);

                    WasGroundedOnProperSurface = true;
                }
            }
            else
            {
                AfterPauseFrameCountDown--;
            }




            // Prevent landing bob if moving across surface
            //if (groundMovementDirection.magnitude > 0f)
            if (_MovementDirection.magnitude > 0f)
            {
                LandingBobCountDown.End();
            }

            LandingBobOverTime();



        }

        void Jumping(bool isGroundedOnProperSurface)
        {
            // Jumping & airborne
            if (isGroundedOnProperSurface)
            {
                // Needed to make movement work propery
                _MovementDirection.y = -10f;

                if (Input.GetButton(JumpButtonName))
                {
                    _MovementDirection.y = JumpForce;

                    CharacterAudioSource.clip = JumpingAudio;
                    CharacterAudioSource.Play();
                }
            }
            else // When paused (with timeScale), isGrounded automatically sets to false
            {
                MovementDirection += Physics.gravity * GravityMultiplier * Time.deltaTime;

                // Prevent isGrounded from affecting WasGrounded during timeScale pause
                if (AfterPauseFrameCountDown <= 0)
                {
                    // As propery airborn, allow landing to occur
                    WasGroundedOnProperSurface = false;
                }
            }
        }

        void LandingBobOverTime()
        {
            if (!LandingBobCountDown.HasCountDownEnded || IsCameraBobbingEnabled)
            {
                float currentSine = Mathf.Sin(LandingBobCountDown.CurrentCount);
                CharacterEnvironmentCamera.transform.localPosition = 
                    CameraLocalPositionStart + new Vector3(0f, currentSine * CameraBobIntensityLanding, 0f);
            }

            LandingBobCountDown.Update();
        }

        bool IsGroundedOnProperSurface(
            CharacterController characterController,
            LayerMask improperSurfaceLayers
            )
        {
            if (CharacterControllerProperty.isGrounded)
            {
                RaycastHit[] hits =
                Physics.SphereCastAll(
                    characterController.transform.position + new Vector3(0f, -characterController.height / 2f, 0f),
                    characterController.radius,
                    Vector3.down,
                    0.1f
                    );

                foreach (RaycastHit h in hits)
                {
                    if (DoesHitMatchLayer(h, improperSurfaceLayers))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        bool DoesHitMatchLayer(RaycastHit hit, LayerMask layersToCheck)
        {
            if (
                    // Access the relavent LayerMash bit by bit shifting with Layer number
                    1 == (1 & (layersToCheck >> hit.collider.gameObject.layer))
                    &&
                    hit.collider.enabled
                    &&
                    !hit.collider.isTrigger
                )
            {
                return true;
            }

            return false;
        }
    }
}
