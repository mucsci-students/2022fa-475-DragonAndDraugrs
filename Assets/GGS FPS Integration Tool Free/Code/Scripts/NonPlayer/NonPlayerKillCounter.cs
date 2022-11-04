using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts.NonPlayer
{
    public class NonPlayerKillCounter : MonoBehaviour
    {
        [SerializeField] short _RequiredKillCount = 50;

        [Space]

        [SerializeField] UnityEvent _OnNonPlayerKill;
        [SerializeField] UnityEvent _OnAllNonPlayersKilled;

        short LastKillCount { get; set; } = 0;
        bool HasReportedRequiredKillCountMet { get; set; } = false;

        void Start()
        {
            // Calling via Start() ensures default values are set (from ObjectiveDisplay)
            // before overriding with these values
            UI.ObjectiveDisplay.MessageText = "Zombies Killed";
            UI.ObjectiveDisplay.TargetCompletionCount = _RequiredKillCount;
        }

        void Update()
        {
            if (UI.GameplayDataManager.KillCount != LastKillCount)
            {
                UI.ObjectiveDisplay.CurrentCompletionCount = UI.GameplayDataManager.KillCount;
                _OnNonPlayerKill?.Invoke();

                LastKillCount = UI.GameplayDataManager.KillCount;
            }

            if (UI.GameplayDataManager.KillCount >= _RequiredKillCount && !HasReportedRequiredKillCountMet)
            {
                _OnAllNonPlayersKilled?.Invoke();

                HasReportedRequiredKillCountMet = true;
            }
        }
    }
}

