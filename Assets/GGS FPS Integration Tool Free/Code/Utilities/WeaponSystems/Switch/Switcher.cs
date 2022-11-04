using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Switch
{
    public class Switcher
    {
        Animator AnimatorProperty { get; set; }
        AudioSource AudioSourceProperty { get; set; }
        Loader LoaderProperty { get; set; }
        General.Time.DownCounter SwitchOutCountDown { get; set; }
        General.Time.DownCounter SwitchInCountDown { get; set; }

        public Switcher(
            Animator animator, 
            AudioSource audioSource, 
            Transform weaponSpaceOffsetTransform, 
            Transform weaponSpaceTransform
            )
        {
            AnimatorProperty = animator;
            AudioSourceProperty = audioSource;

            LoaderProperty = new Loader(weaponSpaceOffsetTransform, weaponSpaceTransform);
            SwitchOutCountDown = new General.Time.DownCounter();
            SwitchInCountDown = new General.Time.DownCounter();
        }

        public void Update(
            ref Scripts.Weapon currentWeapon, 
            Scripts.Weapon nextWeapon,
            ref bool isSwitching,
            ref bool isCurrentWeaponDisabled,
            ref int burstShotCount,
            ref GameObject weaponInstance,
            ref Transform barrelFlashSpawn,
            ref Transform projectileSpawn,
            ref Transform ejectedCartridgeSpawn,
            ref AudioSource barrelFlashAudioSource,
            ref Animator weaponAnimator
            )
        {
            if (!isSwitching) return;

            SwitchOutCountDown.Update();
            SwitchInCountDown.Update();

            if (!SwitchOutCountDown.HasStarted && !SwitchInCountDown.HasStarted)
            {
                AudioSourceProperty.clip = currentWeapon.SwitchOutAudio;
                AudioSourceProperty.Play();
                AnimatorProperty.SetBool(currentWeapon.SwitchAnimationParameterName, true);

                SwitchOutCountDown.Start(currentWeapon.SwitchingTime);
            }
            else if (SwitchOutCountDown.HasEnded && !SwitchInCountDown.HasStarted)
            {
                LoaderProperty.LoadWeapon(
                    nextWeapon, 
                    ref burstShotCount,
                    ref weaponAnimator,
                    ref weaponInstance,
                    ref barrelFlashSpawn,
                    ref projectileSpawn,
                    ref ejectedCartridgeSpawn,
                    ref barrelFlashAudioSource
                    );

                AudioSourceProperty.clip = nextWeapon.SwitchInAudio;
                AudioSourceProperty.Play();

                AnimatorProperty.SetBool(nextWeapon.SwitchAnimationParameterName, false);

                SwitchInCountDown.Start(nextWeapon.SwitchingTime);

            }
            else if (SwitchInCountDown.HasEnded)
            {
                isSwitching = false;
                isCurrentWeaponDisabled = false;

                currentWeapon = nextWeapon;
            }
        }
    }
}
