using UnityEditor;

using UnityEngine;

using static Atomata.VSolar.Apparatus.UnityEditor.UtEiAApparatusNode;

namespace Atomata.VSolar.Apparatus.UnityEditor
{
    [CustomEditor(typeof(SerializationNode))]
    [CanEditMultipleObjects]
    public class EiSerializationNode : Editor
    {
        private SerializedProperty _onRequest;
        private SerializedProperty ApparatusKey;
        private SerializedProperty DefaultState;
        private BaseProperties _baseProperties;

        protected virtual void OnEnable()
        {
            _onRequest = serializedObject.FindProperty("_onRequest");
            ApparatusKey = serializedObject.FindProperty("ApparatusKey");
            DefaultState = serializedObject.FindProperty(nameof(DefaultState));
            _baseProperties = GetBaseProperties(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializationNode node = serializedObject.targetObject as SerializationNode;

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(node), typeof(SerializationNode), false);
            EditorGUILayout.ObjectField("Editor", MonoScript.FromScriptableObject(this), typeof(EiSerializationNode), false);
            GUI.enabled = true;

            EditorGUILayout.PropertyField(_onRequest);
            _baseProperties.RenderGUI_Fields(node);
            EditorGUILayout.PropertyField(ApparatusKey);
            EditorGUILayout.PropertyField(DefaultState);
            _baseProperties.RenderGUI_ConnectionInfoAndDebug(node);
            serializedObject.ApplyModifiedProperties();
        }
    }
}