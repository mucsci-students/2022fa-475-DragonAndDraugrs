using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Aim
{
    public class Manager 
    {
        public float CurrentAimingInterpolation { get; private set; } = 0;

        Animator AnimatorProperty { get; set; }
        Input.Manager InputManager { get; set; }

        public Manager(
            Animator animator,
            Input.Manager inputManager
            )
        {
            AnimatorProperty = animator;
            InputManager = inputManager;
        }

        public void Update(
            Scripts.Weapon weapon,
            bool[] isConflictingStateFalseArray
            )
        {
            if (
                    InputManager.IsAimDetected 
                    && 
                    General.Array.BoolChecker.AreAllOfState(false, isConflictingStateFalseArray)
                )
            {
                AnimatorProperty.SetBool(weapon.AimAnimationParameterName, true);
                UpdateAimingInterpolation(true, weapon.AimingTransitionTime);
            }
            else
            {
                AnimatorProperty.SetBool(weapon.AimAnimationParameterName, false);
                UpdateAimingInterpolation(false, weapon.AimingTransitionTime);
            }
        }

        void UpdateAimingInterpolation(
            bool isAiming,
            float weaponAimingTime
            )
        {
            float interpolationChange = (1f / weaponAimingTime) * Time.deltaTime;

            if (isAiming)
            {
                CurrentAimingInterpolation += interpolationChange;
            }
            else
            {
                CurrentAimingInterpolation -= interpolationChange;
            }

            CurrentAimingInterpolation = Mathf.Clamp01(CurrentAimingInterpolation);
        }
    }
}