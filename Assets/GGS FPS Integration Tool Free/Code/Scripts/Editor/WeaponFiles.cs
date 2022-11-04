using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using UnityEditor.Animations;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    public class WeaponFiles : EditorWindow
    {
        static WeaponFiles window;
        static string weaponName = "";

        const string DefaultWeaponFolderPathInside = "Assets/GGS FPS Integration Tool Free/Default Weapon/";

        [MenuItem("Assets/Create/Weapon Files", false, 0)]
        static void Initialise()
        {
            window = (WeaponFiles)GetWindow(typeof(WeaponFiles), true, "Create Weapon Files");
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 65);

            weaponName = "";
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Weapon name:");
            weaponName = EditorGUILayout.TextField(weaponName);

            if (weaponName.Length > 0 && weaponName[0] != ' ' && weaponName[weaponName.Length - 1] != ' ')
            {
                if (GUILayout.Button("Create"))
                {
                    CreateWeaponFiles();

                    window.Close();
                }
            }
        }

        void CreateWeaponFiles()
        {
            // Finding path of new folder location
            string folderPathOutside = AssetDatabase.GetAssetPath(Selection.activeObject);
            const string defaultPathOutside = "Assets";

            // Prevents 'Assets > Create > Weapon Files' from causing errors,
            // by applying new folder in Assets path by default
            if (folderPathOutside == "")
            {
                folderPathOutside = defaultPathOutside;
            }

            // Create folder
            AssetDatabase.CreateFolder(folderPathOutside, weaponName);

            // Create files
            string folderPathInside = folderPathOutside + "/" + weaponName + "/";
            AssetDatabase.CopyAsset(DefaultWeaponFolderPathInside + "Default Weapon.asset", folderPathInside + weaponName + ".asset");
            AssetDatabase.CopyAsset(DefaultWeaponFolderPathInside + "Default Weapon.controller", folderPathInside + weaponName + ".controller");
            AssetDatabase.CopyAsset(DefaultWeaponFolderPathInside + "Default Weapon.prefab", folderPathInside + weaponName + ".prefab");
            AssetDatabase.CopyAsset(DefaultWeaponFolderPathInside + "Animations", folderPathInside + "Animations");

            // Rename animations
            string animationFolderPathInside = folderPathInside + "Animations/";
            AssetDatabase.RenameAsset(animationFolderPathInside + "Default Weapon Aim.anim", weaponName + " Aim");
            AssetDatabase.RenameAsset(animationFolderPathInside + "Default Weapon Fire.anim", weaponName + " Fire");
            AssetDatabase.RenameAsset(animationFolderPathInside + "Default Weapon Hip.anim", weaponName + " Hip");
            AssetDatabase.RenameAsset(animationFolderPathInside + "Default Weapon Idle.anim", weaponName + " Idle");
            AssetDatabase.RenameAsset(animationFolderPathInside + "Default Weapon Reload.anim", weaponName + " Reload");
            AssetDatabase.RenameAsset(animationFolderPathInside + "Default Weapon Incremental Reload.anim", weaponName + " Incremental Reload");
            AssetDatabase.RenameAsset(animationFolderPathInside + "Default Weapon Run.anim", weaponName + " Run");
            AssetDatabase.RenameAsset(animationFolderPathInside + "Default Weapon Switch.anim", weaponName + " Switch");

            // Apply animations within Animator Controller
            AnimatorController animatorController = (AnimatorController)AssetDatabase.LoadAssetAtPath(folderPathInside + weaponName + ".controller", typeof(AnimatorController));

            AnimatorStateMachine animatorStateMachine = animatorController.layers[0].stateMachine;
            ChildAnimatorState[] childAnimatorStates = animatorStateMachine.states;

            AnimatorState state;
            string stateName;

            AnimationClip
                animationClipIdle = (AnimationClip)AssetDatabase.LoadAssetAtPath(animationFolderPathInside + weaponName + " Idle.anim", typeof(AnimationClip)),
                animationClipFire = (AnimationClip)AssetDatabase.LoadAssetAtPath(animationFolderPathInside + weaponName + " Fire.anim", typeof(AnimationClip)),
                animationClipReload = (AnimationClip)AssetDatabase.LoadAssetAtPath(animationFolderPathInside + weaponName + " Reload.anim", typeof(AnimationClip)),
                animationClipIncrementalReload = (AnimationClip)AssetDatabase.LoadAssetAtPath(animationFolderPathInside + weaponName + " Incremental Reload.anim", typeof(AnimationClip)),
                animationClipRun = (AnimationClip)AssetDatabase.LoadAssetAtPath(animationFolderPathInside + weaponName + " Run.anim", typeof(AnimationClip)),
                animationClipSwitch = (AnimationClip)AssetDatabase.LoadAssetAtPath(animationFolderPathInside + weaponName + " Switch.anim", typeof(AnimationClip)),
                animationClipAim = (AnimationClip)AssetDatabase.LoadAssetAtPath(animationFolderPathInside + weaponName + " Aim.anim", typeof(AnimationClip)),
                animationClipHip = (AnimationClip)AssetDatabase.LoadAssetAtPath(animationFolderPathInside + weaponName + " Hip.anim", typeof(AnimationClip));

            // Base layer
            for (int i = 0; i < childAnimatorStates.Length; i++)
            {
                state = childAnimatorStates[i].state;
                stateName = state.name;

                if (stateName == "Idle")
                {
                    animatorController.SetStateEffectiveMotion(state, animationClipIdle);
                }
                else if (stateName == "Fire")
                {
                    animatorController.SetStateEffectiveMotion(state, animationClipFire);
                }
                else if (stateName == "Reload")
                {
                    animatorController.SetStateEffectiveMotion(state, animationClipReload);
                }
                else if (stateName == "Run")
                {
                    animatorController.SetStateEffectiveMotion(state, animationClipRun);
                }
                else if (stateName == "Switch In")
                {
                    animatorController.SetStateEffectiveMotion(state, animationClipSwitch);
                }
                else if (stateName == "Switch Out")
                {
                    animatorController.SetStateEffectiveMotion(state, animationClipSwitch);
                }
            }

            animatorStateMachine = animatorController.layers[1].stateMachine;
            childAnimatorStates = animatorStateMachine.states;

            // Aim Influance layer
            for (int i = 0; i < childAnimatorStates.Length; i++)
            {
                state = childAnimatorStates[i].state;
                stateName = state.name;

                if (stateName == "Aim")
                {
                    animatorController.SetStateEffectiveMotion(state, animationClipAim);
                }
                else if (stateName == "Hip")
                {
                    animatorController.SetStateEffectiveMotion(state, animationClipHip);
                }
            }

            // Rename animation properties
            RenameAnimationProperties(animationClipIdle, weaponName);
            RenameAnimationProperties(animationClipFire, weaponName);
            RenameAnimationProperties(animationClipReload, weaponName);
            RenameAnimationProperties(animationClipIncrementalReload, weaponName);
            RenameAnimationProperties(animationClipRun, weaponName);
            RenameAnimationProperties(animationClipSwitch, weaponName);
            RenameAnimationProperties(animationClipAim, weaponName + "/Aimbody");
            RenameAnimationProperties(animationClipHip, weaponName + "/Aimbody");


            // Apply dependentcies to Weapon object
            Weapon weaponObject = (Weapon)AssetDatabase.LoadAssetAtPath(folderPathInside + weaponName + ".asset", typeof(Weapon));

            weaponObject.WeaponPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(folderPathInside + weaponName + ".prefab", typeof(GameObject));
            weaponObject.WeaponAnimatorController = animatorController;

            weaponObject.BarrelFlashSpawnDirectory = weaponName + "/Aimbody/[Flash]";
            weaponObject.ProjectileSpawnDirectory = weaponName + "/Aimbody/[Pro]";
            weaponObject.CartridgeSpawnDirectory = weaponName + "/Aimbody/[Cart]";

            // Prevents the Weapon SO of 'weaponObject' from reseting (to entries of the Default Weapon) after closing Unity.
            EditorUtility.SetDirty(weaponObject);
        }

        void RenameAnimationProperties(AnimationClip clip, string name)
        {
            EditorCurveBinding[] currentBindings;
            AnimationCurve currentCurve;

            currentBindings = AnimationUtility.GetCurveBindings(clip);

            for (int i = 0; i < currentBindings.Length; i++)
            {
                currentCurve = AnimationUtility.GetEditorCurve(clip, currentBindings[i]);
                AnimationUtility.SetEditorCurve(clip, currentBindings[i], null);

                currentBindings[i].path = name;
                AnimationUtility.SetEditorCurve(clip, currentBindings[i], currentCurve);
            }
        }
    }
}