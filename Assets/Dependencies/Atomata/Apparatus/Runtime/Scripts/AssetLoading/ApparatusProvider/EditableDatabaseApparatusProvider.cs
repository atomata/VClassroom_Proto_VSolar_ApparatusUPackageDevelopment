using Atomata.VSolar.Apparatus.UnityEditor;
using Cysharp.Threading.Tasks;
using HexUN.Framework.Debugging;
using System.Collections.Generic;

namespace Atomata.VSolar.Apparatus
{
    public class EditableDatabaseApparatusProvider : IApparatusProvider
    {
        private const string cLogCategory = nameof(EditableDatabaseApparatusProvider);
        private SoApparatusConfig _config;
        private Dictionary<string, SrNode> _cache = new Dictionary<string, SrNode>();

        public EditableDatabaseApparatusProvider(SoApparatusConfig config)
        {
            _config = config;
        }

        public async UniTask<SrNode> Provide(string key, LogWriter writer)
        {
            writer.AddInfo(cLogCategory, cLogCategory, "Performing editable database apparatus deserialization");

            if (_cache.ContainsKey(key))
                return _cache[key];

            if (_config.EditorResourceDatabase == null)
            {
                writer.AddWarning(cLogCategory, cLogCategory, "editable database apparatus deserialization failed. Could not find database in config");
                return null;
            }
            else
            {
                SrNode node = _config.EditorResourceDatabase?.ResolveSerializedNode(key);
                _cache[key] = node;
                writer.AddInfo(cLogCategory, cLogCategory, "editable database apparatus deserialization complete");
                return node;
            }
        }
    }
}