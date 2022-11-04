using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Switch
{
    public class Manager
    {
        bool _IsSwitching = false;
        bool _IsCurrentWeaponDisabled = false;
        Scripts.Weapon _CurrentWeapon;
        Scripts.Weapon _NextWeapon;

        public bool IsSwitching
        {
            get => _IsSwitching;
            set => _IsSwitching = value;
        }
        bool IsCurrentWeaponDisabled
        {
            get => _IsCurrentWeaponDisabled;
            set => _IsCurrentWeaponDisabled = value;
        }
        Scripts.Weapon CurrentWeapon 
        {
            get => _CurrentWeapon;
            set => _CurrentWeapon = value;
        }
        Scripts.Weapon NextWeapon
        {
            get => _NextWeapon;
            set => _NextWeapon = value;
        }

        Selector SelectorProperty { get; set; }
        Switcher SwitcherProperty { get; set; }
        Loader LoaderProperty { get; set; }
        

        public Manager(
            List<Scripts.Weapon> weapons,
            WeaponRuntimeHandler[] weaponRuntimeHandlers,
            Animator animator,
            AudioSource audioSource,
            Input.Manager inputManager,
            Transform weaponSpaceOffsetTransform,
            Transform weaponSpaceTransform
            )
        {
            SelectorProperty = new Selector(weapons, weaponRuntimeHandlers, inputManager);
            SwitcherProperty = new Switcher(animator, audioSource, weaponSpaceOffsetTransform, weaponSpaceTransform);
            LoaderProperty = new Loader(weaponSpaceOffsetTransform, weaponSpaceTransform);
        }

        public void Start(
            List<Scripts.Weapon> weapons,
            ref int burstShotCount,
            ref GameObject weaponInstance,
            ref Transform barrelFlashSpawn,
            ref Transform projectileSpawn,
            ref Transform ejectedCartridgeSpawn,
            ref AudioSource barrelFlashAudioSource,
            ref Animator weaponAnimator,
            ref int currentWeaponIndex
            )
        {
            currentWeaponIndex = SelectorProperty.StartWeaponIndex();

            CurrentWeapon = weapons[currentWeaponIndex];

            LoaderProperty.LoadWeapon(
                CurrentWeapon, 
                ref burstShotCount,
                ref weaponAnimator,
                ref weaponInstance,
                ref barrelFlashSpawn,
                ref projectileSpawn,
                ref ejectedCartridgeSpawn,
                ref barrelFlashAudioSource
                );
        }

        public void Update(
            List<Scripts.Weapon> weapons,
            WeaponRuntimeHandler[] weaponRuntimeHandlers,
            ref int burstShotCount,
            ref int selectedWeaponIndex,
            ref GameObject weaponInstance,
            ref Transform barrelFlashSpawn,
            ref Transform projectileSpawn,
            ref Transform ejectedCartridgeSpawn,
            ref AudioSource barrelFlashAudioSource,
            ref Animator weaponAnimator
            )
        {
            if (!weaponRuntimeHandlers[selectedWeaponIndex].IsWeaponEnabled)
            {
                IsCurrentWeaponDisabled = true;
            }

            SelectorProperty.Update(
                ref _NextWeapon,
                ref _IsSwitching,
                IsCurrentWeaponDisabled
                );


            SwitcherProperty.Update(
                ref _CurrentWeapon, 
                NextWeapon, 
                ref _IsSwitching, 
                ref _IsCurrentWeaponDisabled, 
                ref burstShotCount,
                ref weaponInstance,
                ref barrelFlashSpawn,
                ref projectileSpawn,
                ref ejectedCartridgeSpawn,
                ref barrelFlashAudioSource,
                ref weaponAnimator
                );

            selectedWeaponIndex = General.Array.Converter.ElementToIndex(CurrentWeapon, weapons.ToArray());
        }

        

        

    }
}