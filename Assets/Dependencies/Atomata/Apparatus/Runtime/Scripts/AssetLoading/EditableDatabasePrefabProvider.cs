using Atomata.VSolar.Apparatus.UnityEditor;

using Cysharp.Threading.Tasks;

using System.Collections.Generic;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class EditableDatabasePrefabProvider : IPrefabProvider
    {
        private SoApparatusConfig _config;
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        public EditableDatabasePrefabProvider(SoApparatusConfig config)
        {
            _config = config;
        }

        public async UniTask<GameObject> Provide(string key)
        {
            if (_prefabCache.ContainsKey(key))
                return _prefabCache[key];

            if (_config.EditorResourceDatabase == null)
            {
                return null;
            }
            else
            {
                return _config.EditorResourceDatabase?.ResolveAsset(key);
            }
        }
    }
}