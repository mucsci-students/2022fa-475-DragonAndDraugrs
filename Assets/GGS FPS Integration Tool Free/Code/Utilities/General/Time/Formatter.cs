using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Time
{
    /// <summary>
    /// Formats time values for displaying.
    /// </summary>
    public static class Formatter
    {
        /// <summary>
        /// Returns time in the minutes-seconds format (MM:SS).
        /// </summary>
        /// <param name="totalSeconds">The whole time value in seconds.</param>
        /// <param name="isLeadingZeroAllowed">If true, leading zero can be displayed for digits resembling seconds.</param>
        /// <returns></returns>
        public static string GetMinuteSecondTime(
            short totalSeconds, 
            bool isLeadingZeroAllowed = true
            )
        {
            // Prevents negitive input
            totalSeconds = (short)(totalSeconds > 0 ? totalSeconds : 0);

            byte seconds = (byte)(totalSeconds % 60);

            // Rounded down by converting float to integer type
            short minutes = (short)(totalSeconds / 60);

            if (seconds < 10 && isLeadingZeroAllowed)
            {
                return minutes + ":0" + seconds;
            }
            else
            {
                return minutes + ":" + seconds;
            }
        }
    }
}