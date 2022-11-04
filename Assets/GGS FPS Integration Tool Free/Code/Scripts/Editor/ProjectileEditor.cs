using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(Projectile)), CanEditMultipleObjects]
    public class ProjectileEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_CollisionAction"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Lifetimes", EditorStyles.boldLabel);
            if (serializedObject.FindProperty("_CollisionAction").enumValueIndex == (int)Projectile.CollisionAction.Despawn)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_WholeLifetime"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_IsPostCollisionLifetimeEnabled"));
                if (!serializedObject.FindProperty("_IsPostCollisionLifetimeEnabled").boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_WholeLifetime"));
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_PreCollisionLifetime"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_PostCollisionLifetime"));
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Detonation", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_WillDetonateOnCollision"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_WillDetonateOnLifetimeEnd"));
            if (
                    serializedObject.FindProperty("_WillDetonateOnCollision").boolValue 
                    || 
                    serializedObject.FindProperty("_WillDetonateOnLifetimeEnd").boolValue
                )
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_GameObjectToSpawnOnDetonation"));

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_IsDetonationGameObjectLifetimeLimited"));
                if (serializedObject.FindProperty("_IsDetonationGameObjectLifetimeLimited").boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_DetonationGameObjectLifetime"));
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_DamageOnFirstCollision"));
            if (
                    serializedObject.FindProperty("_WillDetonateOnCollision").boolValue
                    ||
                    serializedObject.FindProperty("_WillDetonateOnLifetimeEnd").boolValue
                )
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_MaximumDetonationAreaDamage"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_DetonationAreaRadius"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_DetonationExplosionForce"));
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnSpawn"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnCollision"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnLifetimeEnd"));

            serializedObject.ApplyModifiedProperties();
        }
    }

}

