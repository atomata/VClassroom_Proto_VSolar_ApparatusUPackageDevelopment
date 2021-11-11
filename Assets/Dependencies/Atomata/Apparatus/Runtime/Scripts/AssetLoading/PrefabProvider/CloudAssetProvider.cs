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
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Atomata.VSolar.Apparatus
{
    public class CloudAssetProvider : IPrefabProvider
    {
        private const string cLogCategory = nameof(CloudAssetProvider);

        private string _databaseName;

        private string _rootURL;

        private Dictionary<int, GameObject> _cachedBundles = new Dictionary<int, GameObject>();

        public CloudAssetProvider(string databaseName, string rootURL)
        {
            _databaseName = databaseName;
            _rootURL = rootURL;
        }

        public async UniTask<GameObject> Provide(string key, LogWriter writer)
        {
            writer.AddInfo(cLogCategory, cLogCategory, "Performing cloud prefab load");
            string file_name = key + "_webgl";
            string url = _rootURL + "/" + file_name;

            UnityWebRequest req = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation op = req.SendWebRequest();

            while (!op.isDone)
            {
                await UniTask.Delay(100);
            }
            byte[] data = req.downloadHandler.data;
            if (data != null)
            {
                int hashKey = file_name.GetHashCode();
                if (!_cachedBundles.ContainsKey(hashKey))
                {
                    // Load an assetbundle from bytes

                    AssetBundle assetBundle = AssetBundle.LoadFromMemory(data);
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