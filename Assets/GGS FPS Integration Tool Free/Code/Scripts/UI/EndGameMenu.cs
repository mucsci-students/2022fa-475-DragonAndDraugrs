using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGSFPSIntegrationTool.Scripts.UI
{
    public abstract class EndGameMenu : MonoBehaviour
    {
        [SerializeField] GameObject _Menu;

        public static bool HasGameEnded { get; private set; }
        public static bool HasGameEndedWithDelayRemaining { get; private set; }

        Utilities.General.Time.DownCounter DelayedCountDown { get; set; } = new Utilities.General.Time.DownCounter();
        
        void Awake()
        {
            HasGameEnded = false;
            HasGameEndedWithDelayRemaining = false;

            _Menu.SetActive(false);
            CursorManager.ReportCursorLockingAuthorisation(this, true);
        }

        void Update()
        {
            if (DelayedCountDown.HasEnded)
            {
                EnableEndGame();
            }

            DelayedCountDown.Update();
        }

        public void EnableEndGame()
        {
            _Menu.SetActive(true);

            PauseMenu.IsGamePausedWithOverride = true;
            CursorManager.ReportCursorLockingAuthorisation(this, false);

            HasGameEnded = true;
        }

        public void EnableDelayedEndGame(float delayInSeconds)
        {
            DelayedCountDown.Start(delayInSeconds);

            HasGameEndedWithDelayRemaining = true;
        }
    }
}
