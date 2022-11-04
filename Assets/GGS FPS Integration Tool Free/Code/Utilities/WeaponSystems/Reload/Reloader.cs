using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Reload
{
    public class Reloader
    {
        public int PartialReloadingTypeIndex { get; set; } = 1;
        public int PartialRepeatReloadingTypeIndex { get; set; } = 2;

        public Animator AnimatorProperty { get; private set; }
        public AudioSource AudioSourceProperty { get; private set; }

        public bool IsReloading { get; private set; } = false;

        Scripts.Weapon CurrentWeapon { get; set; }
        General.Time.DownCounter ReloadDurationCountDown { get; set; } = new General.Time.DownCounter();

        public Reloader(
            Animator animator, 
            AudioSource audioSource)
        {
            AnimatorProperty = animator;
            AudioSourceProperty = audioSource;
        }

        public void Update(
            ref bool isReloading,
            ref int weaponAmmoCount,
            ref int totalAmmoCount,
            ref int burstShotCount,
            ref float incrementalReloadInterruptionCountDown
            )
        {
            if (IsReloading)
            {
                if ((int)CurrentWeapon.ReloadingTypeProperty != PartialRepeatReloadingTypeIndex)
                {
                    if (!ReloadDurationCountDown.HasStarted)
                    {
                        AudioSourceProperty.clip = CurrentWeapon.ReloadAudio;
                        AudioSourceProperty.Play();
                        AnimatorProperty.SetBool(CurrentWeapon.ReloadAniamtionParameterName, true);

                        ReloadDurationCountDown.Start(CurrentWeapon.ReloadingTime);
                    }

                    if (ReloadDurationCountDown.HasEnded)
                    {
                        AnimatorProperty.SetBool(CurrentWeapon.ReloadAniamtionParameterName, false);

                        AmmoHandler.ReloadWeapon(
                            ref weaponAmmoCount,
                            ref totalAmmoCount,
                            (int)CurrentWeapon.ReloadingTypeProperty,
                            CurrentWeapon.Capacity,
                            CurrentWeapon.AmmoAddedPerReload
                            );

                        burstShotCount = CurrentWeapon.ShotsPerBurst;

                        // Prevents firing when weapon is transitioning from reloading to idle animations
                        if ((int)CurrentWeapon.ReloadingTypeProperty == PartialReloadingTypeIndex)
                        {
                            incrementalReloadInterruptionCountDown = CurrentWeapon.IncrementalReloadRecoveryTime;
                        }

                        IsReloading = false;
                    }
                }
                else
                {
                    
                    if (!AnimatorProperty.GetBool(CurrentWeapon.ReloadAniamtionParameterName))
                    {
                        AudioSourceProperty.clip = CurrentWeapon.ReloadAudio;

                        AnimatorProperty.SetBool(CurrentWeapon.ReloadAniamtionParameterName, true);
                    }

                    if (weaponAmmoCount < CurrentWeapon.Capacity && totalAmmoCount > 0)
                    {
                        if (!ReloadDurationCountDown.HasStarted)
                        {
                            AudioSourceProperty.Play();

                            ReloadDurationCountDown.Start(CurrentWeapon.ReloadingTime);
                        }

                        if (ReloadDurationCountDown.HasEnded)
                        {
                            AmmoHandler.ReloadWeapon(
                                ref weaponAmmoCount,
                                ref totalAmmoCount,
                                (int)CurrentWeapon.ReloadingTypeProperty,
                                CurrentWeapon.Capacity,
                                CurrentWeapon.AmmoAddedPerReload
                                );
                        }
                    }
                    else
                    {
                        AnimatorProperty.SetBool(CurrentWeapon.ReloadAniamtionParameterName, false);

                        burstShotCount = CurrentWeapon.ShotsPerBurst;

                        // Prevents firing when weapon is transitioning from reloading to idle animations
                        incrementalReloadInterruptionCountDown = CurrentWeapon.IncrementalReloadRecoveryTime;

                        IsReloading = false;
                    }
                }
            }

            ReloadDurationCountDown.Update();

            isReloading = IsReloading;
        }

        public void StartReload(Scripts.Weapon weapon)
        {
            IsReloading = true;

            General.ErrorChecking.ObjectChecker.ThrowIfNull<Component>(weapon.name, weapon);
            CurrentWeapon = weapon;
        }

        public void StopReload()
        {
            IsReloading = false;
            ReloadDurationCountDown.End();
        }
    }
}