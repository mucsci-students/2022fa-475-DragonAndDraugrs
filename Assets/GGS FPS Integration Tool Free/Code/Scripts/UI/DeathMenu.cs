using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGSFPSIntegrationTool.Scripts.UI
{
    public class DeathMenu : MonoBehaviour
    {
        [SerializeField] GameObject _DeathMenu;
        [SerializeField] Text _RespawnTimeMessage;
        [SerializeField] Text _RespawnTime;

        [Space]

        [SerializeField] bool _WillRespawnAfterButtonPress = true;
        [SerializeField] Button _RespawnButton;

        [Space]

        [SerializeField] Player.PlayerDeath _PlayerDeath;

        public static bool IsDeathMenuShown { get; private set; } = false;

        void Awake()
        {
            IsDeathMenuShown = false;
        }

        void Start()
        {
            _DeathMenu.SetActive(false);
        }

        void Update()
        {
            if (EndGameMenu.HasGameEnded)
            {
                if (_DeathMenu.activeSelf)
                {
                    _DeathMenu.SetActive(false);
                }

                return;
            }

            if (!_PlayerDeath.IsDead)
            {
                if (_DeathMenu.activeSelf)
                {
                    _DeathMenu.SetActive(false);
                }

                IsDeathMenuShown = false;

                return;
            }

            if (!_DeathMenu.activeSelf)
            {
                _DeathMenu.SetActive(true);
            }

            if (!PauseMenu.IsGamePaused && !EndGameMenu.HasGameEndedWithDelayRemaining)
            {
                if (!_PlayerDeath.RespawnCountDown.HasCountDownEnded)
                {
                    // Enable respawn time display
                    if (!_RespawnTimeMessage.gameObject.activeSelf)
                    {
                        _RespawnTimeMessage.gameObject.SetActive(true);
                    }

                    // Disable respawn buttom
                    if (_RespawnButton.gameObject.activeSelf)
                    {
                        _RespawnButton.gameObject.SetActive(false);
                    }

                    _RespawnTime.text = _PlayerDeath.RespawnCountDown.CurrentCount.ToString("F1");
                }
                else
                {
                    if (_WillRespawnAfterButtonPress)
                    {
                        // Disable respawn time display
                        if (_RespawnTimeMessage.gameObject.activeSelf)
                        {
                            _RespawnTimeMessage.gameObject.SetActive(false);
                        }

                        // Enable respawn buttom
                        if (!_RespawnButton.gameObject.activeSelf)
                        {
                            _RespawnButton.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        _PlayerDeath.DisableDeath();
                    }
                }
            }
            else
            {
                // Disable respawn time display
                if (_RespawnTimeMessage.gameObject.activeSelf)
                {
                    _RespawnTimeMessage.gameObject.SetActive(false);
                }

                // Disable respawn buttom
                if (_RespawnButton.gameObject.activeSelf)
                {
                    _RespawnButton.gameObject.SetActive(false);
                }
            }

            IsDeathMenuShown = true;
        }
    }
}

