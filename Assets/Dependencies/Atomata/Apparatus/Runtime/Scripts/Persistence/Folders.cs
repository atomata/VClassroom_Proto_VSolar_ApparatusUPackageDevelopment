using Atomata.VSolar.Apparatus.UnityEditor;

using HexCS.Core;
using HexCS.Data.Persistence;

using HexUN.Data;

using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public static class Folders
    {        
        /// <summary>
        /// Creates the path if it does not exist
        /// </summary>
        public static UnityPath AssetLocationPath
        {
            get
            {
#if UNITY_EDITOR
                
                PathString path = HexUN.Data.Folders.GetPath(ECommonFolder.UserData).Path
                    .InsertAtEnd(PlayerSettings.companyName)
                    .InsertAtEnd(Files.EditorOnly_ApparatusConfig.ApparatusDatabaseName)
                    .InsertAtEnd(UTAssets.cDatabaseAssetFolder);

                if (!path.TryAsDirectoryInfo(out DirectoryInfo info)) return null;
                UTDirectoryInfo.ExistsOrCreate(info);

                return path;
#else
                // TO DO: Add something else for none editor path resolution
                Debug.LogError("AssetLocationPath NOT YET IMPLEMENTED");
                return null;
#endif
            }
        }

        /// <summary>
        /// Creates the path if it does not exist
        /// </summary>
        public static UnityPath EditableDatabasePath
        {
            get
            {
#if UNITY_EDITOR
                string databaseName = Files.EditorOnly_ApparatusConfig.ApparatusDatabaseName;
                string editableDatabasePath = Files.EditorOnly_ApparatusConfig.EditableDatabase;

                PathString path = HexUN.Data.Folders.GetPath(ECommonFolder.Assets).Path
                    .InsertAtEnd(editableDatabasePath)
                    .InsertAtEnd(databaseName);

                if (!path.TryAsDirectoryInfo(out DirectoryInfo info)) return null;
                UTDirectoryInfo.ExistsOrCreate(info);

                return path;
#endif
                return null;
            }
        }

        /// <summary>
        /// Creates the path if it doesn not exist
        /// </summary>
        public static UnityPath EditableDatabaseAssetsPath
        {
            get
            {
                PathString path = EditableDatabasePath.Path.InsertAtEnd("assetbundles");

                if (!path.TryAsDirectoryInfo(out DirectoryInfo info)) return null;
                UTDirectoryInfo.ExistsOrCreate(info);

                return path;
            }
        }

        /// <summary>
        /// Creates the path if it doesn not exist
        /// </summary>
        public static UnityPath EditableDatabaseApparatusPath
        {
            get
            {
                PathString path = EditableDatabasePath.Path.InsertAtEnd("apparatus");

                if (!path.TryAsDirectoryInfo(out DirectoryInfo info)) return null;
                UTDirectoryInfo.ExistsOrCreate(info);

                return path;
            }
        }
    }
}