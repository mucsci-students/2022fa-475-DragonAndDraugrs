using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(TotalAmmoManager)), CanEditMultipleObjects]
    public class TotalAmmoManagerEditor : UnityEditor.Editor
    {
        SerializedProperty _NewTotalAmmoCounts;
        int _LastNewTotalAmmoCountsSize = 0;

        void OnEnable()
        {
            _NewTotalAmmoCounts = serializedObject.FindProperty("NewTotalAmmoCounts");
            _LastNewTotalAmmoCountsSize = _NewTotalAmmoCounts.arraySize;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Initialises the variables of the new array elements
            if (_NewTotalAmmoCounts.arraySize > _LastNewTotalAmmoCountsSize)
            {
                for (int i = _LastNewTotalAmmoCountsSize; i < _NewTotalAmmoCounts.arraySize; i++)
                {
                    _NewTotalAmmoCounts.GetArrayElementAtIndex(i).FindPropertyRelative("AmmoType").objectReferenceValue = null;
                    _NewTotalAmmoCounts.GetArrayElementAtIndex(i).FindPropertyRelative("Count").intValue = 0;
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_CharacterWeaponSystem"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_WillAmmoCountsSetOnStart"));

            EditorGUILayout.Space();

            // Appy this before the Size field prevents array index out-of-range errors
            _LastNewTotalAmmoCountsSize = _NewTotalAmmoCounts.arraySize;

            // Size field
            EditorGUILayout.PropertyField(_NewTotalAmmoCounts.FindPropertyRelative("Array.size"));

            // Generate the fields of Threshold elements
            for (int i = 0; i < _NewTotalAmmoCounts.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_NewTotalAmmoCounts.GetArrayElementAtIndex(i).FindPropertyRelative("AmmoType"), GUIContent.none);
                EditorGUILayout.PropertyField(_NewTotalAmmoCounts.GetArrayElementAtIndex(i).FindPropertyRelative("Count"), GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

