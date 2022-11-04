using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Fire
{
    public static class NextShotCountDownHandler
    {
        public static void Update(ref WeaponRuntimeHandler[] weaponRuntimeHandlers)
        {
            float[] nextShotCountDowns = WeaponRuntimeHandler.GetNextShotCountDowns(weaponRuntimeHandlers);

            General.Time.DownCounter.ManualDeltaTimeDecrement(ref nextShotCountDowns);

            WeaponRuntimeHandler.SetNextShotCountDowns(ref weaponRuntimeHandlers, nextShotCountDowns);
        }
    }
}