using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Fire.Visual
{
    public class Manager
    {
        Animator AnimatorProperty { get; set; }

        BarrelFlashManager BarrelFlashManagerProperty { get; set; } = new BarrelFlashManager();
        CartridgeEjector CartridgeEjectorProperty { get; set; } = new CartridgeEjector();

        public Manager(Animator animatorProperty)
        {
            AnimatorProperty = animatorProperty;
        }

        public void Update(string animatorFireParameterName)
        {
            AnimatorProperty.SetBool(animatorFireParameterName, false);
        }

        public void CreateVisuals(
            string animatorFireParameterName,
            GameObject barrelFlashPrefab,
            Transform barrelFlashSpawn,
            GameObject cartridgePrefab,
            Transform cartridgeSpawn,
            Vector3 ejectionDirection,
            float ejectionForce,
            Vector3 playerVelocity
            )
        {
            AnimatorProperty.SetBool(animatorFireParameterName, true);
            BarrelFlashManagerProperty.CreateBarrelFlash(barrelFlashPrefab, barrelFlashSpawn);

            CartridgeEjectorProperty.EjectCartridge(cartridgePrefab, cartridgeSpawn, ejectionDirection, ejectionForce, playerVelocity);
        }


    }
}

