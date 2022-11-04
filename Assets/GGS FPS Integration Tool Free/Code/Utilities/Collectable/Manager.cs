using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.Collectable
{
    /// <summary>
    /// Manages functionality for Collectable script.
    /// </summary>
    public class Manager
    {
        public byte TargetWeaponCollectionTypeIndex { get; private set; } = 0;
        public byte TargetAmmoCollectionTypeIndex { get; private set; } = 1;
        public byte TargetHealthCollectionTypeIndex { get; private set; } = 2;
        public byte TargetLivesCollectionTypeIndex { get; private set; } = 3;

        byte CollectionTypeIndex { get; set; }
        GameObject CollectableGameObject { get; set; }
        Scripts.Weapon WeaponProperty { get; set; }
        Scripts.Ammo AmmoProperty { get; set; }
        bool WillEnable { get; set; }
        int WeaponAmmoCount { get; set; }
        int TotalAmmoCountToAdd { get; set; }
        float HealthToAdd { get; set; }
        short LivesToAdd { get; set; }
        byte DespawnTypeIndex { get; set; }
        GameObject AfterCollectionObject { get; set; }
        float AfterCollectionObjectLiftTime { get; set; }

        public Manager(
            byte collectionTypeIndex,
            GameObject collectableGameObject,
            Scripts.Weapon weapon,
            Scripts.Ammo ammo,
            bool willEnable,
            int weaponAmmoCount,
            int totalAmmoCountToAdd,
            float healthToAdd,
            short livesToAdd,
            byte despawnTypeIndex,
            GameObject afterCollectionObject,
            float afterCollectionObjectLiftTime
            )
        {
            CollectionTypeIndex = collectionTypeIndex;
            CollectableGameObject = collectableGameObject;
            WeaponProperty = weapon;
            AmmoProperty = ammo;
            WillEnable = willEnable;
            WeaponAmmoCount = weaponAmmoCount;
            TotalAmmoCountToAdd = totalAmmoCountToAdd;
            HealthToAdd = healthToAdd;
            LivesToAdd = livesToAdd;
            DespawnTypeIndex = despawnTypeIndex;
            AfterCollectionObject = afterCollectionObject;
            AfterCollectionObjectLiftTime = afterCollectionObjectLiftTime;
        }

        /// <summary>
        /// Checks for a colliding player GameObject (that contains a WeaponSystems component) to process collection process.
        /// </summary>
        /// <param name="enteringCollider">Entering Collider GameObject to check.</param>
        public void DetectColliderEntry(Collider enteringCollider)
        {
            Scripts.WeaponSystems currentWeaponSpace;
            Scripts.Health currentHealthComponent;

            if (CollectionTypeIndex == TargetWeaponCollectionTypeIndex)
            {
                currentWeaponSpace = enteringCollider.GetComponentInChildren<Scripts.WeaponSystems>();
                if (currentWeaponSpace == null)
                {
                    return;
                }

                currentWeaponSpace.WeaponSpaceManager.AmmoManager.WeaponAmmoCount(
                    WeaponProperty,
                    WeaponAmmoCount,
                    WeaponProperty.AmmoProperty
                    );

                currentWeaponSpace.WeaponSpaceManager.AmmoManager.IncreaseTotalAmmoCount(
                    WeaponProperty.AmmoProperty,
                    TotalAmmoCountToAdd
                    );

                // Enabling/disabling weapons after asigning ammo prevents issue with
                // how ammo is applied to weapon or total ammo counts
                if (WillEnable)
                {
                    currentWeaponSpace.WeaponSpaceManager.EnableManager.EnableWeapon(WeaponProperty);
                }
                else
                {
                    currentWeaponSpace.WeaponSpaceManager.EnableManager.DisableWeapon(WeaponProperty);
                }
            }
            else if (CollectionTypeIndex == TargetAmmoCollectionTypeIndex)
            {
                currentWeaponSpace = enteringCollider.GetComponentInChildren<Scripts.WeaponSystems>();
                if (currentWeaponSpace == null)
                {
                    return;
                }

                currentWeaponSpace.WeaponSpaceManager.AmmoManager.IncreaseTotalAmmoCount(
                    AmmoProperty,
                    TotalAmmoCountToAdd
                    );
            }
            else if (CollectionTypeIndex == TargetHealthCollectionTypeIndex)
            {
                currentHealthComponent = enteringCollider.GetComponent<Scripts.Health>();
                if (currentHealthComponent == null)
                {
                    return;
                }

                currentHealthComponent.AddHealth(HealthToAdd);
            }
            else if (CollectionTypeIndex == TargetLivesCollectionTypeIndex)
            {
                currentHealthComponent = enteringCollider.GetComponent<Scripts.Health>();
                if (currentHealthComponent == null)
                {
                    return;
                }

                currentHealthComponent.AddLives(LivesToAdd);
            }

            if (AfterCollectionObject != null)
            {
                GameObject afterObjectInstance = Object.Instantiate(
                    AfterCollectionObject, 
                    CollectableGameObject.transform.position, 
                    CollectableGameObject.transform.rotation
                    );

                Object.Destroy(afterObjectInstance, AfterCollectionObjectLiftTime);
            }

            if (DespawnTypeIndex == 0)
            {
                CollectableGameObject.SetActive(false);
            }
            else
            {
                Object.Destroy(CollectableGameObject);
            }
        }
    }
}

