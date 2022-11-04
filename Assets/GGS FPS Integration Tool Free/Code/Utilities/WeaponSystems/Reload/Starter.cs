using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Reload
{
    public class Starter
    {
        public Reloader ReloaderProperty { get; private set; }

        public Starter(
            Reloader reloader
            )
        {
            ReloaderProperty = reloader;
        }

        public void Update(
            Scripts.Weapon weapon,
            bool isReloadInputDetected,
            int weaponAmmoCount,
            int totalAmmoCount,
            bool isReloading,
            bool[] isConflictingStateTrueArray
            )
        {
            if (
                    (
                        weaponAmmoCount < weapon.AmmoLossPerShot
                        ||
                        (weaponAmmoCount < weapon.Capacity && isReloadInputDetected)
                    )
                    &&
                    totalAmmoCount > 0
                    &&
                    !isReloading
                    &&
                    General.Array.BoolChecker.AreAllOfState(false, isConflictingStateTrueArray)
                )
            {
                ReloaderProperty.StartReload(weapon);
            }
        }
    }
}