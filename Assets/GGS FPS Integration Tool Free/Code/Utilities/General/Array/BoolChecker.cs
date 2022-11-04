using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Array
{
    /// <summary>
    /// Provides information of the states within bool arrays.
    /// </summary>
    public static class BoolChecker
    {
        /// <summary>
        /// Returns true if all bool array states match the target state
        /// </summary>
        /// <param name="target">The state that needs to match the array states.</param>
        /// <param name="valuesToCompare">Array states that will be compared.</param>
        public static bool AreAllOfState(bool target, bool[] valuesToCompare)
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
        /// <summary>
        /// Returns the number of matches between the target state and the bool array states.
        /// </summary>
        /// <param name="target">The state that needs to match the array states.</param>
        /// <param name="valuesToCompare">Array states that will be compared.</param>
        public static int NumberOfStates(bool target, bool[] valuesToCompare)
        {
            int approvedStatesCount = 0;

            for (int i = 0; i < valuesToCompare.Length; i++)
            {
                if (valuesToCompare[i] == target)
                {
                    approvedStatesCount++;
                }
            }

            return approvedStatesCount;
        }
    }
}