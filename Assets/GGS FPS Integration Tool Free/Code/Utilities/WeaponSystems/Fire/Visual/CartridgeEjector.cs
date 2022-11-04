using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Fire.Visual
{
    public class CartridgeEjector
    {
        public float CartridgeLifetime { get; set; } = 2f;
        public float PlayerVelocityEffectMultiplier { get; set; } = 3f;

        public void EjectCartridge(
            GameObject cartridgePrefab,
            Transform cartridgeSpawn,
            Vector3 ejectionDirection,
            float ejectionForce,
            Vector3 playerVelocity
            )
        {
            if (cartridgePrefab == null)
            {
                return;
            }

            GameObject cartridgeInstance = Object.Instantiate(cartridgePrefab, cartridgeSpawn.position, cartridgeSpawn.rotation);
            Object.Destroy(cartridgeInstance, CartridgeLifetime);

            Vector3 ejectionTrajectory = cartridgeSpawn.rotation * ejectionDirection.normalized;

            cartridgeInstance.GetComponent<Rigidbody>().AddForce((ejectionTrajectory * ejectionForce) + (playerVelocity * PlayerVelocityEffectMultiplier));
            
        }
    }
}