using UnityEngine;
using UnityEditor;

using static Atomata.VSolar.Apparatus.UnityEditor.UtEiAApparatusNode;

namespace Atomata.VSolar.Apparatus.UnityEditor
{
    [CustomEditor(typeof(DeltaTransformNode))]
    [CanEditMultipleObjects]
    public class EiApparatusDeltaTransformNode : Editor
    {
        private BaseProperties _baseProperties;
        private SerializedProperty _managedTransform;

        protected virtual void OnEnable()
        {
            _baseProperties = GetBaseProperties(serializedObject);
            _managedTransform = serializedObject.FindProperty(nameof(_managedTransform));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DeltaTransformNode node = serializedObject.targetObject as DeltaTransformNode;

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(node), typeof(EventNode), false);
            GUI.enabled = true;

            _baseProperties.RenderGUI(node);

            EditorGUILayout.PropertyField(_managedTransform);

            GUI.enabled = false;
            EditorGUILayout.Toggle(node.IsLocal);
            EditorGUILayout.Vector3Field("PositionDelta", node.PositionDelta);
            EditorGUILayout.Vector3Field("RotationDelta", node.RotationDelta);
            EditorGUILayout.Vector3Field("ScaleDelta", node.ScaleDelta);
            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }
    }
}