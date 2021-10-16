using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using HexUN.EditorElements;
using HexCS.Core;
using System;
using System.Reflection;

namespace Atomata.VSolar.Apparatus.UnityEditor
{
    /// <summary>
    /// Something about Unity Ui and Reflection usage seems to break Editor extended abstract classes
    /// from correctly functioning. This is my work around, a static utility that encapsulates some of the shared
    /// logic in the concrete editors. <see cref="EiAssetBundleNode"/> and <see cref="EiSerializationNode"/>
    /// </summary>
    public static class UtEiAApparatusNode
    {
        public static BaseProperties GetBaseProperties(SerializedObject serializedObject) => BaseProperties.FromObject(serializedObject);

        public class BaseProperties
        {
            public SerializedProperty _identifier;

            private bool _showGroup_Children = false;
            private Dictionary<string, bool> _showChildInfo = new Dictionary<string, bool>();

            public static BaseProperties FromObject(SerializedObject serializedObject)
            {
                return new BaseProperties()
                {
                    _identifier = serializedObject.FindProperty(nameof(_identifier))
                };
            }

            private T GetNonPublicValue<T>(string name, AApparatusNode node)
            {
                object value = typeof(AApparatusNode)
                    .GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(node);

                return (T)value;
            }

            public void RenderGUI(AApparatusNode node)
            {
                EditorGUILayout.PropertyField(_identifier);

                // Show non-serialized propertes, states, parent
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Connection Info", EditorStyles.boldLabel);
                
                GUI.enabled = false;
                EditorGUILayout.EnumPopup(
                    "Connection State",
                    GetNonPublicValue<EApparatusNodeConnectionState>("_connectionState", node)
                );

                EditorGUILayout.ObjectField(
                    "Parent",
                    GetNonPublicValue<AApparatusNode>("_parent", node),
                    typeof(AApparatusNode),
                    true
                );
                GUI.enabled = true;

                // Show children
                EditorGUILayout.Space();
                List<AApparatusNode> children = GetNonPublicValue<List<AApparatusNode>>("_children", node);
                _showGroup_Children = EditorGUILayout.BeginFoldoutHeaderGroup(_showGroup_Children, $"Children ({children.Count})", EditorStyles.foldout);

                GUI.enabled = false;
                if (_showGroup_Children)
                {
                    foreach (AApparatusNode child in children)
                    {
                        EditorGUILayout.ObjectField(child.Identifier, child, typeof(AApparatusNode),true);
                    }
                }
                GUI.enabled = true;

                EditorGUILayout.EndFoldoutHeaderGroup();

                // Allow children to populate their own ui section
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Children Debug Controls", EditorStyles.boldLabel);

                Queue<AApparatusNode> menuProcess = new Queue<AApparatusNode>();
                menuProcess.Enqueue(node);

                while(menuProcess.Count > 0)
                {
                    AApparatusNode child = menuProcess.Dequeue();
                    PathString path = child.Path(node.Identifier);

                    if (!_showChildInfo.ContainsKey(path)) _showChildInfo.Add(path, false);

                    if (child.IfUnityEditor_PopulateControls(node, out Action render))
                    {
                        _showChildInfo[path] = EditorGUILayout.BeginFoldoutHeaderGroup(_showChildInfo[path], path, EditorStyles.foldout);

                        if (_showChildInfo[path]) render();
                    
                        EditorGUILayout.EndFoldoutHeaderGroup();
                    }

                    foreach (AApparatusNode nxt in child.Children) menuProcess.Enqueue(nxt);
                }
            }
        }
    }
}