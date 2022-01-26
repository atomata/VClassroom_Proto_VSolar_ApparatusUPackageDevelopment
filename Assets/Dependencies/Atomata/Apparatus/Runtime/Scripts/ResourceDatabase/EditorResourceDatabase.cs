using HexCS.Core;
using HexCS.Data.Persistence;

using HexUN.Data;
using HexUN.Engine.Utilities;
using HexUN.Framework;

using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Provides functionality for communicating with a resource database stored in
    /// the Unity project
    /// </summary>
    [CreateAssetMenu(fileName = nameof(EditorResourceDatabase), menuName = "Atomata/Apparatus/Services/EditorResourceDatabase")]
    public class EditorResourceDatabase : ScriptableObject, IEditorResourceDatabase
    {
        private const string cLogCategory = nameof(EditorResourceDatabase);


        /// <inheritdoc/>
        public GameObject ResolveAsset(string name)
        {
#if UNITY_EDITOR
            UnityPath obj = Folders.EditableDatabaseAssetsPath.Path.InsertAtEnd(name).AddExtension("prefab");

            UnityEngine.Object instance 
                = AssetDatabase.LoadAssetAtPath<GameObject>(obj.AssetDatabaseAssetPath);

            GameObject prefab = instance as GameObject;

            // if asset dosen't exist
            if (instance == null || prefab == null)
            {
                // TO DO: Handle non-existance
                Debug.LogError($"[{nameof(EditorResourceDatabase)}] Failed to load prefab {name}. This means that the prefab is not available in the Assets folder because it was created in another project.");
            }

            return prefab;
#endif  
            return null;
        }

        public SrApparatus ResolveSerializedNode(string identifier)
        {
            UnityPath path = Folders.EditableDatabaseApparatusPath.Path.InsertAtEnd($"{identifier}");

            if (!path.Path.TryAsFileInfo(out FileInfo info))
                OneHexServices.Instance.Log.Error(cLogCategory, $"Failed to get path {path} as {nameof(FileInfo)}");

            try
            {
                return JsonUtility.FromJson<SrApparatus>(info.ReadAllText());
            }
            catch (Exception e)
            {
                OneHexServices.Instance.Log.Error(cLogCategory, $"Failed to derserailize file {path}.", e);
                return default;
            }
        }

        /// <inheritdoc/>
        public void UpdatePrefab(GameObject instance)
        {
#if UNITY_EDITOR
            if (!UTPrefab.TryApplyOverrides(instance))
            {
                Debug.LogError($"[{nameof(EditorResourceDatabase)}] There was an issue applying overrides to the prefab {instance.name}");
            }
#endif
        }
    }
}