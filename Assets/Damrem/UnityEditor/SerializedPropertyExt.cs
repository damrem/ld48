using System;
using UnityEditor;
using UnityEngine;

namespace Damrem.UnityEditor {
    public static class SerializedPropertyExt {
        public static void DrawExpandedListInspector(this SerializedProperty listProperty) {
            listProperty.DrawExpandedListInspector(item => {
                EditorGUILayout.PropertyField(item);
            });
            listProperty.isExpanded = false;
        }

        public static void DrawExpandedListInspector(this SerializedProperty listProperty, Action<SerializedProperty> customizeItem) {
            var label = new GUIContent($"{listProperty.displayName} ({listProperty.arraySize})");
            EditorGUILayout.PropertyField(listProperty, label, false);
            if (!listProperty.isExpanded) return;

            for (int i = 0; i < listProperty.arraySize; i++) {
                EditorGUI.indentLevel += 1;
                var item = listProperty.GetArrayElementAtIndex(i);
                item.isExpanded = true;
                customizeItem(item);
                EditorGUI.indentLevel -= 1;
            }
        }

        public static void DrawScriptableObject<T>(this SerializedProperty property) where T : ScriptableObject {
            var serializedObject = new SerializedObject(property.objectReferenceValue);
            var prop = serializedObject.GetIterator();
            if (prop.NextVisible(true)) {
                do {
                    if (prop.name == "m_Script") continue;
                    EditorGUILayout.PropertyField(prop, false);
                }
                while (prop.NextVisible(false));
            }
            if (GUI.changed) serializedObject.ApplyModifiedProperties();
        }
    }
}