using HexCS.Data.Persistence;

using HexUN.Data;
using HexUN.Engine.Utilities;
using HexUN.Framework;

using System;
using System.IO;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Provides functionality for communicating with a resource database stored in
    /// on the local file system
    /// </summary>
    [CreateAssetMenu(fileName = nameof(LocalResourceDatabase), menuName = "Atomata/Apparatus/Services/LocalResourceDatabase")]
    public class LocalResourceDatabase : ScriptableObject, IResourceDatabase
    {
        private const string cLogCategory = nameof(LocalResourceDatabase);

        private UnityPath _dbUnityPath = null;

        private UnityPath _dbPath
        {
            get
            {
                if(_dbUnityPath == null) _dbUnityPath = Folders.AssetLocationPath;
                return _dbUnityPath;
            }
        }

        /// <inheritdoc />
        public GameObject ResolveAsset(string name)
        {
            UnityPath path = _dbPath.Path.InsertAtEnd(UTAssets.GetPlatformAssetName(name));

            // Unload if already loaded (it might have changed)
            if (!UTAssetBundle.TryLoadGameObjectAssetBundle(path, out GameObject loaded,true))
            {
                Debug.LogError($"[{nameof(LocalResourceDatabase)}] Failed to load asset {name}. It cannot be found in the configured database");
                return null;
            }

            return loaded;
        }

        public SrApparatus ResolveSerializedNode(string identifier)
        {
            UnityPath path = Folders.EditableDatabaseApparatusPath.Path.InsertAtEnd($"{identifier}.json");

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
    }
}