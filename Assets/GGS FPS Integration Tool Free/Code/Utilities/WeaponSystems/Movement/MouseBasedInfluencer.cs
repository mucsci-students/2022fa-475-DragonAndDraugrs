using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Movement
{
    public class MouseBasedInfluencer
    {
        // As variables of properties cannot be interacted directly, a member variable is used instead
        Vector2 _CurrentMouseInfluance;

        public float MouseMovementRate { get; set; } = 0.1f;
        public float MouseMovementRounding { get; set; } = 0.001f;

        Animator AnimatorProperty { get; set; }

        public MouseBasedInfluencer(
            Animator animator
            )
        {
            AnimatorProperty = animator;
        }

        public void Update(
            Scripts.Weapon weapon,
            float mouseXInfluence,
            float mouseYInfluence
            )
        {
            // Allows mouse movement influance to adjust gradully, relying on mouse GetAxis alone causes jittering 
            if (
                    _CurrentMouseInfluance.x < mouseXInfluence - MouseMovementRounding 
                    ||
                    _CurrentMouseInfluance.x > mouseXInfluence + MouseMovementRounding
                )
            {
                _CurrentMouseInfluance.x += (mouseXInfluence - _CurrentMouseInfluance.x) * MouseMovementRate;
            }
            else
            {
                _CurrentMouseInfluance.x = mouseXInfluence;
            }

            // Allows mouse movement influance to adjust gradully, relying on mouse GetAxis alone causes jittering 
            if (
                    _CurrentMouseInfluance.y < mouseYInfluence - MouseMovementRounding 
                    ||
                    _CurrentMouseInfluance.y > mouseYInfluence + MouseMovementRounding
                )
            {
                _CurrentMouseInfluance.y += (mouseYInfluence - _CurrentMouseInfluance.y) * MouseMovementRate;
            }
            else
            {
                _CurrentMouseInfluance.y = mouseYInfluence;
            }

            AnimatorProperty.SetFloat(weapon.MouseXAnimationParameterName, _CurrentMouseInfluance.x);
            AnimatorProperty.SetFloat(weapon.MouseYAnimationParameterName, _CurrentMouseInfluance.y);
        }
    }
}