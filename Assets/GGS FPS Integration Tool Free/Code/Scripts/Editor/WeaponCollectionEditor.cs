using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(WeaponCollection)), CanEditMultipleObjects]
    public class WeaponCollectionEditor : UnityEditor.Editor
    {
        ReorderableList _ReorderableList;

        public void OnEnable()
        {
            _ReorderableList = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("WeaponDetailsList"),
                true, true, true, true);

            _ReorderableList.elementHeight = 24f;

            _ReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                rect.y += 3f;

                float[] scalableXPositions =
                {
                    rect.x,
                    rect.x + (rect.width * 0.8f),
                    rect.x + (rect.width * 0.92f)
                };

                float scalableWeaponFieldWidth = rect.width * 0.7f;
                float scalableBoolFieldWidth = 20f;

                EditorGUI.PropertyField(
                    new Rect(scalableXPositions[0], rect.y, scalableWeaponFieldWidth, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("WeaponScriptableObject"),
                    GUIContent.none
                    );

                BeginControlIsEnabledOnStartField(index);
                
                    EditorGUI.PropertyField(
                        new Rect(scalableXPositions[1], rect.y, scalableBoolFieldWidth, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("IsEnabledOnStart"),
                        GUIContent.none
                        );

                EndControlIsEnabledOnStartField();

                if (element.FindPropertyRelative("IsEnabledOnStart").boolValue)
                {
                    EditorGUI.PropertyField(
                        new Rect(scalableXPositions[2], rect.y, scalableBoolFieldWidth, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("IsLoadedOnStart"),
                        GUIContent.none
                        );
                }
            };

            _ReorderableList.drawHeaderCallback = (Rect rect) =>
            {
                float scalableHeaderWidth = rect.width * 0.1f;

                EditorGUI.LabelField(new Rect(rect.x + 15f, rect.y, 60f, EditorGUIUtility.singleLineHeight), "Weapon");
                EditorGUI.LabelField(new Rect(rect.x + -22f + (rect.width * 0.8f), rect.y, rect.width * 0.1f, EditorGUIUtility.singleLineHeight), "Is Enabled");
                EditorGUI.LabelField(new Rect(rect.x + -25f + (rect.width * 0.92f), rect.y, rect.width * 0.175f, EditorGUIUtility.singleLineHeight), "Is Loaded");
            };

            _ReorderableList.onAddCallback = (ReorderableList list) =>
            {
                // Increment array size & select last element index
                int index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;

                SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                
                element.FindPropertyRelative("WeaponScriptableObject").objectReferenceValue = null;
                element.FindPropertyRelative("IsEnabledOnStart").boolValue = true;
                element.FindPropertyRelative("IsLoadedOnStart").boolValue = true;
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _ReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        void BeginControlIsEnabledOnStartField(int index)
        {
            SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            bool[] IsEnabledArray = new bool[_ReorderableList.serializedProperty.arraySize];

            // Get all IsEnabledOnStart variables
            for (short i = 0; i < _ReorderableList.serializedProperty.arraySize; i++)
            {
                IsEnabledArray[i] =
                _ReorderableList.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("IsEnabledOnStart").boolValue;
            }

            // Change IsEnabledOnStart of first element to true if all others are false
            if (index == 0 && Utilities.General.Array.BoolChecker.AreAllOfState(false, IsEnabledArray))
            {
                element.FindPropertyRelative("IsEnabledOnStart").boolValue = true;
            }

            // If the first IsEnabledOnStart is the only one that's true, disable it (grey out)
            if (index == 0 && IsEnabledArray[index] && Utilities.General.Array.BoolChecker.NumberOfStates(true, IsEnabledArray) == 1)
            {
                GUI.enabled = false;
            }
        }

        void EndControlIsEnabledOnStartField()
        {
            // Finish field disabling
            if (!GUI.enabled)
            {
                GUI.enabled = true;
            }
        }
    }
}