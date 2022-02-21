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

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class LocalApparatusProvider : IApparatusProvider
    {
        private const string cLogCategory = nameof(LocalAssetBundleProvider);

        private string _databaseName;

        private Dictionary<int, SrNode> _cache = new Dictionary<int, SrNode>();

        public LocalApparatusProvider(string databaseName)
        {
            _databaseName = databaseName;
        }

        public async UniTask<SrNode> Provide(string key, LogWriter writer)
        {
            writer.AddInfo(cLogCategory, cLogCategory, "Performing local apparatus deserialization");

            // find the file based on the request args
            UnityPath databasePath = UnityPath.PersistentDataPath.Path
                .InsertAtEnd("Database")
                .InsertAtEnd(_databaseName)
                .InsertAtEnd("apparatus");

            if (databasePath.Path.TryAsDirectoryInfo(out DirectoryInfo di))
            {
                // find the file based on the request args
                FileInfo[] files = di.GetFiles();
                FileInfo file = files.FirstOrDefault(
                    f =>
                    {
                        PathString ps = f.FullName;
                        return ps.End == $"{key}.json";
                    }
                );

                if (file == null)
                {
                    writer.AddWarning(cLogCategory, cLogCategory, $"Unable to load apparatus of key {{ {key} }} from path {{ {di.FullName} }}");
                    return null;
                }

                int hashKey = file.FullName.GetHashCode();
                if (!_cache.ContainsKey(hashKey))
                {
                    // Load an assetbundle from bytes
                    string json = null;
                    using (FileStream fs = file.OpenRead())
                    {
                        using(StreamReader sr = new StreamReader(fs))
                        {
                            json = sr.ReadToEnd(); 
                        }
                    }

                    SrNode deserailized = JsonUtility.FromJson<SrNode>(json);
                    _cache.Add(hashKey, deserailized);
                }

                writer.AddInfo(cLogCategory, cLogCategory, "Local apparatus deserialization successful");
                return _cache[hashKey];
            }

            writer.AddInfo(cLogCategory, cLogCategory, "Local apparatus deserialization failed");
            return null;
        }
    }
}