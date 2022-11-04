using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Fire.Visual
{
    public class BarrelFlashManager
    {
        public float BarrelFlashLifetime { get; set; } = 0.5f;

        public void CreateBarrelFlash(GameObject barrelFlashPrefab, Transform barrelFlashSpawn)
        {
            if (barrelFlashPrefab == null)
            {
                return;
            }

            GameObject flashInstance = Object.Instantiate(barrelFlashPrefab, barrelFlashSpawn.position, barrelFlashSpawn.rotation);
            Object.Destroy(flashInstance, BarrelFlashLifetime);
        }
    }
}