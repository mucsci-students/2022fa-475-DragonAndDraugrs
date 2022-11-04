using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts
{
    [System.Serializable]
    public class Threshold
    {
        public int Count;
        public UnityEvent OnThreshold = new UnityEvent();
        public bool HasBeenExecuted;

        // For Editor
        public bool IsFoldoutOpen;
        public bool HasOnThresholdBeenCleared;
    }

    public class Counter : MonoBehaviour
    {
        public List<Threshold> Thresholds;

        public UnityEvent OnIncrement = new UnityEvent();
        public int CurrentCount { get; private set; } = 0;

        void Update()
        {
            for (int i = 0; i < Thresholds.Count; i++)
            {
                if (CurrentCount >= Thresholds[i].Count && !Thresholds[i].HasBeenExecuted)
                {
                    Thresholds[i].OnThreshold?.Invoke();
                    Thresholds[i].HasBeenExecuted = true;
                }
            }
        }

        public void IncrementCount()
        {
            CurrentCount++;
            OnIncrement?.Invoke();
        }
        public void IncrementCount(int amount)
        {
            CurrentCount += amount;
            OnIncrement?.Invoke();
        }
    }
}