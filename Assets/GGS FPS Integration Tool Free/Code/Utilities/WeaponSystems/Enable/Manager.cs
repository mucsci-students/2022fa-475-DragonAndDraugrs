using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Enable
{
    public class Manager
    {
        Scripts.WeaponCollection WeaponCollectionProperty { get; set; }
        WeaponRuntimeHandler[] WeaponRuntimeHandlers { get; set; }
        bool CanCallbackBeRead { get; set; }

        public Manager(
            //List<Scripts.Weapon> weapons,
            Scripts.WeaponCollection weaponCollection,
            WeaponRuntimeHandler[] weaponRuntimeHandlers
            )
        {
            WeaponCollectionProperty = weaponCollection;
            WeaponRuntimeHandlers = weaponRuntimeHandlers;
        }

        public void EnableWeaponsOnStart()       // ? as it's also used for reseting, should be renamed?      
        {
            // ? should check if both array sizes match first?
            for (int i = 0; i < WeaponRuntimeHandlers.Length; i++)
            {
                WeaponRuntimeHandlers[i].IsWeaponEnabled = WeaponCollectionProperty.WeaponDetailsList[i].IsEnabledOnStart;
            }

            CanCallbackBeRead = true;
        }

        // Used to update properties within this class and used for callbacks
        // Allows function calls (EnableWeapon etc) to omit parameters 
        // which would require private properties
        public void Update(
            ref WeaponRuntimeHandler[] weaponRuntimeHandlers
            )
        {
            if (CanCallbackBeRead)
            {
                weaponRuntimeHandlers = WeaponRuntimeHandlers;

                CanCallbackBeRead = false;
            }
            else
            {
                WeaponRuntimeHandlers = weaponRuntimeHandlers;
            }
        }

        public void EnableWeapon(
            int targetWeaponIndex
            )
        {
            WeaponRuntimeHandlers[targetWeaponIndex].IsWeaponEnabled = true;

            CanCallbackBeRead = true;
        }

            // ? mention in documentation that Weapon SOs cannot have the same name
            // or weapon collections cant have more than one of the same weapon
        public void EnableWeapon(
            Scripts.Weapon targetWeapon
            )
        {
            int matchedWeaponIndex = GetIndexOfWeapon(WeaponCollectionProperty, targetWeapon);

            General.ErrorChecking.NumericChecker.ThrowIfUnderMimimum<Component>(
                matchedWeaponIndex,
                -1,
                "The Weapon named '" + targetWeapon.name + "' cannot be found in WeaponCollection.",
                "Ensure that all weapon that will be used in scene are assigned in the WeaponCollection."
                );


            WeaponRuntimeHandlers[matchedWeaponIndex].IsWeaponEnabled = true;

            CanCallbackBeRead = true;
        }

        public void DisableWeapon(
            int targetWeaponIndex
            )
        {
            WeaponRuntimeHandlers[targetWeaponIndex].IsWeaponEnabled = false;

            CanCallbackBeRead = true;
        }

        public void DisableWeapon(
            Scripts.Weapon targetWeapon
            )
        {
            int matchedWeaponIndex = GetIndexOfWeapon(WeaponCollectionProperty, targetWeapon);

            General.ErrorChecking.NumericChecker.ThrowIfUnderMimimum<Component>(
                matchedWeaponIndex,
                -1,
                "The Weapon named '" + targetWeapon.name + "' cannot be found in WeaponCollection.",
                "Ensure that all weapon that will be used in scene are assigned in the WeaponCollection."
                );

            WeaponRuntimeHandlers[matchedWeaponIndex].IsWeaponEnabled = false;

            CanCallbackBeRead = true;
        }



        int GetIndexOfWeapon(Scripts.WeaponCollection weaponCollection, Scripts.Weapon weapon)
        {
            for (int i = 0; i < weaponCollection.WeaponDetailsList.Count; i++)
            {
                if (weaponCollection.WeaponDetailsList[i].WeaponScriptableObject == weapon)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}