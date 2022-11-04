using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts
{
    [System.Serializable]
    public class Wave
    {
        public UnityEvent OnStart = new UnityEvent();
        public UnityEvent OnEnd = new UnityEvent();

        // For Editor
        public bool IsFoldoutOpen;
        public bool HasUnityEventsBeenCleared;
        public float ReorderableListHeight;
    }

    public class WaveManager : MonoBehaviour
    {
        [SerializeField] bool _WillStartWaveOnStart = true;

        public List<Wave> Waves = new List<Wave>();

        public short CurrentWaveNumber { get; private set; } = 0;
        public bool HasCurrentWaveEnded { get; private set; } = true;

        void Start()
        {
            if (_WillStartWaveOnStart)
            {
                StartNextWave();
            }
        }

        public void StartNextWave()
        {
            // Prevents Wave array being called when empty or at limit
            if (Waves.Count == 0 || CurrentWaveNumber + 1 > Waves.Count)
            {
                return;
            }

            CurrentWaveNumber++;
            Waves[CurrentWaveNumber - 1].OnStart?.Invoke();
            
            HasCurrentWaveEnded = false;
        }

        public void EndCurrentWave()
        {
            Waves[CurrentWaveNumber - 1].OnEnd?.Invoke();

            HasCurrentWaveEnded = true;
        }
    }
}