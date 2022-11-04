using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.PlayerController
{
    /// <summary>
    /// Manages functionality for PlayerController script.
    /// </summary>
    public class Manager
    {
        Movement.Manager MovementManager { get; set; }
        Rotation.Manager RotationManager { get; set; }
        WalkCycle.Manager WalkCycleManager { get; set; }

        public Manager(
            CharacterController characterControllerProperty,
            AudioSource characterAudioSource,
            Camera characterEnvironmentCamera,
            string movementXAxisName,
            string movementYAxisName,
            string mouseXAxisName,
            string mouseYAxisName,
            string jumpButtonName,
            string runButtonName,
            float walkingSpeed,
            float runningSpeed,
            float mouseXSensitivity,
            float mouseYSensitivity,
            float jumpForce,
            float aeroMobilityMultiplier,
            float gravityMultiplier,
            float walkCycleRate,
            bool isCameraBobbingEnabled,
            float cameraMovementBobIntensity,
            float cameraLandingBobIntensity,
            AudioClip firstFootstepAudio,
            AudioClip secondFootstepAudio,
            AudioClip jumpingAudio,
            AudioClip landingAudio,
            LayerMask nonJumpSurfaceLayers
            )
        {
            MovementManager = new Movement.Manager(
                characterControllerProperty,
                characterEnvironmentCamera,
                characterAudioSource,
                jumpingAudio,
                landingAudio,
                movementXAxisName,
                movementYAxisName,
                jumpButtonName,
                runButtonName,
                walkingSpeed,
                runningSpeed,
                isCameraBobbingEnabled,
                cameraLandingBobIntensity,
                jumpForce,
                aeroMobilityMultiplier,
                gravityMultiplier,
                nonJumpSurfaceLayers
                );

            RotationManager = new Rotation.Manager(
                characterControllerProperty,
                characterEnvironmentCamera,
                mouseXAxisName,
                mouseYAxisName,
                mouseXSensitivity,
                mouseYSensitivity
                );

            WalkCycleManager = new WalkCycle.Manager(
                characterControllerProperty,
                characterEnvironmentCamera,
                characterAudioSource,
                firstFootstepAudio,
                secondFootstepAudio,
                movementYAxisName,
                runButtonName,
                walkingSpeed,
                runningSpeed,
                walkCycleRate,
                isCameraBobbingEnabled,
                cameraMovementBobIntensity
                );
        }
        
        /// <summary>
        /// Movement to update.
        /// </summary>
        public void Update()
        {
            Vector3 groundMovementDirection;

            MovementManager.Update(out groundMovementDirection);
            RotationManager.Update();
            WalkCycleManager.Update(groundMovementDirection);
        }
    }
}