using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Time
{
    /// <summary>
    /// Counts up in seconds towards specified thresholds.
    /// </summary>
    public class UpCounter : Counter
    {
        /// <summary>
        /// Start counting up.
        /// </summary>
        public void Start()
        {
            CurrentCount = 0f;
            HasStarted = true;
            IsCounting = true;
        }

        /// <summary>
        /// Process counting up frame-by-frame.
        /// </summary>
        public override void Update()
        {
            CurrentCount += UnityEngine.Time.deltaTime;
        }

        /// <summary>
        /// Returns true if current count has passed threshold.
        /// </summary>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public bool HasCountPassedThreshold(float threshold)
        {
            if (CurrentCount > threshold)
            {
                return true;
            }

            return false;
        }
    }
}