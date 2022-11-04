using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    

    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
    public class Weapon : ScriptableObject
    {
        static class Tooltips
        {
            public const string
                WeaponPrefab = "GameObject for visualising the weapon.",
                BarrelFlashPrefab = "GameObject that resembles the weapon's barral flash. Often involves partical effects.",

                IsPositionOffset = "States whether the Weapon Space position is offsetted.",
                PositionOffset = "Defines the offset of the Weapon Space position.",

                CrosshairSprite = "Sprite to display at centre of the screen when not aiming.",
                CrosshairColour = "Colour of crosshair sprite.",

                BarrelAudio = "Sound played when firing weapon.",
                ReloadAudio = "Sound played when reloading weapon.",
                SwitchInAudio = "Sound played when switching-in the weapon.",
                SwitchOutAudio = "Sound played when switching-out the weapon.",

                WeaponAnimatorController = "Animator Controller use for the weapon's animations.",
                FireAnimationParameterName = "Name of the fire parameter, within weapon's animator controller. Allows system to control animations.",
                ReloadAniamtionParameterName = "Name of the reload parameter, within weapon's animator controller. Allows system to control animations.",
                SwitchAnimationParameterName = "Name of the switch parameter, within weapon's animator controller. Allows system to control animations.",
                AimAnimationParameterName = "Name of the aim parameter, within weapon's animator controller. Allows system to control animations.",
                RunAnimationParameterName = "Name of the run parameter, within weapon's animator controller. Allows system to control animations.",
                WalkAnimationParameterName = "Name of the walk parameter, within weapon's animator controller. Allows system to control animations.",
                JumpAnimationParameterName = "Name of the jump parameter, within weapon's animator controller. Allows system to control animations.",
                MouseXAnimationParameterName = "Name of the mouse X parameter, within weapon's animator controller. Allows system to control animations.",
                MouseYAnimationParameterName = "Name of the mouse Y parameter, within weapon's animator controller. Allows system to control animations.",

                BarrelFlashSpawnDirectory = "Path of weapon's barrel flash spawn. Example: Default_Weapon/Aimbody/[Flash]",
                ProjectileSpawnDirectory = "Path of weapon's projectile spawn. Example: Default_Weapon/Aimbody/[Pro]",
                CartridgeSpawnDirectory = "Path of weapon's cartridge spawn. Example: Default_Weapon/Aimbody/[Cart]",

                OutputType = "Type of simulated projectile behaviour when firing.",

                FiringType = "Type of firing responce on button press.",
                ShotsPerBurst = "Times fired in serial per button press.",
                ShotsPerSecond = "Maximum rounds fired per second allowed.",
                OutputPerShot = "Projectiles launched in parallel per round.",

                DamagePerShot = "Amount of damage applied to damagable GameObjects during a Raycast hit. Damage is distributed across the multiple Raycasts of the shot.",

                AimSpread = "Amount of offset from the centre of aiming applied to output when aiming.",
                HipSpread = "Amount of offset from the centre of aiming applied to output when not aiming and moving.",
                MovementSpread = "Amount of offset from the centre of aiming applied to output when not aiming, but moving.",

                Ammo = "Ammo object used with this weapon.",
                Capacity = "Maximum amount of ammo that can be loaded in weapon.",
                AmmoLossPerShot = "Amount of ammo used per round.",
                ReloadingType = "Type of reloading behaviour. If set to Partial or Partial Repeat, " +
                "assign the weapon's 'Incremental Reload' clip to the Reload node of its AnimatorController. " +
                "Otherwise use the standard 'Reload' clip instead.",
                AmmoAddedPerReload = "Amount of ammo loaded in weapon per reload.",

                AimingTransitionTime = "Amount of time to aim or un-aim weapon.",
                ReloadingTime = "Amount of time to reload weapon.",
                SwitchingTime = "Amount of time to switch weapon.",
                RunningRecoveryTime = "Amount of time firing is re-enabled after running.",
                IncrementalReloadRecoveryTime = "Amount of time firing is re-enabled after partial reloading is finished or interrupted.",

                ImpactPrefab = "GameObject to spawn where firing ray hits surface.",
                Range = "Range of firing ray.",
                ImpactForce = "Amount of force applied to rigidbody hit by firing ray.",

                ProjectilePrefab = "GameObject to launch when firing.",
                LaunchForce = "Amount of force applied to launched GameObject.",

                CartridgePrefab = "GameObject to spawn that resembles ejected ammo cartridge.",
                EjectionTrajectory = "Direction that ejected ammo cartridge is launched towards.",
                EjectionForce = "Amount of force applied to ejected ammo cartridge when spawned.";
        }

        public enum OutputType { Ray, Projectile }
        public enum FiringType { SemiAutomatic, Automatic }
        public enum ReloadingType { Full, Partial, PartialRepeat }

        [Tooltip(Tooltips.WeaponPrefab)] public GameObject WeaponPrefab;
        [Tooltip(Tooltips.BarrelFlashPrefab)] public GameObject BarrelFlashPrefab;

        [Tooltip(Tooltips.IsPositionOffset)] public bool IsPositionOffset = false;
        [Tooltip(Tooltips.PositionOffset)] public Vector3 PositionOffset = Vector3.zero;

        [Tooltip(Tooltips.CrosshairSprite)] public Sprite CrosshairSprite;
        [Tooltip(Tooltips.CrosshairColour)] public Color CrosshairColour = Color.white;

        [Tooltip(Tooltips.BarrelAudio)] public AudioClip FireAudio;
        [Tooltip(Tooltips.ReloadAudio)] public AudioClip ReloadAudio;
        [Tooltip(Tooltips.SwitchInAudio)] public AudioClip SwitchInAudio;
        [Tooltip(Tooltips.SwitchOutAudio)] public AudioClip SwitchOutAudio;

        [Tooltip(Tooltips.WeaponAnimatorController)] public RuntimeAnimatorController WeaponAnimatorController;
        [Tooltip(Tooltips.FireAnimationParameterName)] public string FireAnimationParameterName = "Firing";
        [Tooltip(Tooltips.ReloadAniamtionParameterName)] public string ReloadAniamtionParameterName = "Reloading";
        [Tooltip(Tooltips.SwitchAnimationParameterName)] public string SwitchAnimationParameterName = "Switching";
        [Tooltip(Tooltips.AimAnimationParameterName)] public string AimAnimationParameterName = "Aiming";
        [Tooltip(Tooltips.RunAnimationParameterName)] public string RunAnimationParameterName = "Running";
        [Tooltip(Tooltips.WalkAnimationParameterName)] public string WalkAnimationParameterName = "Walking";
        [Tooltip(Tooltips.JumpAnimationParameterName)] public string JumpAnimationParameterName = "Jumping";
        [Tooltip(Tooltips.MouseXAnimationParameterName)] public string MouseXAnimationParameterName = "Mouse X";
        [Tooltip(Tooltips.MouseYAnimationParameterName)] public string MouseYAnimationParameterName = "Mouse Y";

        // Sub-object referancig (for internal barralFlash, projectileSpawn, cartageSpawn)
        [Tooltip(Tooltips.BarrelFlashSpawnDirectory)] public string BarrelFlashSpawnDirectory;
        [Tooltip(Tooltips.ProjectileSpawnDirectory)] public string ProjectileSpawnDirectory;
        [Tooltip(Tooltips.CartridgeSpawnDirectory)] public string CartridgeSpawnDirectory;

        [Tooltip(Tooltips.OutputType)] public OutputType OutputTypeProperty = OutputType.Ray;

        [Tooltip(Tooltips.FiringType)] public FiringType FiringTypeProperty = FiringType.SemiAutomatic;
        [Tooltip(Tooltips.ShotsPerBurst)] public int ShotsPerBurst = 1;
        [Tooltip(Tooltips.ShotsPerSecond)] public float ShotsPerSecond = 10f;
        [Tooltip(Tooltips.OutputPerShot)] public int OutputPerShot = 1;

        // ? include damage tooltip
        [Tooltip(Tooltips.DamagePerShot)] public float DamagePerShot = 20f;

        [Tooltip(Tooltips.AimSpread)] public float AimSpread = 0.01f;
        [Tooltip(Tooltips.HipSpread)] public float HipSpread = 0.1f;
        [Tooltip(Tooltips.MovementSpread)] public float MovementSpread = 0.15f;

        [Tooltip(Tooltips.Ammo)] public Ammo AmmoProperty;
        [Tooltip(Tooltips.Capacity)] public int Capacity = 10;
        [Tooltip(Tooltips.AmmoLossPerShot)] public int AmmoLossPerShot = 1;
        [Tooltip(Tooltips.ReloadingType)] public ReloadingType ReloadingTypeProperty = ReloadingType.Full;
        [Tooltip(Tooltips.AmmoAddedPerReload)] public int AmmoAddedPerReload = 1;

        [Tooltip(Tooltips.AimingTransitionTime)] public float AimingTransitionTime = 0.25f;
        [Tooltip(Tooltips.ReloadingTime)] public float ReloadingTime = 1f;
        [Tooltip(Tooltips.SwitchingTime)] public float SwitchingTime = 1f;
        [Tooltip(Tooltips.RunningRecoveryTime)] public float RunningRecoveryTime = 0.25f;
        [Tooltip(Tooltips.IncrementalReloadRecoveryTime)] public float IncrementalReloadRecoveryTime = 0.25f;

        [System.Serializable]
        public class RayMode
        {
            [Tooltip(Tooltips.ImpactPrefab)] public GameObject ImpactPrefab;
            [Tooltip(Tooltips.Range)] public float Range = 100f;
            [Tooltip(Tooltips.ImpactForce)] public float ImpactForce = 100f;
        }

        [System.Serializable]
        public class ProjectileMode
        {
            [Tooltip(Tooltips.ProjectilePrefab)] public GameObject ProjectilePrefab;
            [Tooltip(Tooltips.LaunchForce)] public float LaunchForce = 100f;
        }

        [System.Serializable]
        public class EjectedCartridge
        {
            [Tooltip(Tooltips.CartridgePrefab)] public GameObject CartridgePrefab;
            [Tooltip(Tooltips.EjectionTrajectory)] public Vector3 EjectionTrajectory = new Vector3(1f, 0f, 0f);
            [Tooltip(Tooltips.EjectionForce)] public float EjectionForce = 5f;
        }

        public RayMode RayModeProperty;
        public ProjectileMode ProjectileModeProperty;
        public EjectedCartridge EjectedCartridgeProperty;


        public string FirstPersonLayerName { get; set; } = "FirstPerson";

        public bool IsDecollideWeaponPrefabNeeded()
        {
            if (WeaponPrefab == null)
            {
                return false;
            }

            if (WeaponPrefab.GetComponentsInChildren<Collider>().Length > 0)
            {
                return true;
            }

            return false;
        }

        public bool IsRelayerWeaponPrefabNeeded()
        {
            if (WeaponPrefab == null)
            {
                return false;
            }

            foreach (Transform t in WeaponPrefab.GetComponentsInChildren<Transform>())
            {
                if (t.gameObject.layer != LayerMask.NameToLayer(FirstPersonLayerName))
                {
                    return true;
                }
            }

            return false;
        }

        public void DecollideWeaponPrefab()
        {
            foreach (Collider c in WeaponPrefab.GetComponentsInChildren<Collider>())
            {
                // Collider components will be remove (from the inspector) after saving the project
                DestroyImmediate(c, true);
            }
        }

        public void RelayerWeaponPrefab()
        {
            foreach (Transform t in WeaponPrefab.GetComponentsInChildren<Transform>())
            {
                // GameObject layers will be updated (from the inspector) after saving the project
                if (t.gameObject.layer != LayerMask.NameToLayer(FirstPersonLayerName))
                {
                    t.gameObject.layer = LayerMask.NameToLayer(FirstPersonLayerName);
                }
            }
        }
    }
}