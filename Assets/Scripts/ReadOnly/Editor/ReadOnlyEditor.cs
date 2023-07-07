using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SkibidiRunner.ReadOnly.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ReadOnlyEditor : UnityEditor.Editor
    {
        private GUIStyle _style;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                {
                    if (IsPropertyReadOnly(iterator))
                    {
                        GUI.enabled = false;
                    }
                    EditorGUILayout.PropertyField(iterator, true);
                }
                    
            }
            serializedObject.ApplyModifiedProperties();
        }

        private static bool IsPropertyReadOnly(SerializedProperty property)
        {
            var targetObject = property.serializedObject.targetObject;
            var targetType = targetObject.GetType();
            string fieldName = property.propertyPath.Replace(".Array.data[", "[").Split('.')[0];
            var field = targetType.GetField(fieldName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            return field != null && field.IsDefined(typeof(ReadOnlyAttribute), true);
        }
    }
}