using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.PlayerController.WalkCycle
{
    public class Manager
    {
        CharacterController CharacterControllerProperty { get; set; }
        Camera CharacterCamera { get; set; }
        AudioSource CharacterAudioSource { get; set; }
        AudioClip FirstFootstepAudio { get; set; }
        AudioClip SecondFootstepAudio { get; set; }
        string MovementYAxisName { get; set; }
        string RunButtonName { get; set; }
        float WalkingSpeed { get; set; }
        float RunningSpeed { get; set; }
        float WalkCycleRate { get; set; } // ? could be optional property
        bool IsCameraBobbingEnabled { get; set; }
        float CameraBobIntensityMoving { get; set; }
        float CurrentWalkCycleTime { get; set; } = 0f;
        bool IsSinePeakPassed { get; set; } = false;
        bool IsSecondFootstepNext { get; set; } = false;

        Vector3 CameraLocalPositionStart { get; set; }


        public Manager(
            CharacterController characterControllerProperty,
            Camera characterEnvironmentCamera,
            AudioSource characterAudioSource,
            AudioClip firstFootstepAudio,
            AudioClip secondFootstepAudio,
            string movementYAxisName,
            string runButtonName,
            float walkingSpeed,
            float runningSpeed,
            float walkCycleRate,
            bool isCameraBobbingEnabled,
            float cameraBobIntensityMoving
            )
        {
            CharacterControllerProperty = characterControllerProperty;
            CharacterCamera = characterEnvironmentCamera;
            CharacterAudioSource = characterAudioSource;
            FirstFootstepAudio = firstFootstepAudio;
            SecondFootstepAudio = secondFootstepAudio;
            MovementYAxisName = movementYAxisName;
            RunButtonName = runButtonName;
            WalkingSpeed = walkingSpeed;
            RunningSpeed = runningSpeed;
            WalkCycleRate = walkCycleRate;
            IsCameraBobbingEnabled = isCameraBobbingEnabled;
            CameraBobIntensityMoving = cameraBobIntensityMoving;
            CameraLocalPositionStart = CharacterCamera.transform.localPosition;
        }

        public void Update(Vector3 groundMovementDirection)
        {
            // Reture if not moving across surface
            if (groundMovementDirection.magnitude <= 0f)
            {
                return;
            }

            // Reture if airborne
            if (!CharacterControllerProperty.isGrounded)
            {
                return;
            }

            const float pi = Mathf.PI;




            //float walkCycleRate = Input.GetButton("Run") ? (WalkCycleRate * (RunningSpeed / WalkingSpeed)) : WalkCycleRate;
             
            //#
            float walkCycleRate = WalkCycleRate;
            if (Input.GetButton(RunButtonName) && Input.GetAxis(MovementYAxisName) > 0f) // !
            {
                walkCycleRate = WalkCycleRate * (RunningSpeed / WalkingSpeed);
            }





            CurrentWalkCycleTime += Time.deltaTime * pi * 2f * walkCycleRate;

            // Reset cycle
            if (CurrentWalkCycleTime > pi * 2f)
            {
                CurrentWalkCycleTime -= (pi * 2f);
            }

            // Determine if peak has passed & prepare for footstep
            if (CurrentWalkCycleTime > pi * 0.5f && CurrentWalkCycleTime <= pi * 1.5f)
            {
                IsSinePeakPassed = true;
            }

            float currentSine = Mathf.Sin(CurrentWalkCycleTime);

            // Bob camera while moving across surface
            if (IsCameraBobbingEnabled)
            {
                CharacterCamera.transform.localPosition = CameraLocalPositionStart + new Vector3(0f, currentSine * CameraBobIntensityMoving, 0f);
            }

            // Play footstep
            if (IsSinePeakPassed && !(CurrentWalkCycleTime > pi * 0.5f && CurrentWalkCycleTime <= pi * 1.5f))
            {
                CharacterAudioSource.clip = IsSecondFootstepNext ? SecondFootstepAudio : FirstFootstepAudio;
                CharacterAudioSource.Play();

                IsSinePeakPassed = false;
                IsSecondFootstepNext = !IsSecondFootstepNext;
            }
        }
    }
}