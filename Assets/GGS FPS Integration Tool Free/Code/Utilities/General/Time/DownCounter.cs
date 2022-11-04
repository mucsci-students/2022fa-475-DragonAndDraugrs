using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Time
{
    /// <summary>
    /// Counts down in seconds towards zero.
    /// </summary>
    public class DownCounter : Counter
    {
        //public float StartCount { get; private set; } = 0f;

        /// <summary>
        /// Is true if count has been started, even after ending. 
        /// </summary>
        public bool HasStartedOnce { get; private set; } = false;

        /// <summary>
        /// Is true if count down has ended, but only just after and not continuously.
        /// </summary>
        public bool HasEnded
        {
            get
            {
                if (CurrentCount <= 0f && HasStarted)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Is true continuously after count down has ended, until starting again.
        /// </summary>
        public bool HasCountDownEnded
        {
            get
            {
                if (CurrentCount <= 0f && HasStartedOnce)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Start counting down.
        /// </summary>
        /// <param name="startCount">Amount of seconds to count down from.</param>
        public void Start(float startCount)
        {
            CurrentCount = startCount;
            HasStarted = true;
            HasStartedOnce = true;
            IsCounting = true;
        }

        /// <summary>
        /// Process count down frame-by-frame.
        /// </summary>
        public override void Update()
        {
            // Ensures HasStarted is false on the next frame of count down ending
            if (CurrentCount <= 0f)
            {
                HasStarted = false;
            }

            // Decrement if necessary
            if (CurrentCount > 0f && IsCounting)
            {
                CurrentCount -= UnityEngine.Time.deltaTime;

                if (CurrentCount <= 0f)
                {
                    CurrentCount = 0f;
                    IsCounting = false;
                }
            }
        }

        /// <summary>
        /// Decrease independant count value by one per second.
        /// </summary>
        /// <param name="count">Count value to decreace.</param>
        public static void ManualDeltaTimeDecrement(ref float count)
        {
            if (count > 0f)
            {
                count -= UnityEngine.Time.deltaTime;

                if (count < 0f)
                {
                    count = 0;
                }
            }
        }
        /// <summary>
        /// Decrease independant count values by one per second.
        /// </summary>
        /// <param name="counts">Count values to decreace.</param>
        public static void ManualDeltaTimeDecrement(ref float[] counts)
        {
            for (int i = 0; i < counts.Length; i++)
            {
                if (counts[i] > 0f)
                {
                    counts[i] -= UnityEngine.Time.deltaTime;

                    if (counts[i] < 0f)
                    {
                        counts[i] = 0;
                    }
                }
            }
        }
    }
}