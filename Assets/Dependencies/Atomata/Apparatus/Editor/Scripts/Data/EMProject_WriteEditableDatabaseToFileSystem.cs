using HexCS.Core;
using HexCS.Data.Persistence;

using HexUN.Data;
using HexUN.Framework;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEngine;

namespace Atomata.VSolar.Apparatus.UnityEditor
{
    /// <summary>
    /// Tool that allows developers to export a database prepared manually
    /// in a Unity Project and export it to their file system. Uses a standard path
    /// based on <see cref="Application.persistentDataPath"/>.
    /// </summary>
    public static class EMProject_WriteEditableDatabaseToFileSystem
    {
        private const string cLogCategory = "WriteEditableDatabaseToFileSystem";

        private static readonly BuildTarget[] SupportedTargets = new BuildTarget[]
        {
            BuildTarget.WebGL,
            BuildTarget.StandaloneWindows
        };

        /// <summary>
        /// Process an object, and save it to the target directory
        /// </summary>
        private delegate void ExportStepProcessor(PathString sourceFile, PathString targetDirectory);

        [MenuItem("Assets/Atomata/WriteEditableDatabaseToFileSystem", false, 20)]
        public static void EMWriteEditableDatabaseToFileSystem(MenuCommand command)
        {
            // Get the selected resource
            string selectedAssetDatabasePath = AssetDatabase.GetAssetPath(Selection.activeObject);
            UnityPath selectedPath = UnityPath.FromAssetDatabasePath(selectedAssetDatabasePath);

            // Get the path to save to (config)
            PathString targetRoot = UnityPath.PersistentDataPath.Path.InsertAtEnd("Database");

            if(!targetRoot.TryAsDirectoryInfo(out DirectoryInfo info))
            {
                OneHexServices.Instance.Log.Error(cLogCategory, $"Failed to get {targetRoot} as directory info. Cannot save database");
                return;
            }

            info.ExistsOrCreate();

            SoApparatusConfig config = Files.EditorOnly_ApparatusConfig;

            if(config == null)
            {
                OneHexServices.Instance.Log.Error(cLogCategory, $"No {nameof(SoApparatusConfig)}, cannot complete export");
                return;
            }

            // Create the destination directory for the export
            PathString srcDatabaseRoot = selectedPath.Path.InsertAtEnd($"{config.ApparatusDatabaseName}");
            PathString targetDatabaseRoot = targetRoot.InsertAtEnd($"{config.ApparatusDatabaseName}");
            targetDatabaseRoot.TryAsDirectoryInfo(out DirectoryInfo databaseDi);
            databaseDi.ExistsOrCreate();

            // Save the json files as they are to their new location
            PerformSourceTargetProcess("apparatus", "json", ProcessApparatusJson);

            void ProcessApparatusJson(PathString sourceFile, PathString targetFolder)
            {
                if (sourceFile.TryAsFileInfo(out FileInfo fi))
                {
                    fi.CopyTo(targetFolder.InsertAtEnd(sourceFile.End), true);
                }
            }

            // Converts to assets to asset bundles, then move them to the desired location
            PerformSourceTargetProcess("assetbundles", "prefab", ProcessAssetBundle);

            void ProcessAssetBundle(PathString sourceFile, PathString targetFolder)
            {
                UnityPath bundleFile = sourceFile;

                foreach(BuildTarget bt in SupportedTargets)
                {
                    AssetBundleBuild abb = new AssetBundleBuild()
                    {
                        assetBundleName = $"{sourceFile.EndWithoutExtension}_{Enum.GetName(typeof(BuildTarget), bt)}",
                        assetNames = new string[] { bundleFile.ProjectRelativePath }
                    };

                    AssetBundleManifest abm = BuildPipeline.BuildAssetBundles(
                        targetFolder,
                        new AssetBundleBuild[] { abb },
                        BuildAssetBundleOptions.None,
                        bt
                    );
                }
            }

            OneHexServices.Instance.Log.Info(cLogCategory, $"Saved editable database to path {targetRoot}");

            void PerformSourceTargetProcess(string srcDirectoryName, string srcFileExtention, ExportStepProcessor processor)
            {
                PathString srcProcessRoot = srcDatabaseRoot.InsertAtEnd(srcDirectoryName);
                PathString targetProcessRoot = targetDatabaseRoot.InsertAtEnd(srcDirectoryName);

                // make sure the source directoy exists
                if (!srcProcessRoot.TryAsDirectoryInfo(out DirectoryInfo srcDir) || !srcDir.Exists)
                {
                    OneHexServices.Instance.Log.Error(cLogCategory, $"Failed export step {srcDirectoryName}, could not find directory");
                    return;
                }

                // enforce that target directory exists
                targetProcessRoot.TryAsDirectoryInfo(out DirectoryInfo targetProcessDi);
                targetProcessDi.ExistsOrCreate();

                // get all source objects that need
                IEnumerable<PathString> srcObject = srcDir.GetFiles()
                    .Select(f => new PathString(f.FullName))
                    .Where(p => p.Extension == srcFileExtention);

                // iterate over all the paths 
                foreach (PathString srcPath in srcObject)
                {
                    processor(srcPath, targetProcessRoot);

                    if (srcPath.TryAsFileInfo(out FileInfo srcFile))
                    {
                        srcFile.CopyTo(targetProcessRoot.InsertAtEnd(srcPath.End), true);
                    }
                }
            }
        }

        [MenuItem("Assets/Atomata/WriteEditableDatabaseToFileSystem", true)]
        public static bool EMWriteEdtableDatabaseToFileSystemValidator()
        {
            SoApparatusConfig config = Files.EditorOnly_ApparatusConfig;

            if (config == null) return false;
            UnityEngine.Object selected = Selection.activeObject;

            return selected != null && selected.name == config.EditableDatabase;
        }
    }
}