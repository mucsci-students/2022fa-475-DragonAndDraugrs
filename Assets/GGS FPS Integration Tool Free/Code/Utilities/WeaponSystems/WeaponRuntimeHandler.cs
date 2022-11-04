using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems
{
    /// <summary>
    /// Handles weapon's runtime data.
    /// </summary>
    public class WeaponRuntimeHandler
    {
        public bool IsWeaponEnabled = false;
        public int WeaponAmmoCount = 0;
        public float NextShotCountDown = 0f;

        public static void InitialiseArray(ref WeaponRuntimeHandler[] weaponRuntimeHandlers)
        {
            for (int i = 0; i < weaponRuntimeHandlers.Length; i++)
            {
                weaponRuntimeHandlers[i] = new WeaponRuntimeHandler();
            }
        }

        public static bool[] GetIsWeaponEnabledArray(WeaponRuntimeHandler[] weaponRuntimeHandlers)
        {
            bool[] isWeaponEnabledArray = new bool[weaponRuntimeHandlers.Length];

            for (int i = 0; i < weaponRuntimeHandlers.Length; i++)
            {
                isWeaponEnabledArray[i] = weaponRuntimeHandlers[i].IsWeaponEnabled;
            }

            return isWeaponEnabledArray;
        }

        public static int[] GetWeaponAmmoCounts(WeaponRuntimeHandler[] weaponRuntimeHandlers)
        {
            int[] weaponAmmoCounts = new int[weaponRuntimeHandlers.Length];

            for (int i = 0; i < weaponRuntimeHandlers.Length; i++)
            {
                weaponAmmoCounts[i] = weaponRuntimeHandlers[i].WeaponAmmoCount;
            }

            return weaponAmmoCounts;
        }

        public static float[] GetNextShotCountDowns(WeaponRuntimeHandler[] weaponRuntimeHandlers)
        {
            float[] nextShotCountDowns = new float[weaponRuntimeHandlers.Length];

            for (int i = 0; i < weaponRuntimeHandlers.Length; i++)
            {
                nextShotCountDowns[i] = weaponRuntimeHandlers[i].NextShotCountDown;
            }

            return nextShotCountDowns;
        }

        public static void SetIsWeaponEnabledArray(
            ref WeaponRuntimeHandler[] weaponRuntimeHandlers,
            bool[] isWeaponEnabledArray
            )
        {
            for (int i = 0; i < isWeaponEnabledArray.Length; i++)
            {
                 weaponRuntimeHandlers[i].IsWeaponEnabled = isWeaponEnabledArray[i];
            }
        }

        public static void SetWeaponAmmoCounts(
            ref WeaponRuntimeHandler[] weaponRuntimeHandlers,
            int[] weaponAmmoCounts
            )
        {
            for (int i = 0; i < weaponAmmoCounts.Length; i++)
            {
                 weaponRuntimeHandlers[i].WeaponAmmoCount = weaponAmmoCounts[i];
            }
        }

        public static void SetNextShotCountDowns(
            ref WeaponRuntimeHandler[] weaponRuntimeHandlers,
            float[] nextShotCountDowns
            )
        {
            for (int i = 0; i < nextShotCountDowns.Length; i++)
            {
                 weaponRuntimeHandlers[i].NextShotCountDown = nextShotCountDowns[i];
            }
        }
    }
}