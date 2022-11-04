using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.NonPlayer
{
    public class BossShield : MonoBehaviour
    {
        [SerializeField] NonPlayerDeath _CharacterNonPlayerDeath;
        [SerializeField] float _DegreesRotationPerSecond = 45f;
        [SerializeField] float _ActivatedDuration = 7f;
        [SerializeField] float _DeactivatedDuration = 3.5f;
        [SerializeField] float _SecondsUntilFirstActivation = 3f;
        [SerializeField] float _ShieldScalingDuration = 0.5f;

        [SerializeField] GameObject[] ShieldGameObjects;

        bool IsShieldActive { get; set; } = false;

        Utilities.General.Time.DownCounter ToggleShieldCountDown { get; set; } = 
            new Utilities.General.Time.DownCounter();

        float CurrentShieldScale { get; set; } = 0f;

        void Start()
        {
            

            ToggleShieldCountDown.Start(_SecondsUntilFirstActivation);

            ToggleShieldGameObjects(IsShieldActive);
        }

        void Update()
        {
            transform.Rotate(0f, 0f, _DegreesRotationPerSecond * Time.deltaTime);

            if (_CharacterNonPlayerDeath.IsDead)
            {
                IsShieldActive = false;
            }

            if (ToggleShieldCountDown.HasEnded)
            {
                if (!_CharacterNonPlayerDeath.IsDead)
                {
                    IsShieldActive = !IsShieldActive;

                    ToggleShieldCountDown.Start(IsShieldActive ? _ActivatedDuration : _DeactivatedDuration);
                }
            }

            // Prevents dividing-by-zero errors
            if (_ShieldScalingDuration <= 0f)
            {
                _ShieldScalingDuration = Mathf.Epsilon;
            }

            if (IsShieldActive)
            {
                CurrentShieldScale += Time.deltaTime / _ShieldScalingDuration;
            }
            else
            {
                CurrentShieldScale -= Time.deltaTime / _ShieldScalingDuration;
            }

            CurrentShieldScale = Mathf.Clamp01(CurrentShieldScale);

            transform.localScale = new Vector3(CurrentShieldScale, CurrentShieldScale, CurrentShieldScale);


            if (CurrentShieldScale > 0f)
            {
                ToggleShieldGameObjects(true);
            }
            else
            {
                ToggleShieldGameObjects(false);
            }

            ToggleShieldCountDown.Update();
        }

        void ToggleShieldGameObjects(bool IsActivated)
        {
            foreach (GameObject g in ShieldGameObjects)
            {
                if (g != transform)
                {
                    g.SetActive(IsActivated);
                }
            }
        }
    }
}
