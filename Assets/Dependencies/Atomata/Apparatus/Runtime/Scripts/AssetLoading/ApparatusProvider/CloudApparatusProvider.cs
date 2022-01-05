using Cysharp.Threading.Tasks;

using HexCS.Core;

using HexUN.Data;
using HexUN.Framework;
using HexUN.Framework.Debugging;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Atomata.VSolar.Apparatus
{
    public class CloudApparatusProvider : IApparatusProvider
    {
        private const string cLogCategory = nameof(CloudApparatusProvider);

        private string _databaseName;

        private string _rootURL;

        private Dictionary<int, SrApparatus> _cache = new Dictionary<int, SrApparatus>();

        public CloudApparatusProvider(string databaseName, string rootURL)
        {
            _databaseName = databaseName;
            _rootURL = rootURL;
        }

        public async UniTask<SrApparatus> Provide(string key, LogWriter writer)
        {
            writer.AddInfo(cLogCategory, cLogCategory, "Performing cloud apparatus deserialization");
            string file_name = key + ".json";
            string url = _rootURL + "/" + file_name;
            UnityWebRequest req = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation op = req.SendWebRequest();

            while (!op.isDone)
            {
                await UniTask.Delay(100);
            }

            SrApparatus deserailized = JsonUtility.FromJson<SrApparatus>(req.downloadHandler.text);
            int hashKey = file_name.GetHashCode();
            if (deserailized != null)
            {
                if (!_cache.ContainsKey(hashKey))
                    _cache.Add(hashKey, deserailized);

                writer.AddInfo(cLogCategory, cLogCategory, "Local apparatus deserialization successful");
                return _cache[hashKey];
            }

            writer.AddInfo(cLogCategory, cLogCategory, "Local apparatus deserialization failed");
            return null;
        }
    }
}

