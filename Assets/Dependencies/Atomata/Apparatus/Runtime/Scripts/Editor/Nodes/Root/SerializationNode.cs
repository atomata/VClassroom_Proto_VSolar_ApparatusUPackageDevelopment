#if UNITY_EDITOR
using HexCS.Data.Persistence;

using HexUN.Data;

using System.IO;
using System.Text;

using UnityEditor;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public partial class SerializationNode
    {
        /// <summary>
        /// Saves a prefab to the editable database in the Unity project. This allows the prefab
        /// to be used by totems, and eventually saved as asset bundles to a remote database
        /// endpoint
        /// </summary>
        [ContextMenu("SerailizeAndSave")]
        public void SerializeAndSave()
        {
            SerializeNode(this);

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        private void SerializeNode(SerializationNode node)
        {
            UnityPath editableDatabase = Folders.EditableDatabaseApparatusPath;

            string json = JsonUtility.ToJson(new SrApparatus(node));
            UnityPath svPath = editableDatabase.Path.InsertAtEnd($"{node.Identifier}.json");

            if (svPath.Path.TryAsFileInfo(out FileInfo pth))
            {
                pth.ForceEmptyOrCreate();
                pth.WriteString(json, Encoding.UTF8);
            }

            Debug.Log($"Saved Json to {svPath}");

            foreach (AApparatusNode child in node.Children)
            {
                SerializationNode rootChild = child as SerializationNode;
                if (rootChild != null) SerializeNode(rootChild);
            }
        }
    }
}
#endif