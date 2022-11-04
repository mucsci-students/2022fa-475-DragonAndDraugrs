using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(Counter)), CanEditMultipleObjects]
    public class CounterEditor : UnityEditor.Editor
    {
        Counter _Counter;
        int _LastThresholdListSize = 0;

        SerializedProperty _Thresholds;

        void OnEnable()
        {
            _Counter = (Counter)target;

            _Thresholds = serializedObject.FindProperty("Thresholds");

            // Prevents field enteries from resetting when selecting differnt GUI selectable
            _LastThresholdListSize = _Thresholds.arraySize;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Initialises the variables of the new array elements
            if (_Thresholds.arraySize > _LastThresholdListSize)
            {
                for (int i = _LastThresholdListSize; i < _Thresholds.arraySize; i++)
                {
                    _Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("Count").intValue = 0;
                    _Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("HasBeenExecuted").boolValue = false;

                    _Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("IsFoldoutOpen").boolValue = false;
                    _Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("HasOnThresholdBeenCleared").boolValue = false;
                }
            }

            // Ensures OnThreshold new array elements have been cleared
            // As this uses the '(Counter)target' variable, functions involved in it do not work if called instantly,
            // thus clearing OnThreshold is performed after a delay that can vary. 
            // Works by continuely resetting OnThreshold until GetPersistentEventCount is 0, 
            // once there such count is allowed to vary
            for (int i = 0; i < _Thresholds.arraySize; i++)
            {
                if (_Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("HasOnThresholdBeenCleared").boolValue)
                {
                    continue;
                }

                if (_Counter.Thresholds[i].OnThreshold.GetPersistentEventCount() == 0)
                {
                    _Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("HasOnThresholdBeenCleared").boolValue = true;
                }
                else
                {
                    for (int j = 0; j < _Counter.Thresholds[i].OnThreshold.GetPersistentEventCount(); j++)
                    {
                        UnityEditor.Events.UnityEventTools.RemovePersistentListener(_Counter.Thresholds[i].OnThreshold, j);
                    }
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnIncrement"));

            EditorGUILayout.Space();

            // Appy this before the Size field prevents array index out-of-range errors
            _LastThresholdListSize = _Thresholds.arraySize;

            // Size field
            EditorGUILayout.PropertyField(_Thresholds.FindPropertyRelative("Array.size"));

            // Prevents null errors when adding this component to GameObject
            if (_Counter.Thresholds != null)
            {
                // Lambda expression for sorting order of Threshold elements by Count
                _Counter.Thresholds.Sort((Threshold x, Threshold y) => x.Count.CompareTo(y.Count));
            }

            // Generate the fields of Threshold elements
            for (int i = 0; i < _Thresholds.arraySize; i++)
            {
                _Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("IsFoldoutOpen").boolValue =
                    EditorGUILayout.Foldout(
                        _Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("IsFoldoutOpen").boolValue,
                        "Threshold Count: " + _Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("Count").intValue,
                        true
                        );

                if (_Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("IsFoldoutOpen").boolValue)
                {
                    EditorGUILayout.PropertyField(_Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("Count"));
                    EditorGUILayout.PropertyField(_Thresholds.GetArrayElementAtIndex(i).FindPropertyRelative("OnThreshold"));
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}