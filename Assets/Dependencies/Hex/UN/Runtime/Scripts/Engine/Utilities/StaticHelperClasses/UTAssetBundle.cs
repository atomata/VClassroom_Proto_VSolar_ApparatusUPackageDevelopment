using HexUN.Data;

using System;
using System.Linq;

using UnityEngine;

namespace HexUN.Engine.Utilities
{
    /// <summary>
    /// Utiltiies and extension methods for dealing with AssetBundles
    /// </summary>
    public static class UTAssetBundle
    {
        /// <summary>
        /// Try to load an asset bundle that is already loaded. If it's not
        /// loaded yet, return false
        /// </summary>
        public static bool TryGetLoadedBundle(string name, out AssetBundle loaded)
        {
            AssetBundle[] loadedBundles = AssetBundle.GetAllLoadedAssetBundles().ToArray();

            foreach (AssetBundle bund in loadedBundles)
            {
                if (bund.name == name)
                {
                    loaded = bund;
                    return true;
                }
            }

            loaded = null;
            return false;
        }

        /// <summary>
        /// Shortcut to load asset bundles that contain one GameObject. If unload is false, the gameobject
        /// will be loaded from an already loaded assetbundle if it exists. Otherwise, the assetbundle
        /// will be unloaded if it already exists
        /// </summary>
        public static bool TryLoadGameObjectAssetBundle(UnityPath path, out GameObject go, bool unload = false)
        {
            // See if already loaded and handle
            if(TryGetLoadedBundle(path.Path.End, out AssetBundle bund))
            {
                if(unload)
                {
                    bund.Unload(true);
                    bund = null;
                }
            }

            // if not loaded, load
            if(bund == null)
            {
                bund = AssetBundle.LoadFromFile(path);

                if (bund == null)
                {
                    go = null;
                    return false;
                }
            }

            // Get the assets inside
            UnityEngine.Object[] obj = bund.LoadAllAssets();

            // Validate length
            if(obj.Length != 1)
            {
                Debug.LogWarning($"GameObject AssetBundle should have 1 asset, but has {obj.Length}");
                go = null;
                return false;
            }

            go = obj[0] as GameObject;

            bool isSuccess = go != null;
            if (!isSuccess) Debug.LogWarning($"Failed to cast asset in GameObject AssetBundle to GameObject");
            return isSuccess;
        }
    }
}