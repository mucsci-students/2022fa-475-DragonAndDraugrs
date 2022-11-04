using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Fire
{
    public class Authoriser
    {
        public int TargetSemiautoFiringTypeIndex { get; set; } = 0;
        public int TargetAutoFiringTypeIndex { get; set; } = 1;

        public bool IsFireAuthorised(
            int firingTypeIndex,
            bool isSemiautoFireInputDetected,
            bool isAutoFireInputDetected,
            int burstShotCount,
            int shotsPerBurst,
            float nextShotCountDown,
            float weaponAmmoCount,
            float ammoLossPerShot,
            bool[] isConflictingStateFalseArray,
            float[] conflictingCountDowns
            )
        {
            bool
                isSemiautoFireInputConfirmed = IsFireInputConfirmed(firingTypeIndex, isSemiautoFireInputDetected, TargetSemiautoFiringTypeIndex),
                isAutoFireInputConfirmed = IsFireInputConfirmed(firingTypeIndex, isAutoFireInputDetected, TargetAutoFiringTypeIndex),
                areBurstShotsOutstanding = AreBurstShotsOutstanding(burstShotCount, shotsPerBurst),
                hasNextShotCountedDown = HasNextShotCountedDown(nextShotCountDown),
                isEnoughAmmoAvailable = IsEnoughAmmoAvailable(weaponAmmoCount, ammoLossPerShot),
                areConflictingStatesFalse = General.Array.BoolChecker.AreAllOfState(false, isConflictingStateFalseArray),
                areConflictingCountDownsZero = General.Array.NumericChecker.AreAllLessThan(0, conflictingCountDowns, true);
                
            if (
                    !
                    (
                        isSemiautoFireInputConfirmed
                        ||
                        isAutoFireInputConfirmed
                        ||
                        areBurstShotsOutstanding
                    )
                )
            {
                return false;
            }
            else if (
                    !
                    (
                        hasNextShotCountedDown
                        &&
                        isEnoughAmmoAvailable
                        &&
                        areConflictingStatesFalse
                        &&
                        areConflictingCountDownsZero
                    )
                )
            {
                return false;
            }

            return true;
        }

        bool IsFireInputConfirmed(int firingTypeIndex, bool isFireInputDetected, int targetFiringTypeIndex)
        {
            if (firingTypeIndex == targetFiringTypeIndex && isFireInputDetected)
            {
                return true;
            }

            return false;
        }

        bool AreBurstShotsOutstanding(int burstShotCount, int shotsPerBurst)
        {
            if (burstShotCount > 0 && burstShotCount < shotsPerBurst)
            {
                return true;
            }

            return false;
        }

        bool HasNextShotCountedDown(float nextShotCountDown)
        {
            if (nextShotCountDown <= 0f)
            {
                return true;
            }

            return false;
        }

        bool IsEnoughAmmoAvailable(float weaponAmmoCount, float ammoLossPerShot)
        {
            if (weaponAmmoCount >= ammoLossPerShot)
            {
                return true;
            }

            return false;
        }
    }
}


