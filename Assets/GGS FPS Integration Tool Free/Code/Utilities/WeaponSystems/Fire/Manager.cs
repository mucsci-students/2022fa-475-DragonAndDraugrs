using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Fire
{
    public class Manager
    {
        public int BurstShotCount { get; set; }

        public Authoriser AuthoriserProperty { get; private set; }
        public FireHandler FireManagerProperty { get; private set; }
        public Visual.Manager VisualManager { get; private set; }
        public Input.Manager InputManager { get; private set; }

        
        public Manager(
            Animator animator,
            Input.Manager inputManager,
            GameObject bloodSplatterImpact,
            LayerMask raycastIgnorableLayers
            )
        {
            InputManager = inputManager;

            AuthoriserProperty = new Authoriser();

            FireManagerProperty = new FireHandler(
                bloodSplatterImpact,
                raycastIgnorableLayers
                );

            VisualManager = new Visual.Manager(animator);
        }

        public void Update(
            Scripts.Weapon weapon,
            int selectedWeaponIndex,
            ref WeaponRuntimeHandler[] weaponRuntimeHandlers,
            ref int burstShotCount,
            bool[] isConflictingStateFalseArray,
            float[] conflictingCountDowns,
            float aimingInterpolationValue,
            bool isPlayerMoving,
            Transform raySpawn,
            Transform barrelFlashSpawn,
            Transform projectileSpawn,
            Transform cartridgeSpawn,
            AudioSource barrelFlashAudioSource,
            Vector3 forwardVector,
            Vector3 playerVelocity
            )
        {
            NextShotCountDownHandler.Update(ref weaponRuntimeHandlers);

            VisualManager.Update(weapon.FireAnimationParameterName);

            if (
                !
                AuthoriserProperty.IsFireAuthorised(
                    (int)weapon.FiringTypeProperty,
                    InputManager.IsFireDetected,
                    InputManager.IsAutoFireDetected,
                    burstShotCount,
                    weapon.ShotsPerBurst,
                    weaponRuntimeHandlers[selectedWeaponIndex].NextShotCountDown,
                    weaponRuntimeHandlers[selectedWeaponIndex].WeaponAmmoCount,
                    weapon.AmmoLossPerShot,
                    isConflictingStateFalseArray,
                    conflictingCountDowns
                    )
                )
            {
                return;
            }

            if (weapon.OutputTypeProperty == 0)
            {
                FireManagerProperty.FireWeapon(
                    weapon,
                    aimingInterpolationValue,
                    isPlayerMoving,
                    raySpawn
                    );
            }
            else
            {
                FireManagerProperty.FireWeapon(
                    weapon,
                    aimingInterpolationValue,
                    isPlayerMoving,
                    projectileSpawn,
                    forwardVector
                    );
            }

            barrelFlashAudioSource.Play();

            VisualManager.CreateVisuals(
                weapon.FireAnimationParameterName,
                weapon.BarrelFlashPrefab,
                barrelFlashSpawn,
                weapon.EjectedCartridgeProperty.CartridgePrefab,
                cartridgeSpawn,
                weapon.EjectedCartridgeProperty.EjectionTrajectory,
                weapon.EjectedCartridgeProperty.EjectionForce,
                playerVelocity
                );

            ManageBurstShots(
                ref burstShotCount, 
                weapon.ShotsPerBurst
                );
            AfterCalculation(
                ref weaponRuntimeHandlers[selectedWeaponIndex].NextShotCountDown, 
                weapon.ShotsPerSecond, 
                ref weaponRuntimeHandlers[selectedWeaponIndex].WeaponAmmoCount, 
                weapon.AmmoLossPerShot
                );
        }

        void ManageBurstShots(
            ref int burstShotCount, 
            int shotsPerBurst
            )
        {
            burstShotCount--;

            if (burstShotCount <= 0)
            {
                burstShotCount = shotsPerBurst;
            }
        }

        void AfterCalculation(
            ref float nextShotCountDown, 
            float weaponFireRate, 
            ref int weaponAmmoCount, 
            int ammoLossPerShot
            )
        {
            nextShotCountDown = 1f / weaponFireRate;
            weaponAmmoCount -= ammoLossPerShot;
        }
    }
}