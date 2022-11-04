using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Array
{
    /// <summary>
    /// Provides information of the values within numeric arrays.
    /// </summary>
    public static class NumericChecker
    {
        /// <summary>
        /// Returns true if all array elements are more than the target value.
        /// </summary>
        /// <param name="target">Threshold that array values should be more than.</param>
        /// <param name="valuesToCompare">Array values that will be compared.</param>
        /// <param name="IsEqualToAccepted">Defines if array values that are equal to the threshold are also accepted.</param>
        public static bool AreAllMoreThan(float target, float[] valuesToCompare, bool IsEqualToAccepted = false)
        {
            for (int i = 0; i < valuesToCompare.Length; i++)
            {
                if (IsEqualToAccepted)
                {
                    if (valuesToCompare[i] < target)
                    {
                        return false;
                    }
                }
                else
                {
                    if (valuesToCompare[i] <= target)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if all array elements are less than the target value.
        /// </summary>
        /// <param name="target">Threshold that array values should be less than.</param>
        /// <param name="valuesToCompare">Array values that will be compared.</param>
        /// <param name="IsEqualToAccepted">Defines if array values that are equal to the threshold are also accepted.</param>
        public static bool AreAllLessThan(float target, float[] valuesToCompare, bool IsEqualToAccepted = false)
        {
            for (int i = 0; i < valuesToCompare.Length; i++)
            {
                if (IsEqualToAccepted)
                {
                    if (valuesToCompare[i] > target)
                    {
                        return false;
                    }
                }
                else
                {
                    if (valuesToCompare[i] >= target)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if all array elements are equal to the target value.
        /// </summary>
        /// <param name="target">The value that array values should match.</param>
        /// <param name="valuesToCompare">Array values that will be compared.</param>
        public static bool AreAllEqualTo(float target, float[] valuesToCompare)
        {
            for (int i = 0; i < valuesToCompare.Length; i++)
            {
                if (valuesToCompare[i] != target)
                {
                    return false;
                }
            }

            return true;
        }
    }
}