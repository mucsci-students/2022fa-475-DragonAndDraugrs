using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Time
{
    /// <summary>
    /// Parent class for classes that count time in seconds.
    /// </summary>
    public abstract class Counter
    {
        /// <summary>
        /// Current time count in seconds.
        /// </summary>
        public float CurrentCount { get; protected set; } = 0f;
        /// <summary>
        /// Is true if counting has started but not yet ended.
        /// </summary>
        public bool HasStarted { get; protected set; } = false;
        /// <summary>
        /// Is true if counting is currently being executed.
        /// </summary>
        public bool IsCounting { get; protected set; } = false;

        /// <summary>
        /// For implementing the counting system which will be call repeatedly frame-by-frame.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Resume counting after being paused.
        /// </summary>
        public void Resume()
        {
            IsCounting = true;
        }

        /// <summary>
        /// Pause counting process, to resume later.
        /// </summary>
        public void Pause()
        {
            IsCounting = false;
        }

        /// <summary>
        /// End counting process before count has finished. 
        /// </summary>
        public void End()
        {
            CurrentCount = 0f;
            IsCounting = false;
        }

        /// <summary>
        /// Increase count by certain amount of seconds.
        /// </summary>
        /// <param name="amountToAdd">Amount of seconds to add to count.</param>
        public void AddToCount(float amountToAdd)
        {
            if (amountToAdd <= 0)
            {
                return;
            }

            CurrentCount += amountToAdd;
        }

        /// <summary>
        /// Decrease count by certain amount of seconds.
        /// </summary>
        /// <param name="amountToDeduct">Amount of seconds to deduct from count.</param>
        public void DeductFromCount(float amountToDeduct)
        {
            if (amountToDeduct <= 0)
            {
                return;
            }

            CurrentCount -= amountToDeduct;
        }
    }
}