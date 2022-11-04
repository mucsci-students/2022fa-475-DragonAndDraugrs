using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GGSFPSIntegrationTool.Scripts.Editor
{   
    [CustomEditor(typeof(Health)), CanEditMultipleObjects]
    public class HealthEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_MaximumHealth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_MinimumHealth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_StartHealth"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Lives", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_HasLives"));
            if (serializedObject.FindProperty("_HasLives").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_MaximumLives"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_StartLives"));
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnDamage"));
            if (!serializedObject.FindProperty("_HasLives").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnDeath"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnDeathWithLives"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnDeathWithoutLives"));
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    
}
