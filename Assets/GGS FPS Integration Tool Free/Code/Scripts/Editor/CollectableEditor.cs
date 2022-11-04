using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(Collectable)), CanEditMultipleObjects]
    public class CollectableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_CollectionType"));

            // Weapon
            if (serializedObject.FindProperty("_CollectionType").enumValueIndex == (int)Collectable.CollectionType.Weapon)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_Weapon"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_Enable"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_AmmoInWeapon"));
            }

            // Ammo
            if (serializedObject.FindProperty("_CollectionType").enumValueIndex == (int)Collectable.CollectionType.Ammo)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_Ammo"));
            }

            // Weapon / Ammo
            if (
                serializedObject.FindProperty("_CollectionType").enumValueIndex == (int)Collectable.CollectionType.Weapon
                ||
                serializedObject.FindProperty("_CollectionType").enumValueIndex == (int)Collectable.CollectionType.Ammo
                )
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_AddToAmmoTotal"));
            }

            // Health
            if (serializedObject.FindProperty("_CollectionType").enumValueIndex == (int)Collectable.CollectionType.Health)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_HealthToAdd"));
            }

            // Lives
            if (serializedObject.FindProperty("_CollectionType").enumValueIndex == (int)Collectable.CollectionType.Lives)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_LivesToAdd"));
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_DespawnType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_AfterCollectionObject"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_AfterCollectionDespawnTime"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}