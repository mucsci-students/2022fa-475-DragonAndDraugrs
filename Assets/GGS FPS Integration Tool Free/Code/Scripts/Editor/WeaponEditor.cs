using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(Weapon)), CanEditMultipleObjects]
    public class WeaponEditor : UnityEditor.Editor
    {
        // For calling functions in Weapon class
        Weapon _EditorTarget;
        WeaponSystems _WeaponSystems;
        Transform _WeaponSpaceTransform;

        void OnEnable()
        {
            _EditorTarget = (Weapon)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _WeaponSystems = FindObjectOfType<WeaponSystems>();

            // Prevents error when accessing Weapon SOs while in scenes without a WeaponSystems component
            if (_WeaponSystems != null)
            {
                _WeaponSpaceTransform = _WeaponSystems.transform.GetChild(0);
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("WeaponPrefab"));

            float currentHelpBoxPosition = 34f;

            if (_EditorTarget.IsDecollideWeaponPrefabNeeded())
            {
                DecollideHelpBox(ref currentHelpBoxPosition);
            }

            if (_EditorTarget.IsRelayerWeaponPrefabNeeded())
            {
                RelayerHelpBox(ref currentHelpBoxPosition);
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("BarrelFlashPrefab"));

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("IsPositionOffset"));
            if (serializedObject.FindProperty("IsPositionOffset").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("PositionOffset"));
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Crosshair", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CrosshairSprite"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CrosshairColour"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FireAudio"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ReloadAudio"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SwitchInAudio"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SwitchOutAudio"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();

                GUILayoutOption labelGUIOption = GUILayout.Width(EditorGUIUtility.labelWidth - 1);
                GUILayoutOption buttonGUIOption = GUILayout.Width(EditorGUIUtility.currentViewWidth - (EditorGUIUtility.labelWidth + 38));

                EditorGUILayout.LabelField("Animation Mode", labelGUIOption);

                if (!IsAnimationMode())
                {
                    if (GUILayout.Button("Enable", buttonGUIOption))
                    {
                        EnableAnimationMode();
                    }
                }
                else
                {
                    if (GUILayout.Button("Disable", buttonGUIOption))
                    {
                        DisableAnimationMode();
                    }
                }

            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("WeaponAnimatorController"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Spawn Paths", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("BarrelFlashSpawnDirectory"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ProjectileSpawnDirectory"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CartridgeSpawnDirectory"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Attributes", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OutputTypeProperty"), new GUIContent("Output Type"));

            // Ray Mode
            if (serializedObject.FindProperty("OutputTypeProperty").enumValueIndex == (int)Weapon.OutputType.Ray)
            {
                SerializedProperty rayModeProperty = serializedObject.FindProperty("RayModeProperty");
                EditorGUILayout.PropertyField(rayModeProperty.FindPropertyRelative("ImpactPrefab"));
                EditorGUILayout.PropertyField(rayModeProperty.FindPropertyRelative("Range"));
                EditorGUILayout.PropertyField(rayModeProperty.FindPropertyRelative("ImpactForce"));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("DamagePerShot"));
            }

            // Projectile Mode
            if (serializedObject.FindProperty("OutputTypeProperty").enumValueIndex == (int)Weapon.OutputType.Projectile)
            {
                SerializedProperty projectileModeProperty = serializedObject.FindProperty("ProjectileModeProperty");
                EditorGUILayout.PropertyField(projectileModeProperty.FindPropertyRelative("ProjectilePrefab"));
                EditorGUILayout.PropertyField(projectileModeProperty.FindPropertyRelative("LaunchForce"));
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("FiringTypeProperty"), new GUIContent("Firing Type"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ShotsPerBurst"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ShotsPerSecond"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OutputPerShot"));

            EditorGUILayout.Space();

            serializedObject.FindProperty("AimSpread").floatValue = EditorGUILayout.Slider("Aiming Spread", serializedObject.FindProperty("AimSpread").floatValue, 0f, 0.3f);
            serializedObject.FindProperty("HipSpread").floatValue = EditorGUILayout.Slider("Hip Spread", serializedObject.FindProperty("HipSpread").floatValue, 0f, 0.3f);
            serializedObject.FindProperty("MovementSpread").floatValue = EditorGUILayout.Slider("Movement Spread", serializedObject.FindProperty("MovementSpread").floatValue, 0f, 0.3f);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("AmmoProperty"), new GUIContent("Ammo"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Capacity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AmmoLossPerShot"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("ReloadingTypeProperty"), new GUIContent("Reloading Type"));
            if (
                serializedObject.FindProperty("ReloadingTypeProperty").enumValueIndex == (int)Weapon.ReloadingType.Partial ||
                serializedObject.FindProperty("ReloadingTypeProperty").enumValueIndex == (int)Weapon.ReloadingType.PartialRepeat
                )
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AmmoAddedPerReload"));
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Animation Timing", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AimingTransitionTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ReloadingTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SwitchingTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RunningRecoveryTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("IncrementalReloadRecoveryTime"));
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Cartridge Ejection", EditorStyles.boldLabel);
            SerializedProperty ejectedCartridgeProperty = serializedObject.FindProperty("EjectedCartridgeProperty");
            EditorGUILayout.PropertyField(ejectedCartridgeProperty.FindPropertyRelative("CartridgePrefab"));
            EditorGUILayout.PropertyField(ejectedCartridgeProperty.FindPropertyRelative("EjectionTrajectory"));
            EditorGUILayout.PropertyField(ejectedCartridgeProperty.FindPropertyRelative("EjectionForce"));

            serializedObject.ApplyModifiedProperties();
        }

        void DecollideHelpBox(ref float inspectorPosition)
        {
            float height;

            if (EditorGUIUtility.currentViewWidth <= 373f) { height = 50f; }
            else { height = 50f; }

            float buttonY = inspectorPosition + height;

            string      // ? should be const?
                buttonText = "Remove Colliders",
                cancelButtonText = "Cancel",
                dialogTitle = "Remove Colliders from Weapon Prefab?",
                dialogText = "This will remove colliders from the Weapon Prefab and its children. " +
                "Changes may only be noticeable after saving the project.",
                helpBoxText = "Remove Colliders from Weapon Prefab to prevent raycast firing disruption.";

            if (GUI.Button(new Rect(25f, buttonY, EditorGUIUtility.currentViewWidth - 48f, 20f), buttonText))
            {
                if (EditorUtility.DisplayDialog(dialogTitle, dialogText, buttonText, cancelButtonText))
                {
                    _EditorTarget.DecollideWeaponPrefab();
                }
            }

            EditorGUI.HelpBox(new Rect(18f, inspectorPosition, EditorGUIUtility.currentViewWidth - 35f, height + 25f), helpBoxText, MessageType.Warning);

            inspectorPosition += height + 30f;
            EditorGUILayout.Space(height + 30f);
        }

        void RelayerHelpBox(ref float inspectorPosition)
        {
            float height;

            if (EditorGUIUtility.currentViewWidth <= 373f) { height = 50f; }
            else { height = 50f; }

            float buttonY = inspectorPosition + height;

            string
                buttonText = "Resolve Layers",
                cancelButtonText = "Cancel",
                dialogTitle = "Resolve Layers of Weapon Prefab?",
                dialogText = "This will assign the Weapon Prefab and its children to the FirstPerson layer. " +
                "Changes may only be noticeable after saving the project.",
                helpBoxText = "Assign Weapon Prefab to the FirstPerson layer to prevent rendering issues.";

            if (GUI.Button(new Rect(25f, buttonY, EditorGUIUtility.currentViewWidth - 48f, 20f), buttonText))
            {
                if (EditorUtility.DisplayDialog(dialogTitle, dialogText, buttonText, cancelButtonText))
                {
                    _EditorTarget.RelayerWeaponPrefab();
                }
            }

            EditorGUI.HelpBox(new Rect(18f, inspectorPosition, EditorGUIUtility.currentViewWidth - 35f, height + 25f), helpBoxText, MessageType.Warning);

            inspectorPosition += height + 30f;
            EditorGUILayout.Space(height + 30f);
        }

        void EnableAnimationMode()
        {
            Utilities.General.ErrorChecking.ObjectChecker.ThrowIfNull<Component>(
                "WeaponSpaceTransform",
                _WeaponSpaceTransform,
                "Cannot toggle Animation Modes in scenes without a WeaponSystems component.",
                this
                );

            // Remove other instantiated weapon prefabs first
            DisableAnimationMode();

            GameObject instantiatedObject = Instantiate(_EditorTarget.WeaponPrefab, _WeaponSpaceTransform);
            _WeaponSpaceTransform.GetComponent<Animator>().runtimeAnimatorController = _EditorTarget.WeaponAnimatorController;

            // Removes "(clone)" from name of spawned weapon object, to prevent animations from disconnecting with object
            const int numberOfCharacterToRemove = 7;
            instantiatedObject.name = instantiatedObject.name.Remove(instantiatedObject.name.Length - numberOfCharacterToRemove); 
        }

        void DisableAnimationMode()
        {
            Utilities.General.ErrorChecking.ObjectChecker.ThrowIfNull<Component>(
                "WeaponSpaceTransform",
                _WeaponSpaceTransform,
                "Cannot toggle Animation Modes in scenes without a WeaponSystems component.",
                this
                );

            for (int i = 0; i < _WeaponSpaceTransform.transform.childCount; i++)
            {
                DestroyImmediate(_WeaponSpaceTransform.transform.GetChild(i).gameObject);
            }

            _WeaponSpaceTransform.GetComponent<Animator>().runtimeAnimatorController = null;
        }

        bool IsAnimationMode()
        {
            if (_WeaponSpaceTransform == null)
            {
                return false;
            }

            if (_WeaponSpaceTransform.transform.childCount != 1)
            {
                return false;
            }

            if (_WeaponSpaceTransform.transform.GetChild(0).name != _EditorTarget.name)
            {
                return false;
            }

            if (_WeaponSpaceTransform.GetComponent<Animator>().runtimeAnimatorController != _EditorTarget.WeaponAnimatorController)
            {
                return false;
            }

            return true;
        }
    }
}