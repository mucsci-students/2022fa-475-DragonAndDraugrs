using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGSFPSIntegrationTool.Utilities.General.Time;

namespace GGSFPSIntegrationTool.Scripts.UI
{
    public class GameplayDataManager : MonoBehaviour
    {
        public static short KillCount { get; set; }
        public static short DeathCount { get; set; }
        public static string TimeToDisplay
        {
            get
            {
                return Formatter.GetMinuteSecondTime((short)TimeCountUp.CurrentCount);
            }
        }

        static UpCounter TimeCountUp { get; set; } = new UpCounter();

        void Awake()
        {
            KillCount = 0;
            DeathCount = 0;
        }

        void Start()
        {
            TimeCountUp.Start();
        }

        void Update()
        {
            TimeCountUp.Update();
        }

        void OnEnable()
        {
            NonPlayer.NonPlayerDeath.OnDeath += IncrementKillCount;
            Player.PlayerDeath.OnDeath += IncrementDeathCount;
        }

        void OnDisable()
        {
            NonPlayer.NonPlayerDeath.OnDeath -= IncrementKillCount;
            Player.PlayerDeath.OnDeath -= IncrementDeathCount;
        }

        void IncrementKillCount()
        {
            KillCount++;
        }

        void IncrementDeathCount()
        {
            DeathCount++;
        }
    }
}
