using HexCS.Core;

using HexUN.Data;
using HexUN.Engine.Utilities;

using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public static class UTAssets
    {
        /// <summary>
        /// The name of the assets folder in the database
        /// </summary>
        public const string cDatabaseAssetFolder = "assets";

        /// <summary>
        /// Get the asset path of an asset
        /// </summary>
        public static string GetPlatformAgnosticAssetPath(string assetName) => $"{cDatabaseAssetFolder}/{assetName}".ToLower();

        /// <summary>
        /// Gets the asset name for the target platform 
        /// </summary>
        public static string GetPlatformAssetName(string assetName)
        {
            // ISSUE:
            // Need a solution to resolve this name at runtime and in editor
            return $"{assetName}_standalonewindows";
        }

#if UNITY_EDITOR
        /// <summary>
        /// Converts the asset to an asset bundle and saves it to the local database
        /// in the users local filesystem.
        /// </summary>
        public static SaveToDatabaseResult ConvertToAssetBundleAndSaveToDatabase(GameObject instance)
        {
            // Get path to the prefab root
            PathString path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(instance);
            string assetBundleName = path.EndWithoutExtension;

            // Get the importer and set name to gameobject name
            AssetImporter ai = AssetImporter.GetAtPath(path);

            BuildTarget[] targets =
                new BuildTarget[] { BuildTarget.StandaloneWindows, BuildTarget.WebGL };

            UnityPath assetLocationPath = Folders.AssetLocationPath;
            foreach (BuildTarget bt in targets)
            {
                string name = $"{assetBundleName}_{bt}";
                ai.assetBundleName = name;
                BuildPipeline.BuildAssetBundles(assetLocationPath, BuildAssetBundleOptions.None, bt);
                ai.assetBundleName = null;
            }

            return new SaveToDatabaseResult() { AssetName = assetBundleName, AssetLocationPath = assetLocationPath};
        }

        /// <summary>
        /// Converts the asset to an asset bundle and saves it to the database. Returns null if fails
        /// </summary>
        public static SaveToDatabaseResult SavePrefabToEditableAssetDatabase(GameObject instance)
        {
            // Get path to the prefab root
            PathString path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(instance);
            string newPrefabName = path.End.ToLower();
            string newPrefabNameWithoutExtentions = path.EndWithoutExtension;

            UnityPath editableDatabase = Folders.EditableDatabaseAssetsPath;
            UnityPath targetAssetPath = editableDatabase.Path.InsertAtEnd(newPrefabName);

            if(!AssetDatabase.CopyAsset(path, targetAssetPath.AssetDatabaseAssetPath))
            {
                Debug.LogError($"[{nameof(UTAssets)}] Failed to copy asset {path} to {targetAssetPath}");
                return null;
            }
            
            AssetDatabase.Refresh();
            return new SaveToDatabaseResult() { AssetName = newPrefabNameWithoutExtentions, AssetLocationPath = targetAssetPath };
        }

        public class SaveToDatabaseResult
        {
            public string AssetName;
            public UnityPath AssetLocationPath;
        }
#endif
    }
}