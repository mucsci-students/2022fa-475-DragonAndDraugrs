using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Fire
{
    public class FireHandler
    {
        public float ImpactLifeTime { get; set; } = 1f;

        
        GameObject BloodSplatterImpact { get; set; }        // ? implement in its own system
        LayerMask RaycastIgnorableLayers { get; set; }

        public FireHandler(
            GameObject bloodSplatterImpact,
            LayerMask raycastIgnorableLayers
            )
        {
            BloodSplatterImpact = bloodSplatterImpact;
            RaycastIgnorableLayers = raycastIgnorableLayers;
        }


        public void FireWeapon(
            Scripts.Weapon weapon,
            float aimingInterpolationValue,
            bool isPlayerMoving,
            Transform raySpawn
            )
        {
            for (int i = 1; i <= weapon.OutputPerShot; i++)
            {
                Vector3 spreadVector = GenerateSpreadVector(
                    aimingInterpolationValue,
                    isPlayerMoving,
                    weapon.AimSpread,
                    weapon.HipSpread,
                    weapon.MovementSpread
                    );
                
                ProjectRay(
                    raySpawn,
                    spreadVector,
                    weapon.RayModeProperty.Range,
                    weapon.RayModeProperty.ImpactPrefab,
                    weapon.RayModeProperty.ImpactForce,
                    weapon.DamagePerShot / weapon.OutputPerShot
                    );
            }
        }

        public void FireWeapon(
            Scripts.Weapon weapon,
            float aimingInterpolationValue,
            bool isPlayerMoving,
            Transform projectileSpawn,
            Vector3 forwardVector
            )
        {
            for (int i = 1; i <= weapon.OutputPerShot; i++)
            {
                Vector3 spreadVector = GenerateSpreadVector(
                    aimingInterpolationValue,
                    isPlayerMoving,
                    weapon.AimSpread,
                    weapon.HipSpread,
                    weapon.MovementSpread
                    );

                LaunchProjectile(
                    spreadVector,
                    weapon.ProjectileModeProperty.ProjectilePrefab,
                    projectileSpawn,
                    forwardVector,
                    weapon.ProjectileModeProperty.LaunchForce
                    );
            }
        }

        Vector3 GenerateSpreadVector(
            float aimingInterpolationValue,
            bool isPlayerMoving,
            float aimSpread,
            float hipSpread,
            float movementSpread
            )
        {
            if (aimingInterpolationValue >= 1f)
            {
                return GenerateSpreadOffset(aimSpread);
            }
            else if (isPlayerMoving)
            {
                return GenerateSpreadOffset(movementSpread);
            }

            return GenerateSpreadOffset(hipSpread);
        }

        Vector3 GenerateSpreadOffset(float eulerAngleOffset)
        {
            Vector3 output = Vector3.zero;

            for (int i = 0; i < 3; i++)
            {
                output[i] = Random.Range(-eulerAngleOffset, eulerAngleOffset);
            }

            return output;
        }

        void ProjectRay(
            Transform raySpawn,
            Vector3 spreadVector,
            float rayRange,
            GameObject defaultImpactPrefab,
            float impactForce,
            float healthDamage
            )
        {
            RaycastHit hit;

            // RaycastIgnorableLayers is needed to ignore multiple layers
            // Raycast's integer parameter behaves like bool array at bit level
            // '~' converts integer to negitive spectrum, thus defines listed layers will be ignored
            if (Physics.Raycast(raySpawn.position, raySpawn.forward + spreadVector, out hit, rayRange, ~RaycastIgnorableLayers))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                if (hit.collider.gameObject.layer == 13) // ? should number be editable with property
                {
                    Scripts.Health hitHealth = hit.collider.gameObject.GetComponentInParent<Scripts.Health>();
                
                    if (hitHealth != null)
                    {
                        hitHealth.InflictDamage(healthDamage, hit.collider);
                    }

                        // ? Should make independant impact manager?
                    if (BloodSplatterImpact != null)
                    {
                        GameObject hitInstance = Object.Instantiate(BloodSplatterImpact, hit.point, Quaternion.LookRotation(hit.normal));
                        Object.Destroy(hitInstance, ImpactLifeTime);
                    }
                }
                else
                {
                    if (defaultImpactPrefab != null)
                    {
                        GameObject hitInstance = Object.Instantiate(defaultImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        Object.Destroy(hitInstance, ImpactLifeTime);
                    }
                }
            }
        }

        void LaunchProjectile(
            Vector3 spreadVector,
            GameObject projectileObject,
            Transform projectileSpawn,
            Vector3 forwardVector,
            float projectileLaunchForce
            )
        {
            if (projectileObject != null)
            {
                GameObject projectileInstance = Object.Instantiate(projectileObject, projectileSpawn.position, projectileSpawn.rotation);

                projectileInstance.transform.Rotate(90f, 0f, 0f);
                projectileInstance.GetComponent<Rigidbody>().AddForce((forwardVector + spreadVector) * projectileLaunchForce);
            }
        }
    }
}
