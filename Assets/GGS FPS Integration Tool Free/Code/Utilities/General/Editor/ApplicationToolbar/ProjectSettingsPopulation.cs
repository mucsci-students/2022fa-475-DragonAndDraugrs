using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GGSFPSIntegrationTool.Utilities.General.Editor.ApplicationToolbar
{
    class AxisData
    {
        public string Name { get; set; } = "";
        public string DescriptiveName { get; set; } = "";
        public string DescriptiveNegativeName { get; set; } = "";

        public string NegativeButton { get; set; } = "";
        public string PositiveButton { get; set; } = "";
        public string AltNegativeButton { get; set; } = "";
        public string AltPositiveButton { get; set; } = "";

        public float Gravity { get; set; } = 0;
        public float Dead { get; set; } = 0;
        public float Sensitivity { get; set; } = 0;

        public bool Snap { get; set; } = false;
        public bool Invert { get; set; } = false;

        public byte Type { get; set; } = 0;
        public byte Axis { get; set; } = 0;
        public byte JoyNum { get; set; } = 0;
    }

    public static class ProjectSettingsPopulation
    {
        const string
            _PathStart = "Tools/FPS Integration/Populate Project Settings/",
            _AllMenuItemPath = _PathStart + "All",
            _TagsAndLayersMenuItemPath = _PathStart + "Tags and Layers",
            _InputManagerMenuItemPath = _PathStart + "Input";

        static string TagsAndLayersPath { get; } = "ProjectSettings/TagManager.asset";
        static string InputManagerPath { get; } = "ProjectSettings/InputManager.asset";

        static bool IsAllOptionSelected { get; set; } = false;

        [MenuItem(_AllMenuItemPath, priority = 1)]
        public static void PopulateAll()
        {
            const string
                title = "Populate Project Settings?",
                text = "This will replace elements of the Project Settings 'Tags and Layers' and 'Input Manager' " +
                "to allow the FPS Integration Tool to function correctly.";

            if (!IsDisplayedDialogAccepted(title, text))
            {
                return;
            }

            IsAllOptionSelected = true;

            PopulateTagsAndLayers();
            PopulateInputManager();

            IsAllOptionSelected = false;

            // Updates Project Settings immediately when applying settings
            AssetDatabase.Refresh();

            Utilities.General.Console.Logger.LogMessage<Component>(
                "Project Settings have been successfully updated."
                );
        }

        [MenuItem(_TagsAndLayersMenuItemPath, priority = 2)]
        public static void PopulateTagsAndLayers()
        {
            if (!IsAllOptionSelected)
            {
                const string
                title = "Populate Tags and Layers of Project Settings?",
                text = "This will replace elements of the Project Settings 'Tags and Layers' " +
                "to allow the FPS Integration Tool to function correctly.";

                if (!IsDisplayedDialogAccepted(title, text))
                {
                    return;
                }
            }
            
            SerializedObject tagsAndLayersManager = 
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(TagsAndLayersPath)[0]);
            
            ApplyTags(tagsAndLayersManager);
            ApplyLayers(tagsAndLayersManager);

            if (!IsAllOptionSelected)
            {
                // Updates Project Settings immediately when applying settings
                AssetDatabase.Refresh();

                Utilities.General.Console.Logger.LogMessage<Component>(
                    "Project Settings 'Tags and Layers' have been successfully updated."
                    );
            }
        }

        [MenuItem(_InputManagerMenuItemPath, priority = 3)]
        public static void PopulateInputManager()
        {
            if (!IsAllOptionSelected)
            {
                const string
                title = "Populate Input Manager of Project Settings?",
                text = "This will replace elements of the Project Settings 'Input Manager' " +
                "to allow the FPS Integration Tool to function correctly.";

                if (!IsDisplayedDialogAccepted(title, text))
                {
                    return;
                }
            }

            ApplyInput(new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(InputManagerPath)[0]));

            if (!IsAllOptionSelected)
            {
                // Updates Project Settings immediately when applying settings
                AssetDatabase.Refresh();

                Utilities.General.Console.Logger.LogMessage<Component>(
                    "Project Settings 'Input Manager' has been successfully updated."
                    );
            }
        }


        static void ApplyTags(SerializedObject tagManager)
        {
            string[] tagValues =
            {
                "Loose",
                "DeathBody"
            };

            SerializedProperty tags = tagManager.FindProperty("tags");

            for (int i = 0; i < tagValues.Length; i++)
            {
                if (i == tags.arraySize)
                {
                    tags.InsertArrayElementAtIndex(i);
                }

                if (tags.GetArrayElementAtIndex(i).stringValue != tagValues[i])
                {
                    tags.GetArrayElementAtIndex(i).stringValue = tagValues[i];
                }
            }

            tagManager.ApplyModifiedProperties();
        }

        static void ApplyLayers(SerializedObject layerManager)
        {
            const byte userLayerStartIndex = 8;

            string[] layerValues =
            {
                "FirstPerson",
                "ThirdPerson",
                "Projectile",
                "NonPlayer",
                "EffectToPlayer",
                "EffectFromPlayer",
                "ThirdPersonBarrier"
            };

            SerializedProperty layers = layerManager.FindProperty("layers");

            for (int i = 0; i < layerValues.Length; i++)
            {
                layers.GetArrayElementAtIndex(i + userLayerStartIndex).stringValue = layerValues[i];
            };

            layerManager.ApplyModifiedProperties();
        }

        static void ApplyInput(SerializedObject inputManager)
        {
            const short elementsToUpdateCount = 26;

            SerializedProperty axes = inputManager.FindProperty("m_Axes");

            if (axes.arraySize < elementsToUpdateCount)
            {
                axes.arraySize = elementsToUpdateCount;
            }

            AxisData[] axisData = new AxisData[elementsToUpdateCount];

            for (short i = 0; i < axisData.Length; i++)
            {
                axisData[i] = new AxisData();
            }

            axisData[0].Name = "Horizontal";
            axisData[0].NegativeButton = "left";
            axisData[0].PositiveButton = "right";
            axisData[0].AltNegativeButton = "a";
            axisData[0].AltPositiveButton = "d";
            axisData[0].Gravity = 3;
            axisData[0].Sensitivity = 3;
            axisData[0].Snap = true;

            axisData[1].Name = "Vertical";
            axisData[1].NegativeButton = "down";
            axisData[1].PositiveButton = "up";
            axisData[1].AltNegativeButton = "s";
            axisData[1].AltPositiveButton = "w";
            axisData[1].Gravity = 3;
            axisData[1].Sensitivity = 3;
            axisData[1].Snap = true;

            axisData[2].Name = "Fire";
            axisData[2].PositiveButton = "mouse 0";

            axisData[3].Name = "Mouse X";
            axisData[3].Sensitivity = 0.1f;
            axisData[3].Type = 1;

            axisData[4].Name = "Mouse Y";
            axisData[4].Sensitivity = 0.1f;
            axisData[4].Type = 1;
            axisData[4].Axis = 1;

            axisData[5].Name = "Mouse ScrollWheel";
            axisData[5].Sensitivity = 0.1f;
            axisData[5].Type = 1;
            axisData[5].Axis = 2;

            axisData[6].Name = "Mouse X Influence";
            axisData[6].Sensitivity = 0.01f;
            axisData[6].Type = 1;

            axisData[7].Name = "Mouse Y Influence";
            axisData[7].Sensitivity = 0.01f;
            axisData[7].Type = 1;
            axisData[7].Axis = 1;

            axisData[8].Name = "Jump";
            axisData[8].PositiveButton = "space";

            axisData[9].Name = "Run";
            axisData[9].PositiveButton = "left shift";
            axisData[9].AltPositiveButton = "right shift";

            axisData[10].Name = "Reload";
            axisData[10].PositiveButton = "r";

            axisData[11].Name = "Switch";
            axisData[11].PositiveButton = "q";

            axisData[12].Name = "Aim";
            axisData[12].PositiveButton = "mouse 1";

            axisData[13].Name = "Select Weapon 1";
            axisData[13].PositiveButton = "1";

            axisData[14].Name = "Select Weapon 2";
            axisData[14].PositiveButton = "2";

            axisData[15].Name = "Select Weapon 3";
            axisData[15].PositiveButton = "3";

            axisData[16].Name = "Select Weapon 4";
            axisData[16].PositiveButton = "4";

            axisData[17].Name = "Select Weapon 5";
            axisData[17].PositiveButton = "5";

            axisData[18].Name = "Select Weapon 6";
            axisData[18].PositiveButton = "6";

            axisData[19].Name = "Select Weapon 7";
            axisData[19].PositiveButton = "7";

            axisData[20].Name = "Select Weapon 8";
            axisData[20].PositiveButton = "8";

            axisData[21].Name = "Select Weapon 9";
            axisData[21].PositiveButton = "9";

            axisData[22].Name = "Select Weapon 10";
            axisData[22].PositiveButton = "0";

            axisData[23].Name = "Toggle Pause";
            axisData[23].PositiveButton = "escape";
            axisData[23].AltPositiveButton = "p";

            axisData[24].Name = "Submit";
            axisData[24].PositiveButton = "enter";
            axisData[24].AltPositiveButton = "return";

            axisData[25].Name = "Cancel";
            axisData[25].PositiveButton = "escape";

            SerializedProperty axis;
            for (short i = 0; i < axisData.Length; i++)
            {
                axis = axes.GetArrayElementAtIndex(i);

                axis.FindPropertyRelative("m_Name").stringValue = axisData[i].Name;
                axis.FindPropertyRelative("descriptiveName").stringValue = axisData[i].DescriptiveName;
                axis.FindPropertyRelative("descriptiveNegativeName").stringValue = axisData[i].DescriptiveNegativeName;

                axis.FindPropertyRelative("negativeButton").stringValue = axisData[i].NegativeButton;
                axis.FindPropertyRelative("positiveButton").stringValue = axisData[i].PositiveButton;
                axis.FindPropertyRelative("altNegativeButton").stringValue = axisData[i].AltNegativeButton;
                axis.FindPropertyRelative("altPositiveButton").stringValue = axisData[i].AltPositiveButton;

                axis.FindPropertyRelative("gravity").floatValue = axisData[i].Gravity;
                axis.FindPropertyRelative("dead").floatValue = axisData[i].Dead;
                axis.FindPropertyRelative("sensitivity").floatValue = axisData[i].Sensitivity;

                axis.FindPropertyRelative("snap").boolValue = axisData[i].Snap;
                axis.FindPropertyRelative("invert").boolValue = axisData[i].Invert;

                axis.FindPropertyRelative("type").intValue = axisData[i].Type;
                axis.FindPropertyRelative("axis").intValue = axisData[i].Axis;
                axis.FindPropertyRelative("joyNum").intValue = axisData[i].JoyNum;
            }

            inputManager.ApplyModifiedProperties();
        }

        static bool IsDisplayedDialogAccepted(string dialogTitle, string dialogText)
        {
            const string
                buttonText = "Apply",
                cancelButtonText = "Cancel";

            if (EditorUtility.DisplayDialog(dialogTitle, dialogText, buttonText, cancelButtonText))
            {
                return true;
            }

            return false;
        }

    }
}