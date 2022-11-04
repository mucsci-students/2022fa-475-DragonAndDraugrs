using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Ammo
{
    public class Manager
    {
        public Dictionary<string, int> TotalAmmoCounts { get; set; }

        List<Scripts.WeaponDetails> WeaponDetailsList { get; set; }
        List<Scripts.Weapon> Weapons { get; set; }
        WeaponRuntimeHandler[] WeaponRuntimeHandlers { get; set; }
        bool CanCallbackBeRead { get; set; }

        public Manager(
            List<Scripts.WeaponDetails> weaponDetailsList,
            WeaponRuntimeHandler[] weaponRuntimeHandlers
            )
        {
            TotalAmmoCounts = new Dictionary<string, int>();

            WeaponDetailsList = weaponDetailsList;

            Weapons = new List<Scripts.Weapon>(
                General.Array.Converter.ApplyMembersToArray(weaponDetailsList.ToArray())
                );

            WeaponRuntimeHandlers = weaponRuntimeHandlers;
        }

        public void ApplyWeaponsAmmoCountOnStart()   // ? as it's also used for reseting, should be renamed?
        {
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (
                        // enableOnStart is included as loadedAtStart can be hidden in the Inspector
                        // when enableOnStart is false
                        WeaponDetailsList[i].IsEnabledOnStart
                        &&
                        WeaponDetailsList[i].IsLoadedOnStart
                    )
                {
                    WeaponRuntimeHandlers[i].WeaponAmmoCount = Weapons[i].Capacity;
                }
            }
        }

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

        public void WeaponAmmoCount(
            Scripts.Weapon weapon,
            int ammoCount,
            Scripts.Ammo weaponAmmo
            )
        {
            ammoCount = Mathf.Clamp(
                ammoCount,
                0, 
                weapon.Capacity
                );

            // If weapon has already been enabled when collected,
            // apply weapon's ammo to total ammo count
            if (WeaponRuntimeHandlers[Weapons.IndexOf(weapon)].IsWeaponEnabled)
            {
                IncreaseTotalAmmoCount(weaponAmmo, ammoCount);
            }
            else
            {
                WeaponRuntimeHandlers[Weapons.IndexOf(weapon)].WeaponAmmoCount = ammoCount;
            }

            CanCallbackBeRead = true;
        }

        public void IncreaseTotalAmmoCount(
            Scripts.Ammo targetAmmo,
            int amountToAdd
            )
        {
            TotalAmmoCounts[targetAmmo.name] += amountToAdd;

            if (TotalAmmoCounts[targetAmmo.name] < 0)
            {
                TotalAmmoCounts[targetAmmo.name] = 0;
            }
        }

        public void SetTotalAmmoCount(
            Scripts.Ammo targetAmmo,
            int amount
            )
        {
            TotalAmmoCounts[targetAmmo.name] = amount;

            if (TotalAmmoCounts[targetAmmo.name] < 0)
            {
                TotalAmmoCounts[targetAmmo.name] = 0;
            }
        }
    }
}