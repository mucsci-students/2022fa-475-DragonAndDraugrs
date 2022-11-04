using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EC = GGSFPSIntegrationTool.Utilities.General.ErrorChecking;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Switch
{
    public class Loader
    {
        // Number of characters to remove from the end of instance's name, usually being "(clone)" hence being 7 by default
        // Prevents animations from disconnecting with object
        public byte CharactersToShortenInstanceNameBy { get; set; } = 7;

        public Transform WeaponSpaceOffsetTransform { get; set; }
        public Transform WeaponSpaceTransform { get; set; }
        public Vector3 OriginalWeaponSpaceOffsetPosition { get; set; }

        public Loader(Transform weaponSpaceOffsetTransform, Transform weaponSpaceTransform)
        {
            WeaponSpaceOffsetTransform = weaponSpaceOffsetTransform;
            WeaponSpaceTransform = weaponSpaceTransform;
            OriginalWeaponSpaceOffsetPosition = WeaponSpaceOffsetTransform.localPosition;
        }

        public void LoadWeapon(
            Scripts.Weapon weapon, 
            ref int burstShotCount,
            ref Animator weaponSpaceAnimator,
            ref GameObject weaponInstance,
            ref Transform barrelFlashSpawn,
            ref Transform projectileSpawn,
            ref Transform ejectedCartridgeSpawn,
            ref AudioSource barrelFlashAudioSource
            )
        {
            DestroyLastWeapon(ref weaponInstance);
            InstantiateWeapon(weapon, WeaponSpaceTransform, ref weaponInstance);
            FindWeaponSpawnPoints(weapon, ref barrelFlashSpawn, ref projectileSpawn, ref ejectedCartridgeSpawn);

            barrelFlashAudioSource = barrelFlashSpawn.GetComponent<AudioSource>();
            barrelFlashAudioSource.clip = weapon.FireAudio;

            burstShotCount = weapon.ShotsPerBurst;

            // Prevents animation disconnection issue (if first weapon controller is pre-applied to animator)
            weaponSpaceAnimator.runtimeAnimatorController = null;
            weaponSpaceAnimator.runtimeAnimatorController = weapon.WeaponAnimatorController;

            // Position offsetting is executed after Weapon Instantiation to prevent issue with apply the offset
            if (weapon.IsPositionOffset)
            {
                WeaponSpaceOffsetTransform.localPosition = OriginalWeaponSpaceOffsetPosition + weapon.PositionOffset;
            }
            else
            {
                WeaponSpaceOffsetTransform.localPosition = OriginalWeaponSpaceOffsetPosition;
            }
        }

        void DestroyLastWeapon(ref GameObject weaponInstance)
        {
            if (weaponInstance != null)
            {
                Object.Destroy(weaponInstance);
            }
        }

        void InstantiateWeapon(
            Scripts.Weapon weapon,
            Transform weaponSpaceTransform,
            ref GameObject weaponInstance
            )
        {
            // Spawn new weapon instance
            if (weapon.WeaponPrefab != null)
            {
                weaponInstance = Object.Instantiate(weapon.WeaponPrefab, weaponSpaceTransform);
                General.ObjectNamer.ShortenName(weaponInstance, CharactersToShortenInstanceNameBy);
            }
        }

        void FindWeaponSpawnPoints(
            Scripts.Weapon weapon,
            ref Transform barrelFlashSpawn,
            ref Transform projectileSpawn,
            ref Transform ejectedCartridgeSpawn
            )
        {
            barrelFlashSpawn = GameObject.Find(weapon.BarrelFlashSpawnDirectory).transform;
            projectileSpawn = GameObject.Find(weapon.ProjectileSpawnDirectory).transform;
            ejectedCartridgeSpawn = GameObject.Find(weapon.CartridgeSpawnDirectory).transform;

            const string BarrelFlashSpawnErrorMessage = "Ensure Barrel Flash Spawn Name field matches GameObject's name.";
            const string ProjectileSpawnErrorMessage = "Ensure Projectile Spawn Name field matches GameObject's name.";
            const string EjectedCartridgeSpawnErrorMessage = "Ensure Ejected Cartridge Spawn Name field matches GameObject's name.";

            EC.ObjectChecker.ThrowIfNull<Component>(barrelFlashSpawn, BarrelFlashSpawnErrorMessage);
            EC.ObjectChecker.ThrowIfNull<Component>(projectileSpawn, ProjectileSpawnErrorMessage);
            EC.ObjectChecker.ThrowIfNull<Component>(ejectedCartridgeSpawn, EjectedCartridgeSpawnErrorMessage);
        }
    }
}
