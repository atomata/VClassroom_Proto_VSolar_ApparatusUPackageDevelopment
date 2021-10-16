using UnityEngine;
using UnityEditor;

using static Atomata.VSolar.Apparatus.UnityEditor.UtEiAApparatusNode;

namespace Atomata.VSolar.Apparatus.UnityEditor
{
    [CustomEditor(typeof(EventNode))]
    [CanEditMultipleObjects]
    public class EiApparatusEventNode : Editor
    {
        private BaseProperties _baseProperties;

        private SerializedProperty _voidEventNames;
        private SerializedProperty _voidEvents;
        private SerializedProperty _boolEventNames;
        private SerializedProperty _boolEvents;

        protected virtual void OnEnable()
        {
            _baseProperties = GetBaseProperties(serializedObject);

            _voidEventNames = serializedObject.FindProperty(nameof(_voidEventNames));
            _voidEvents = serializedObject.FindProperty(nameof(_voidEvents));
            _boolEventNames = serializedObject.FindProperty(nameof(_boolEventNames));
            _boolEvents = serializedObject.FindProperty(nameof(_boolEvents));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EventNode node = serializedObject.targetObject as EventNode;

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(node), typeof(EventNode), false);
            GUI.enabled = true;

            _baseProperties.RenderGUI(node);

            EditorGUILayout.PropertyField(_voidEventNames);
            EditorGUILayout.PropertyField(_voidEvents);
            EditorGUILayout.PropertyField(_boolEventNames);
            EditorGUILayout.PropertyField(_boolEvents);

            serializedObject.ApplyModifiedProperties();
        }
    }
}