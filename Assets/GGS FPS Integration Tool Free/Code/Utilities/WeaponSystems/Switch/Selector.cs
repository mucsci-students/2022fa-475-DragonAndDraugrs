using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Switch
{
    public class Selector
    {
        public int CurrentIndex { get; private set; } = 0;

        List<Scripts.Weapon> Weapons { get; set; }
        WeaponRuntimeHandler[] WeaponRuntimeHandlers { get; set; }
        Input.Manager InputManager { get; set; }

        public Selector(
            List<Scripts.Weapon> weapons,
            WeaponRuntimeHandler[] weaponRuntimeHandlers,
            Input.Manager inputManager
            )
        {
            Weapons = weapons;
            WeaponRuntimeHandlers = weaponRuntimeHandlers;
            InputManager = inputManager;
        }

        public int StartWeaponIndex()
        {
            // Only the error throwing (for when no weapons are enabled on start) is needed
            // Hence bool return is not used
            IsAnotherWeaponsEnabled();

            while (!WeaponRuntimeHandlers[CurrentIndex].IsWeaponEnabled)
            {
                CurrentIndex++;

                if (CurrentIndex >= Weapons.Count)
                {
                    CurrentIndex = 0;
                }
            }

            return CurrentIndex;
        }

        public void Update(
            ref Scripts.Weapon nextWeapon,
            ref bool isSwitching,
            bool isCurrentWeaponDisabled
            )
        {
            if (isSwitching)
            {
                return;
            }

            if (
                    (
                        InputManager.IsSwitchDetected
                        &&
                        IsAnotherWeaponsEnabled()
                    )
                    ||
                    isCurrentWeaponDisabled
                )
            {
                nextWeapon = Weapons[NextWeaponIndex()];
                isSwitching = true;
            }
            else if (!General.Array.BoolChecker.AreAllOfState(false, InputManager.IsSelectionSwitchDetectedArray))
            {
                int indexToCheck = NextSelectedWeaponIndex();

                // NextSelectWeaponIndex can return -1 if selection is not possible
                if (indexToCheck > -1)
                {
                    nextWeapon = Weapons[indexToCheck];
                    isSwitching = true;
                }
            }
        }

        int NextWeaponIndex()
        {
            // Only the error throwing (for when no weapons are enabled on start) is needed
            // Hence bool return is not used
            IsAnotherWeaponsEnabled();

            do
            {
                CurrentIndex++;

                if (CurrentIndex >= Weapons.Count)
                {
                    CurrentIndex = 0;
                }
            }
            while (!WeaponRuntimeHandlers[CurrentIndex].IsWeaponEnabled);

            return CurrentIndex;
        }

        int NextSelectedWeaponIndex()
        {
            // Find Selected Switch button that was pressed
            for (int i = 0; i < InputManager.IsSelectionSwitchDetectedArray.Length; i++)
            {
                // Ensure index does not exceed WeaponCollection lenght
                if (i >= WeaponRuntimeHandlers.Length)
                {
                    break;
                }

                if (InputManager.IsSelectionSwitchDetectedArray[i])
                {
                    // Ensure selected weapon is enabled & not the same as current
                    if (
                            WeaponRuntimeHandler.GetIsWeaponEnabledArray(WeaponRuntimeHandlers)[i]
                            &&
                            CurrentIndex != i
                        )
                    {
                        return CurrentIndex = i;
                    }

                    break;
                }
            }

            return -1;
        }

        bool IsAnotherWeaponsEnabled()
        {
            int weaponsEnabledCount = 
                General.Array.BoolChecker.NumberOfStates(true, WeaponRuntimeHandler.GetIsWeaponEnabledArray(WeaponRuntimeHandlers));

            if (weaponsEnabledCount > 1)
            {
                return true;
            }
            else if (weaponsEnabledCount <= 0)
            {
                General.Console.Logger.ThrowException<Component>(
                    "At least one weapon must be enabled.",
                    "Check the WeaponCollection being used and if weapons are being disabled when collecting weapon collectables."
                    );
            }

            return false;
        }
    }
}