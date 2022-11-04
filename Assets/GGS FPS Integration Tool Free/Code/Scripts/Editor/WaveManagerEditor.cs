using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.Editor
{
    [CustomEditor(typeof(WaveManager)), CanEditMultipleObjects]
    public class WaveManagerEditor : UnityEditor.Editor
    {
        WaveManager _WaveManager;
        ReorderableList _ReorderableList;

        SerializedProperty _Waves;

        public void OnEnable()
        {
            float totalPreviousRectY = 0f;

            _WaveManager = (WaveManager)target;

            _Waves = serializedObject.FindProperty("Waves");

            _ReorderableList = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("Waves"),
                true, true, true, true);

            _ReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                totalPreviousRectY = rect.y;

                float visableTickBoxYPosition = rect.y + 35;

                rect.y += 2f;

                element.FindPropertyRelative("IsFoldoutOpen").boolValue = EditorGUI.Foldout(
                    new Rect(rect.x + 10, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("IsFoldoutOpen").boolValue,
                    "Wave " + (index + 1)
                    );

                rect.y += 23;

                if (element.FindPropertyRelative("IsFoldoutOpen").boolValue)
                {
                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("OnStart"),
                        new GUIContent("OnStart")
                        );

                    rect.y += GetUnityEventElementYAddition(_WaveManager.Waves[index].OnStart.GetPersistentEventCount());

                    EditorGUI.PropertyField(
                        new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("OnEnd"),
                        new GUIContent("OnEnd")
                        );

                    rect.y += GetUnityEventElementYAddition(_WaveManager.Waves[index].OnEnd.GetPersistentEventCount());
                }

                element.FindPropertyRelative("ReorderableListHeight").floatValue = rect.y - totalPreviousRectY;
            };

            _ReorderableList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), "Waves");
            };

            _ReorderableList.elementHeightCallback = (int index) =>
            {
                SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                if (!element.FindPropertyRelative("IsFoldoutOpen").boolValue)
                {
                    return 22f;
                }

                return element.FindPropertyRelative("ReorderableListHeight").floatValue;
            };

            _ReorderableList.onAddCallback = (ReorderableList list) =>
            {
                // Increment array size & select last element index
                int index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;

                SerializedProperty element = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                _WaveManager.Waves.Add(new Wave());

                element.FindPropertyRelative("IsFoldoutOpen").boolValue = false;
                element.FindPropertyRelative("HasUnityEventsBeenCleared").boolValue = false;
                element.FindPropertyRelative("ReorderableListHeight").floatValue = 0f;

                for (int i = 0; i < _WaveManager.Waves[index].OnStart.GetPersistentEventCount(); i++)
                {
                    UnityEditor.Events.UnityEventTools.RemovePersistentListener(_WaveManager.Waves[index].OnStart, i);
                }

                for (int i = 0; i < _WaveManager.Waves[index].OnEnd.GetPersistentEventCount(); i++)
                {
                    UnityEditor.Events.UnityEventTools.RemovePersistentListener(_WaveManager.Waves[index].OnEnd, i);
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Ensures new UnityEvent elements have been cleared
            // As this uses the 'target' variable, functions involved in it do not work if called instantly,
            // Thus clearing UnityEvents is performed after a delay that can vary. 
            // Works by continuely resetting UnityEvents until GetPersistentEventCount is 0, 
            // once there such count is allowed to vary
            //for (int i = 0; i < _Waves.arraySize; i++)
            for (int i = 0; i < _ReorderableList.serializedProperty.arraySize; i++)
            {
                if (_Waves.GetArrayElementAtIndex(i).FindPropertyRelative("HasUnityEventsBeenCleared").boolValue)
                {
                    continue;
                }

                if (
                        _WaveManager.Waves[i].OnStart.GetPersistentEventCount() == 0
                        &&
                        _WaveManager.Waves[i].OnEnd.GetPersistentEventCount() == 0
                    )
                {
                    _Waves.GetArrayElementAtIndex(i).FindPropertyRelative("HasUnityEventsBeenCleared").boolValue = true;
                }
                else
                {
                    for (int j = 0; j < _WaveManager.Waves[i].OnStart.GetPersistentEventCount(); j++)
                    {
                        UnityEditor.Events.UnityEventTools.RemovePersistentListener(_WaveManager.Waves[i].OnStart, j);
                    }

                    for (int j = 0; j < _WaveManager.Waves[i].OnEnd.GetPersistentEventCount(); j++)
                    {
                        UnityEditor.Events.UnityEventTools.RemovePersistentListener(_WaveManager.Waves[i].OnEnd, j);
                    }
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_WillStartWaveOnStart"));

            _ReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        float GetUnityEventElementYAddition(int persistentEventCount)
        {
            float rectY = 0f;

            if (persistentEventCount > 1)
            {
                rectY += 47f * persistentEventCount - 1;
                rectY += 54f;
            }
            else
            {
                rectY += 100f;
            }

            return rectY;
        }
    }
}