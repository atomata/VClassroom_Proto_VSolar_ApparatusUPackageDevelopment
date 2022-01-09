using Atomata.VSolar.Apparatus.UnityEditor;

using Cysharp.Threading.Tasks;

using HexCS.Core;

using HexUN.Data;
using HexUN.Framework;
using HexUN.Framework.Debugging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class LocalAssetBundleProvider : IGameObjectProvider
    {
        private const string cLogCategory = nameof(LocalAssetBundleProvider);

        private string _databaseName;

        private Dictionary<int, GameObject> _cachedBundles = new Dictionary<int, GameObject>();

        public LocalAssetBundleProvider(string databaseName)
        {
            _databaseName = databaseName;
        }

        public async UniTask<GameObject> Provide(string key, LogWriter writer)
        {
            writer.AddInfo(cLogCategory, cLogCategory, "Performing local prefab load");

            // find the file based on the request args
            UnityPath databasePath = UnityPath.PersistentDataPath.Path
                .InsertAtEnd("Database")
                .InsertAtEnd(_databaseName)
                .InsertAtEnd("assetbundles");

            if (databasePath.Path.TryAsDirectoryInfo(out DirectoryInfo di))
            {
                string platformKey = $"{key}_{Enum.GetName(typeof(EAtomataPlatform), UTAtomataPlatform.FromRuntimePlatform(Application.platform))}".ToLower();

                // find the file based on the request args
                FileInfo[] files = di.GetFiles();
                FileInfo file = files.FirstOrDefault(
                    f =>
                    {
                        PathString ps = f.FullName;
                        return ps.End == platformKey;
                    }
                );

                if (file == null)
                {
                    writer.AddWarning(cLogCategory, cLogCategory, $"Unable to load assetbundle of key {{ {platformKey} }} from path {{ {di.FullName} }}");
                    return null;
                }

                int hashKey = file.FullName.GetHashCode();
                if (!_cachedBundles.ContainsKey(hashKey))
                {
                    // Load an assetbundle from bytes
                    byte[] bytes = null;
                    using (FileStream fs = file.OpenRead())
                    {
                        bytes = fs.ReadAllBytes();
                    }

                    AssetBundle assetBundle = await AssetBundle.LoadFromMemoryAsync(bytes);
                    UnityEngine.Object obj = await assetBundle.LoadAllAssetsAsync();
                    GameObject go = obj as GameObject;

                    _cachedBundles.Add(hashKey, go);
                }

                writer.AddInfo(cLogCategory, cLogCategory, "Local asset load successful");
                return _cachedBundles[hashKey];
            }

            writer.AddError(cLogCategory, cLogCategory, "Local asset load failed");
            return null;
        }
    }
}