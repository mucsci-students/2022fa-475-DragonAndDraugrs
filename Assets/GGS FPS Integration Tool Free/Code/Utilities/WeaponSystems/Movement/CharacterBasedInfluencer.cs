using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Movement
{
    public class CharacterBasedInfluencer
    {
        public bool IsJumping { get; private set; } = false;
        public bool IsRunning { get; private set; } = false;
        public bool IsWalking { get; private set; } = false;

        public float RunningRecoveryCountDown { get; private set; } = 0f;

        CharacterController CharacterControllerProperty { get; set; }
        Animator AnimatorProperty { get; set; }

        public CharacterBasedInfluencer(
            CharacterController characterController,
            Animator animator
            )
        {
            CharacterControllerProperty = characterController;
            AnimatorProperty = animator;
        }

        public void Update(
            Scripts.Weapon weapon,
            Input.Manager inputManager
            )
        {
            // Decrement running transition
            if (RunningRecoveryCountDown > 0f)
            {
                RunningRecoveryCountDown -= 1f * Time.deltaTime;
            }
            else
            {
                RunningRecoveryCountDown = 0f;
            }

            IsJumping = false;
            IsRunning = false;
            IsWalking = false;

            if (!CharacterControllerProperty.isGrounded)
            {
                IsJumping = true;
            }
            
            // Changed from Else If to If to prevent firing while running & jumping/airborn
            if (new Vector2(CharacterControllerProperty.velocity.x, CharacterControllerProperty.velocity.z).magnitude > 0f)
            {
                if (inputManager.IsRunDetected && inputManager.IsForwardMovementDetected)
                {
                    IsRunning = true;
                }
                else
                {
                    IsWalking = true;
                }
            }

            // Animation effected by the states
            AnimatorProperty.SetBool(weapon.JumpAnimationParameterName, false);
            AnimatorProperty.SetBool(weapon.RunAnimationParameterName, false);
            AnimatorProperty.SetBool(weapon.WalkAnimationParameterName, false);

            if (IsJumping)
            {
                AnimatorProperty.SetBool(weapon.JumpAnimationParameterName, true);
            }
            else if (IsRunning)
            {
                AnimatorProperty.SetBool(weapon.RunAnimationParameterName, true);
                RunningRecoveryCountDown = weapon.RunningRecoveryTime;
            }

            if (IsWalking)
            {
                AnimatorProperty.SetBool(weapon.WalkAnimationParameterName, true);
            }
        }
    }
}