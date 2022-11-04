using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject _PauseMenu;

        public static bool IsGamePausedWithOverride { get; set; } = false;

        public static bool IsGamePaused { get; private set; } = false;

        void Awake()
        {
            // Reinitialise static properties
            IsGamePausedWithOverride = false;
            IsGamePaused = false;
        }

        void Start()
        {
            ResumeGame();
        }

        void Update()
        {
            if (Input.GetButtonUp("Toggle Pause") && !IsGamePausedWithOverride)
            {
                TogglePauseState();
            }

            UpdateTimeScale();

                // ? needs optimising?
            CursorManager.ReportCursorLockingAuthorisation(this, !IsGamePaused);
        }

        void UpdateTimeScale()
        {
            if (IsGamePaused || IsGamePausedWithOverride)
            {
                if (Time.timeScale != 0f)
                {
                    Time.timeScale = 0f;
                }
            }
            else
            {
                if (Time.timeScale != 1f)
                {
                    Time.timeScale = 1f;
                }
            }
        }

        void TogglePauseState()
        {
            if (IsGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            if (IsGamePausedWithOverride)
            {
                return;
            }

            if (!_PauseMenu.activeSelf)
            {
                _PauseMenu.SetActive(true);
            }

            IsGamePaused = true;
        }

        public void ResumeGame()
        {
            if (IsGamePausedWithOverride)
            {
                return;
            }

            if (_PauseMenu.activeSelf)
            {
                _PauseMenu.SetActive(false);
            }

            IsGamePaused = false;
        }
    }
}

