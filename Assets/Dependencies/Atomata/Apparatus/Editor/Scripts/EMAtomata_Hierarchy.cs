using HexUN.Engine.Utilities;

using UnityEditor;

using UnityEngine;

namespace Atomata.VSolar.Apparatus.UnityEditor 
{ 
    public static class EMAtomata_Hierarchy
    {
        /// <summary>
        /// Creates an asset node
        /// </summary>
        [MenuItem("GameObject/Atomata/AssetNode", false, 10)]
        public static void CreateAssetNode(MenuCommand menuCommand)
        {
            GameObject obj = menuCommand.context as GameObject;
            string name = "[Asset] NewAsset";

            GameObject created = null;
            if(obj == null)
            {
                created = new GameObject(name);
            }
            else
            {
                created = obj.AddChild(name);
            }

            created.AddComponent<AssetBundleNode>();
            Undo.RegisterCreatedObjectUndo(created, "Created Asset Node");
        }

        /// <summary>
        /// Creates an Apparatus Node
        /// </summary>
        [MenuItem("GameObject/Atomata/ApparatusNode", false, 10)]
        public static void CreateApparatusNode(MenuCommand menuCommand)
        {
            GameObject obj = menuCommand.context as GameObject;
            string name = "[Apparatus] NewApparatus";

            GameObject created = null;
            if (obj == null)
            {
                created = new GameObject(name);
            }
            else
            {
                created = obj.AddChild(name);
            }

            created.AddComponent<SerializationNode>();
            Undo.RegisterCreatedObjectUndo(created, "Created Apparatus Node");
        }
    }
}
