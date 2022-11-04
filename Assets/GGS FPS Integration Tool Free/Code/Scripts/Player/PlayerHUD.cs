using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGSFPSIntegrationTool.Scripts.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] GameObject _HUDGameObject;
        [SerializeField] Image _CrosshairSpace;
        [SerializeField] Image _AmmoIconSpace;
        [SerializeField] Text _WeaponAmmoCountSpace;
        [SerializeField] Text _TotalAmmoCountSpace;

        [Space]
        
        [SerializeField] Text _HealthText;           
        [SerializeField] Image _LowHealthOverlay;
        [SerializeField] Image _DamageOverlay;
        [SerializeField] [Range(0f, 1f)] float _MaxDamageOverlayOpacity = 0.3f;
        [SerializeField] [Range(0f, 5f)] float _DamageOverlayFadeDuration = 0.5f;

        [Space]

        [SerializeField] WeaponSystems _CharacterWeaponSystems;
        [SerializeField] Health _CharacterHealth;

        float CurrentDamageOverlayOpacity { get; set; }

        void Update()
        {
            _HealthText.text = ((int)_CharacterHealth.HealthCount).ToString();

            UpdateHUD();
            UpdateLowHealthOverlay();
            UpdateDamageOverlay();
        }

        void UpdateHUD()
        {
            if (UI.EndGameMenu.HasGameEnded)
            {
                if (_HUDGameObject.activeSelf)
                {
                    _HUDGameObject.SetActive(false);
                }

                return;
            }

            Utilities.WeaponSystems.Manager weaponSpaceManager = _CharacterWeaponSystems.WeaponSpaceManager;

            // Prevents Null Exception errors while weaponSpaceManager is reinitialising 
            // during player respawning
            if (weaponSpaceManager.SelectedWeapon == null)
            {
                return;
            }

            if (!UI.DeathMenu.IsDeathMenuShown)
            {
                if (!_CrosshairSpace.gameObject.activeSelf)
                {
                    _CrosshairSpace.gameObject.SetActive(true);
                }

                _CrosshairSpace.sprite = weaponSpaceManager.SelectedWeapon.CrosshairSprite;

                if (weaponSpaceManager.AimManager.CurrentAimingInterpolation <= 0f)
                {
                    _CrosshairSpace.color = weaponSpaceManager.SelectedWeapon.CrosshairColour;
                }
                else
                {
                    Color c = weaponSpaceManager.SelectedWeapon.CrosshairColour;
                    _CrosshairSpace.color = new Color(c.r, c.g, c.b, 0f);
                }
            }
            else
            {
                if (_CrosshairSpace.gameObject.activeSelf)
                {
                    _CrosshairSpace.gameObject.SetActive(false);
                }
            }

            _AmmoIconSpace.sprite = weaponSpaceManager.SelectedWeapon.AmmoProperty.AmmoIcon;
            _WeaponAmmoCountSpace.text 
                = weaponSpaceManager.WeaponRuntimeHandlers[weaponSpaceManager.SelectedWeaponIndex].WeaponAmmoCount.ToString();
            _TotalAmmoCountSpace.text
                = weaponSpaceManager.AmmoManager.TotalAmmoCounts[weaponSpaceManager.SelectedWeapon.AmmoProperty.name].ToString();
        }

        void UpdateLowHealthOverlay()
        {
            float alpha = (_CharacterHealth.MaximumHealth - _CharacterHealth.HealthCount) / _CharacterHealth.MaximumHealth;

            // To assist with negitive health
            alpha = Mathf.Clamp(alpha, 0f, 1f);

            Color c = _LowHealthOverlay.color;
            _LowHealthOverlay.color = new Color(c.r, c.g, c.b, alpha);
        }

        void UpdateDamageOverlay()
        {
            if (CurrentDamageOverlayOpacity > 0f)
            {
                CurrentDamageOverlayOpacity -= Time.deltaTime / _DamageOverlayFadeDuration;
            }
            
            if (CurrentDamageOverlayOpacity < 0f)
            {
                CurrentDamageOverlayOpacity = 0f;
            }

            Color c = _DamageOverlay.color;
            _DamageOverlay.color = new Color(c.r, c.g, c.b, CurrentDamageOverlayOpacity);
        }

        // Call via OnDamage Event on Health component
        public void PlayDamageEffects()
        {
            CurrentDamageOverlayOpacity = _MaxDamageOverlayOpacity;
        }
    }
}

