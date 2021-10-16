using UnityEngine;
using HexUN.Data;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HexUN.Utilities
{
    public static class UTScriptableObject
    {

#if UNITY_EDITOR
        /// <summary>
        ///	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// The path should point to the indented file dir/path/soname. The .asset extension should be
        /// included in the path. The change will not show up in the editor without a call to AssetDatabase.Refresh()
        /// </summary>
        public static TSo CreateSoAsset<TSo>(UnityPath path) where TSo : ScriptableObject
        {
            TSo asset = ScriptableObject.CreateInstance<TSo>();
            AssetDatabase.CreateAsset(asset, path.AssetDatabaseAssetPath);
            AssetDatabase.SaveAssets();
            return asset;
        }

        /// <summary>
        ///	This makes it easy to create, name and place unique new ScriptableObject asset files using the type name.
        /// The path should point to the indented file dir/path/soname. The .asset extension should be
        /// included in the path. The change will not show up in the editor without a call to AssetDatabase.Refresh()
        /// </summary>
        public static ScriptableObject CreateSoAsset(UnityPath path, string type)
        {
            path.Path.CreateIfNotExistsDirectory();
            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path.AssetDatabaseAssetPath);
            AssetDatabase.SaveAssets();
            return asset;
        }

        /// <summary>
        /// Attempts to load an SoAsset. If not found, creates it. Path should include the .asset extension.
        /// The change will not show up in the editor without a call to AssetDatabase.Refresh()
        /// </summary>
        /// <typeparam name="TSo"></typeparam>
        /// <param name="path"></param>
        /// <param name="autoRefesh"></param>
        /// <returns></returns>
        public static TSo LoadOrCreateSoAsset<TSo>(UnityPath path)
            where TSo: ScriptableObject
        {
            TSo obj = AssetDatabase.LoadAssetAtPath<TSo>(path.AssetDatabaseAssetPath);
            if (obj == null) obj = CreateSoAsset<TSo>(path);

            return obj;
        }

        /// <summary>
        /// Attempts to load an SoAsset. If not found, creates it. Path should include the .asset extension.
        /// The change will not show up in the editor without a call to AssetDatabase.Refresh()
        /// </summary>
        /// <typeparam name="TSo"></typeparam>
        /// <param name="path"></param>
        /// <param name="autoRefesh"></param>
        /// <returns></returns>
        public static ScriptableObject LoadOrCreateSoAsset(UnityPath path, string type)
        {
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path.AssetDatabaseAssetPath);
            if (obj == null) obj = CreateSoAsset(path, type);

            return obj;
        }

        /// <summary>
        /// This function is really a function contructor for a common pattern. This function 1) Attempts
        /// to load a So at a location. 2) if the so dosen't exist at the location, creates it. 3) Calls the
        /// populator on the So and the pop object, with the intention that the data in the pop object
        /// is used to populate the So. 4) Returns the So. 
        /// </summary>
        /// <returns></returns>
        public static TSo PopulateOrCreateAssetFrom<TSo, TPopulator>(UnityPath path, TPopulator pop, Action<TSo, TPopulator> populator)
            where TSo : ScriptableObject
        {
            TSo obj = LoadOrCreateSoAsset<TSo>(path);
            populator(obj, pop);
            return obj;
        }

        /// <summary>
        /// Get all instances of scriptable object of type T. from https://answers.unity.com/questions/1425758/how-can-i-find-all-instances-of-a-scriptable-objec.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            //FindAssets uses tags check documentation for more info
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  

            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }
#endif
    }
}