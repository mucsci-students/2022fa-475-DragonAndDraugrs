using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Movement
{
    public class Manager
    {
        public CharacterBasedInfluencer CharacterBasedInfluencerProperty { get; private set; }
        public MouseBasedInfluencer MouseBasedInfluencerProperty { get; private set; }
        
        Input.Manager InputManager { get; set; }

        public Manager(
            CharacterController characterController,
            Animator animator,
            Input.Manager inputManager
            )
        {
            CharacterBasedInfluencerProperty = new CharacterBasedInfluencer(characterController, animator);
            MouseBasedInfluencerProperty = new MouseBasedInfluencer(animator);
            InputManager = inputManager;
        }

        public void Update(
            Scripts.Weapon weapon,
            float mouseXInfluence,
            float mouseYInfluence
            )
        {
            CharacterBasedInfluencerProperty.Update(
                weapon,
                InputManager
                );

            MouseBasedInfluencerProperty.Update(
                weapon,
                mouseXInfluence,
                mouseYInfluence
                );
        }
    }
}