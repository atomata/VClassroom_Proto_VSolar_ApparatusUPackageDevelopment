using Atomata.VSolar.Apparatus.UnityEditor;

using Cysharp.Threading.Tasks;

using HexUN.Framework.Debugging;

using System.Collections.Generic;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class EditableDatabaseGameObjectProvider : IGameObjectProvider
    {
        private const string cLogCategory = nameof(EditableDatabaseGameObjectProvider);

        private SoApparatusConfig _config;
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        public EditableDatabaseGameObjectProvider(SoApparatusConfig config)
        {
            _config = config;
        }

        public async UniTask<GameObject> Provide(string key, LogWriter log)
        {
            log.AddInfo(cLogCategory, cLogCategory, "Performing editable database prefab load");

            if (_prefabCache.ContainsKey(key))
                return _prefabCache[key];

            if (_config.EditorResourceDatabase == null)
            {
                log.AddWarning(cLogCategory, cLogCategory, "editable database prefab load failed. Could not find database in config");
                return null;
            }
            else
            {
                GameObject load = _config.EditorResourceDatabase?.ResolveAsset(key);
                _prefabCache[key] = load;
                log.AddInfo(cLogCategory, cLogCategory, "editable database prefab load complete");
                return load;
            }
        }
    }
}