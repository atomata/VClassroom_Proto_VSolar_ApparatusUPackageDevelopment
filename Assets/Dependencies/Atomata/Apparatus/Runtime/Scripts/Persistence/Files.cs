#if UNITY_EDITOR
using Atomata.VSolar.Apparatus.UnityEditor;
using UnityEditor;
#endif

using HexUN.Data;
using HexUN.Framework;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Atomata files
    /// </summary>
    public static partial class Files
    {
        private const string cLogCategory = nameof(Files);
        private const string cAppartusConfigFileName = "ApparatusConfig.asset";

#if UNITY_EDITOR
        private static SoApparatusConfig _config = null;

        /// <summary>
        /// Path to the apparatus config json file
        /// </summary>
        public static UnityPath EditorOnly_ApparatusConfigPath
            => HexUN.Data.Folders.GetPath(ECommonFolder.Config).Path.InsertAtEnd(cAppartusConfigFileName);

        /// <summary>
        /// Load the config scriptable object. If this fails, logs a warning automatically
        /// </summary>
        public static SoApparatusConfig EditorOnly_ApparatusConfig
        {
            get
            {
                if(_config == null) 
                    _config = AssetDatabase.LoadAssetAtPath<SoApparatusConfig>(EditorOnly_ApparatusConfigPath.AssetDatabaseAssetPath);

                if (_config == null)
                    OneHexServices.Instance.Log.Warn(cLogCategory, $"Failed to find {nameof(SoApparatusConfig)}");

                return _config;
            }
        }
#endif

        /// <summary>
        /// Get the local absolute path to an asset
        /// </summary>
        public static UnityPath AssetPath(string asset)
        {
            return Folders.AssetLocationPath.Path.InsertAtEnd($"{asset}_standalonewindows");
        }
    }
}