using UnityEngine;
using UnityEditor;

using static Atomata.VSolar.Apparatus.UnityEditor.UtEiAApparatusNode;

namespace Atomata.VSolar.Apparatus.UnityEditor
{
    [CustomEditor(typeof(AssetBundleNode))]
    [CanEditMultipleObjects]
    public class EiAssetBundleNode : Editor
    {
        private BaseProperties _baseProperties;

        protected virtual void OnEnable()
        {
            _baseProperties = GetBaseProperties(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AssetBundleNode node = serializedObject.targetObject as AssetBundleNode;

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(node), typeof(AssetBundleNode), false);
            GUI.enabled = true;

            _baseProperties.RenderGUI(node);
            serializedObject.ApplyModifiedProperties();
        }
    }
}