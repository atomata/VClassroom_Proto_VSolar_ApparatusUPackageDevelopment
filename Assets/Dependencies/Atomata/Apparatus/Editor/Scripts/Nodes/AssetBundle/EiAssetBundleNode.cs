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
        private SerializedProperty AssetBundleKey;

        protected virtual void OnEnable()
        {
            _baseProperties = GetBaseProperties(serializedObject);
            AssetBundleKey = serializedObject.FindProperty("AssetBundleKey");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AssetBundleNode node = serializedObject.targetObject as AssetBundleNode;

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(node), typeof(AssetBundleNode), false);
            EditorGUILayout.ObjectField("Editor", MonoScript.FromScriptableObject(this), typeof(EiAssetBundleNode), false);
            GUI.enabled = true;

            _baseProperties.RenderGUI_Fields(node);
            EditorGUILayout.PropertyField(AssetBundleKey);
            _baseProperties.RenderGUI_ConnectionInfoAndDebug(node);

            serializedObject.ApplyModifiedProperties();
        }
    }
}