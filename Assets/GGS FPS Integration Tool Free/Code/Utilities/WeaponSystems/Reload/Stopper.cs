using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Reload
{
    public class Stopper
    {
        public int SemiautoFiringTypeIndex { get; set; } = 0;
        public int AutoFiringTypeIndex { get; set; } = 1;
        public int PartialRepeatReloadingTypeIndex { get; set; } = 2;

        public Reloader ReloaderProperty { get; private set; }
        public Input.Manager InputManager { get; private set; }

        public Stopper(
            Reloader reloader, 
            Input.Manager inputManager
            )
        {
            ReloaderProperty = reloader;
            InputManager = inputManager;
        }

        public void Update(
            Scripts.Weapon weapon,
            int weaponAmmoCount,
            ref int burstShotCount,
            ref float incrementalReloadInterruptionCountDown,
            ref bool isReloading,
            bool[] isConflictingStateFalseArray
            )
        {
            if (
                    isReloading
                    &&
                    (
                        !General.Array.BoolChecker.AreAllOfState(false, isConflictingStateFalseArray)
                        ||
                        (
                            // If the weapon does not have enough ammo to fire again,
                            // reloaded cannot be aborted
                            weaponAmmoCount >= weapon.AmmoLossPerShot
                            &&
                            (
                                ((int)weapon.FiringTypeProperty == SemiautoFiringTypeIndex && InputManager.IsFireDetected)
                                ||
                                ((int)weapon.FiringTypeProperty == AutoFiringTypeIndex && InputManager.IsAutoFireDetected)
                            )
                        )
                    )
                )
            {
                ReloaderProperty.StopReload();

                ReloaderProperty.AudioSourceProperty.Stop();
                ReloaderProperty.AnimatorProperty.SetBool(weapon.ReloadAniamtionParameterName, false);

                burstShotCount = weapon.ShotsPerBurst;
                incrementalReloadInterruptionCountDown = weapon.IncrementalReloadRecoveryTime;
            }
        }
    }
}