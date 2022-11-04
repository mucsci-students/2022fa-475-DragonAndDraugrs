using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    [System.Serializable]
    public class NewTotalAmmoCount
    {
        public Ammo AmmoType;
        public short Count;
    }

    public class TotalAmmoManager : MonoBehaviour
    {
        [SerializeField] WeaponSystems _CharacterWeaponSystem;
        [SerializeField] bool _WillAmmoCountsSetOnStart = true;
        
        public List<NewTotalAmmoCount> NewTotalAmmoCounts;

        void Start()
        {
            if (_WillAmmoCountsSetOnStart)
            {
                SetTotalAmmoCounts();
            }
        }

        // For UnityEvents
        public void SetTotalAmmoCounts()
        {
            foreach (NewTotalAmmoCount c in NewTotalAmmoCounts)
            {
                for (int i = 0; i < _CharacterWeaponSystem.WeaponSpaceManager.AmmoManager.TotalAmmoCounts.Count; i++)
                {
                    _CharacterWeaponSystem.WeaponSpaceManager.AmmoManager.TotalAmmoCounts[c.AmmoType.name] = c.Count;
                }
            }
        }
    }
}