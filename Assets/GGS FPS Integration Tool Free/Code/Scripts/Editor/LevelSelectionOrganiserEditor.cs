using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(LevelSelectionOrganiser)), CanEditMultipleObjects]
    public class LevelSelectionOrganiserEditor : UnityEditor.Editor
    {
        ReorderableList _ReorderableList;

        public void OnEnable()
        {
            _ReorderableList = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("_SceneDetails"),
                true, true, true, true);

            _ReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                rect.y += 5f;

                float visableTickBoxYPosition = rect.y + 35;

                if (!element.FindPropertyRelative("IsFoldoutOpen").boolValue)
                {
                    visableTickBoxYPosition = rect.y + 10;
                }

                float[] scalableXPositions =
                {
                    rect.x,
                    rect.x + 30f,
                    rect.x + 92f,
                    rect.x + 63f + ((rect.width - 30f) * 0.5f)
                };

                float scalableFieldWidth = -3f + ((rect.width - 92f) * 0.5f);

                EditorGUI.PropertyField(
                    new Rect(scalableXPositions[0], visableTickBoxYPosition, 30, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("IsVisible"),
                    GUIContent.none
                    );

                EditorGUI.PropertyField(            // #
                    new Rect(scalableXPositions[1], rect.y, 58f, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("SceneIndex"),
                    GUIContent.none
                    );

                EditorGUI.PropertyField(
                    new Rect(scalableXPositions[2], rect.y, scalableFieldWidth, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Title"),
                    GUIContent.none
                    );

                EditorGUI.PropertyField(
                    new Rect(scalableXPositions[3], rect.y, scalableFieldWidth, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("ButtonSprite"),
                    GUIContent.none
                    );


                element.FindPropertyRelative("IsFoldoutOpen").boolValue = EditorGUI.Foldout(
                    new Rect(rect.x + 40, rect.y + 18, rect.width - 40, 20),
                    element.FindPropertyRelative("IsFoldoutOpen").boolValue,
                    "Description"
                    );

                if (element.FindPropertyRelative("IsFoldoutOpen").boolValue)
                {
                    element.FindPropertyRelative("Description").stringValue = EditorGUI.TextArea(
                        new Rect(rect.x + 42, rect.y + 38, rect.width - 42, 60),
                        element.FindPropertyRelative("Description").stringValue
                        );
                }

            };

            _ReorderableList.drawHeaderCallback = (Rect rect) =>
            {
                rect.x += 0f;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), "Visible");
                EditorGUI.LabelField(new Rect(rect.x + 25f + (rect.width - 30f) * 0.15f, rect.y, 60, EditorGUIUtility.singleLineHeight), "Scene");
                EditorGUI.LabelField(new Rect(rect.x + 25f + (rect.width - 30f) * 0.49f, rect.y, 60, EditorGUIUtility.singleLineHeight), "Title");
                EditorGUI.LabelField(new Rect(rect.x + 17f + (rect.width - 30f) * 0.83f, rect.y, 60, EditorGUIUtility.singleLineHeight), "Sprite");
            };

            _ReorderableList.elementHeightCallback = (int index) =>
            {
                SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                if (!element.FindPropertyRelative("IsFoldoutOpen").boolValue)
                {
                    return 45f;
                }

                return 110f;
            };

            _ReorderableList.onAddCallback = (ReorderableList list) => 
            {
                // Increment array size & select last element index
                int index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;

                SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                element.FindPropertyRelative("IsVisible").boolValue = true;
                element.FindPropertyRelative("SceneIndex").intValue = 1;
                element.FindPropertyRelative("Title").stringValue = "";
                element.FindPropertyRelative("Description").stringValue = "";
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_LevelButtons"));

            if (serializedObject.FindProperty("_LevelButtons").arraySize != 6)
            {
                serializedObject.FindProperty("_LevelButtons").arraySize = 6;
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_InfoMenuTitle"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_InfoMenuDescription"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_InfoMenuStartButton"));

            EditorGUILayout.Space();

            _ReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
