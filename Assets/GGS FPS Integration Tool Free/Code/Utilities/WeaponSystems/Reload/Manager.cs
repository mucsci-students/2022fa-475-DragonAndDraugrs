using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Reload
{
    public class Manager
    {
        bool _IsReloading = false;
        float _IncrementalReloadInterruptionCountDown;

        public bool IsReloading
        {
            get => _IsReloading;
            private set => _IsReloading = value;
        }
        public float IncrementalReloadInterruptionCountDown
        {
            get => _IncrementalReloadInterruptionCountDown;
            private set => _IncrementalReloadInterruptionCountDown = value;
        }

        public Starter StarterProperty { get; private set; }
        public Stopper StopperProperty { get; private set; }
        public Reloader ReloaderProperty { get; private set; }

        public Manager(
            Input.Manager inputManager,
            Animator animator,
            AudioSource audioSource
            )
        {
            ReloaderProperty = new Reloader(
                animator,
                audioSource
                );

            StarterProperty = new Starter(
                ReloaderProperty
                );

            StopperProperty = new Stopper(
                ReloaderProperty, 
                inputManager
                );
        }

        public void Update(
            Scripts.Weapon weapon,
            ref int weaponAmmoCount,
            ref int burstShotCount,
            bool isReloadInputDetected,
            ref Dictionary<string, int> totalAmmoCounts,
            bool[] isConflictingStateTrueArray
            )
        {
            General.Time.DownCounter.ManualDeltaTimeDecrement(ref _IncrementalReloadInterruptionCountDown);

            StopperProperty.Update(
                weapon,
                weaponAmmoCount,
                ref burstShotCount,
                ref _IncrementalReloadInterruptionCountDown,
                ref _IsReloading,
                isConflictingStateTrueArray
                );

            StarterProperty.Update(
                weapon,
                isReloadInputDetected,
                weaponAmmoCount,
                totalAmmoCounts[weapon.AmmoProperty.name],
                IsReloading,
                isConflictingStateTrueArray
                );

            // As indexers are not allowed in ref parameters
            int refTotalAmmoCount = totalAmmoCounts[weapon.AmmoProperty.name];

            ReloaderProperty.Update(
                ref _IsReloading,
                ref weaponAmmoCount,
                ref refTotalAmmoCount,
                ref burstShotCount,
                ref _IncrementalReloadInterruptionCountDown
                );

            totalAmmoCounts[weapon.AmmoProperty.name] = refTotalAmmoCount;
        }
    }
}
