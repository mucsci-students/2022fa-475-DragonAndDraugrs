using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems
{
    /// <summary>
    /// Manages functionality for WeaponSystems script.
    /// </summary>
    public class Manager
    {
        Animator _WeaponSpaceAnimator;

        int _SelectedWeaponIndex;
        WeaponRuntimeHandler[] _WeaponRuntimeHandlers;

        Transform _ProjectileSpawn;
        Transform _BarrelFlashSpawn;
        Transform _EjectedCartridgeSpawn;

        AudioSource _BarrelFlashAudioSource;

        GameObject _InstantiatedWeaponObject;

        CharacterController CharacterControllerProperty { get; set; }
        Scripts.WeaponCollection WeaponCollectionProperty { get; set; }
        
        Animator WeaponSpaceAnimator
        {
            get => _WeaponSpaceAnimator;
            set => _WeaponSpaceAnimator = value;
        }
        AudioSource BarrelFlashAudioSource
        {
            get => _BarrelFlashAudioSource;
            set => _BarrelFlashAudioSource = value;
        }

        Transform CameraRaySpawn { get; set; }
        Transform WeaponSpaceOffsetTransform { get; set; }
        Transform WeaponSpaceTransform { get; set; }
        
        public int SelectedWeaponIndex
        {
            get => _SelectedWeaponIndex;
            private set => _SelectedWeaponIndex = value;
        }
        public WeaponRuntimeHandler[] WeaponRuntimeHandlers
        {
            get => _WeaponRuntimeHandlers;
            private set => _WeaponRuntimeHandlers = value;
        }
        public List<Scripts.Weapon> Weapons { get; private set; }
        public Scripts.Weapon SelectedWeapon { get; private set; }

        Transform ProjectileSpawn
        {
            get => _ProjectileSpawn;
            set => _ProjectileSpawn = value;
        }
        Transform BarrelFlashSpawn
        {
            get => _BarrelFlashSpawn;
            set => _BarrelFlashSpawn = value;
        }
        Transform EjectedCartridgeSpawn
        {
            get => _EjectedCartridgeSpawn;
            set => _EjectedCartridgeSpawn = value;
        }

        string MouseXInfluenceAxisName { get; set; }
        string MouseYInfluenceAxisName { get; set; }

        GameObject InstantiatedWeaponObject
        {
            get => _InstantiatedWeaponObject;
            set => _InstantiatedWeaponObject = value;
        }

        Fire.Manager FireManager { get; set; }
        Reload.Manager ReloadManager { get; set; }
        Switch.Manager SwitchManager { get; set; }
        public Aim.Manager AimManager { get; set; }
        Input.Manager InputManager { get; set; }
        Movement.Manager MovementManager { get; set; }
        public Enable.Manager EnableManager { get; set; }
        public Ammo.Manager AmmoManager { get; set; }

        public Manager(
            CharacterController characterController,
            Scripts.WeaponCollection weaponCollection,
            Animator weaponSpaceAnimator,
            AudioSource weaponSpaceAudioSource,
            Transform weaponSpaceOffsetTransform,
            Transform weaponSpaceTransform,
            Transform cameraRaySpawn,
            string mouseXInfluenceName,
            string mouseYInfluenceName,

            string fireButtonName,
            string autoFireButtonName,
            string reloadButtonName,
            string switchButtonName,
            string aimButtonName,
            string runButtonName,
            string[] selectionSwitchButtonNames,

            string movementYAxisName,

            GameObject bloodSplatterImpact,
            LayerMask raycastIgnorableLayers
            )
        {
            CharacterControllerProperty = characterController;
            WeaponCollectionProperty = weaponCollection;
            WeaponSpaceAnimator = weaponSpaceAnimator;
            WeaponSpaceOffsetTransform = weaponSpaceOffsetTransform;
            WeaponSpaceTransform = weaponSpaceTransform;
            CameraRaySpawn = cameraRaySpawn;
            MouseXInfluenceAxisName = mouseXInfluenceName;
            MouseYInfluenceAxisName = mouseYInfluenceName;

            Weapons = new List<Scripts.Weapon>(
                General.Array.Converter.ApplyMembersToArray(WeaponCollectionProperty.WeaponDetailsList.ToArray())
                );

            WeaponRuntimeHandlers = new WeaponRuntimeHandler[Weapons.Count];
            WeaponRuntimeHandler.InitialiseArray(ref _WeaponRuntimeHandlers);

            AmmoManager = new Ammo.Manager(WeaponCollectionProperty.WeaponDetailsList, WeaponRuntimeHandlers);

            for (int i = 0; i < Weapons.Count; i++)
            {
                if (
                        !AmmoManager.TotalAmmoCounts.ContainsKey(Weapons[i].AmmoProperty.name)
                    )
                {
                    AmmoManager.TotalAmmoCounts.Add(
                        Weapons[i].AmmoProperty.name,
                        Weapons[i].AmmoProperty.StartAmount
                        );
                }
            }

            InputManager = new Input.Manager(
                fireButtonName,
                autoFireButtonName,
                reloadButtonName,
                switchButtonName,
                aimButtonName,
                runButtonName,
                selectionSwitchButtonNames,
                movementYAxisName
                );

            FireManager = new Fire.Manager(
                WeaponSpaceAnimator,
                InputManager,
                bloodSplatterImpact,
                raycastIgnorableLayers
                );

            ReloadManager = new Reload.Manager(
                InputManager,
                WeaponSpaceAnimator,
                weaponSpaceAudioSource
                );

            EnableManager = new Enable.Manager(
                WeaponCollectionProperty,
                WeaponRuntimeHandlers
                );

            SwitchManager = new Switch.Manager(
                Weapons,
                WeaponRuntimeHandlers,
                WeaponSpaceAnimator,
                weaponSpaceAudioSource,
                InputManager,
                WeaponSpaceOffsetTransform,
                WeaponSpaceTransform
                );

            AimManager = new Aim.Manager(
                WeaponSpaceAnimator, 
                InputManager
                );

            MovementManager = new Movement.Manager(
                characterController,
                WeaponSpaceAnimator,
                InputManager
                );

            // Removes all child object of FPSWeaponSystems (that were used in EditMode)
            // Executed in contuctor to destroy EditMode Weapon GameObjects before creating one for PlayMode
            for (int i = 0; i < WeaponSpaceTransform.childCount; i++)
            {
                // Standard Destory takes too long to destroy GameObjects,
                // thus causing conflicts & errors with Play & Edit Mode Weapon GameObjects
                Object.DestroyImmediate(WeaponSpaceTransform.GetChild(i).gameObject);
            }
        }

        public void Start()
        {
            EnableManager.EnableWeaponsOnStart();
            AmmoManager.ApplyWeaponsAmmoCountOnStart();

            // These variables are used because Properties cannot be applied in ref parameters 
            int refBurstShotCount = FireManager.BurstShotCount;

            SwitchManager.Start(
                Weapons,
                ref refBurstShotCount,
                ref _InstantiatedWeaponObject,
                ref _BarrelFlashSpawn,
                ref _ProjectileSpawn,
                ref _EjectedCartridgeSpawn,
                ref _BarrelFlashAudioSource,
                ref _WeaponSpaceAnimator,
                ref _SelectedWeaponIndex
                );

            FireManager.BurstShotCount = refBurstShotCount;
        }

        public void Update()
        {
            SelectedWeapon = Weapons[SelectedWeaponIndex];

            UpdateInput();

            UpdateFire();
            UpdateReload();
            UpdateSwitch();

            UpdateAim();
            UpdateMovement();

            UpdateEnable();
            UpdateAmmo();
        }

        void UpdateFire()
        {
            bool[] ConflictingStates = {
                ReloadManager.IsReloading,
                SwitchManager.IsSwitching,
                MovementManager.CharacterBasedInfluencerProperty.IsRunning
            };
            float[] ConflictingCountDowns = {
                MovementManager.CharacterBasedInfluencerProperty.RunningRecoveryCountDown,
                ReloadManager.IncrementalReloadInterruptionCountDown
            };

            int refBurstShotCount = FireManager.BurstShotCount;

            FireManager.Update(
                SelectedWeapon,
                SelectedWeaponIndex,
                ref _WeaponRuntimeHandlers,
                ref refBurstShotCount,
                ConflictingStates, 
                ConflictingCountDowns,
                AimManager.CurrentAimingInterpolation,
                MovementManager.CharacterBasedInfluencerProperty.IsWalking,
                CameraRaySpawn,
                BarrelFlashSpawn,
                ProjectileSpawn,
                EjectedCartridgeSpawn,
                BarrelFlashAudioSource,
                WeaponSpaceTransform.forward,
                CharacterControllerProperty.velocity
                );

            FireManager.BurstShotCount = refBurstShotCount;
        }

        void UpdateReload()
        {
            Dictionary<string, int> totalAmmoCounts = AmmoManager.TotalAmmoCounts;

            bool[] conflictingStates = { 
                SwitchManager.IsSwitching,
                MovementManager.CharacterBasedInfluencerProperty.IsRunning 
            };

            int refBurstShotCount = FireManager.BurstShotCount;

            ReloadManager.Update(
                SelectedWeapon,
                ref _WeaponRuntimeHandlers[SelectedWeaponIndex].WeaponAmmoCount,
                ref refBurstShotCount,
                InputManager.IsReloadDetected,
                ref totalAmmoCounts,
                conflictingStates
                );

            FireManager.BurstShotCount = refBurstShotCount;

            AmmoManager.TotalAmmoCounts = totalAmmoCounts;
        }

        void UpdateSwitch()
        {
            int refBurstShotCount = FireManager.BurstShotCount;

            SwitchManager.Update(
               Weapons,
               _WeaponRuntimeHandlers,
               ref refBurstShotCount,
               ref _SelectedWeaponIndex,
               ref _InstantiatedWeaponObject,
               ref _BarrelFlashSpawn,
               ref _ProjectileSpawn,
               ref _EjectedCartridgeSpawn,
               ref _BarrelFlashAudioSource,
               ref _WeaponSpaceAnimator
               );

            FireManager.BurstShotCount = refBurstShotCount;
        }

        void UpdateAim()
        {
            bool[] isFalseArray = { 
                ReloadManager.IsReloading,
                SwitchManager.IsSwitching, 
                MovementManager.CharacterBasedInfluencerProperty.IsRunning 
            };
            
            AimManager.Update(
                SelectedWeapon,
                isFalseArray
                );
        }

        void UpdateInput()
        {
            InputManager.Update();
        }

        void UpdateMovement()
        {
            MovementManager.Update(
                SelectedWeapon,
                // Input.GetAxis() is used here as sensitity does not work outside MonoBehaviour scripts   
                UnityEngine.Input.GetAxis(MouseXInfluenceAxisName),
                UnityEngine.Input.GetAxis(MouseYInfluenceAxisName)
                );
        }

        void UpdateEnable()
        {
            EnableManager.Update(ref _WeaponRuntimeHandlers);
        }

        void UpdateAmmo()
        {
            AmmoManager.Update(ref _WeaponRuntimeHandlers);
        }
    }
}