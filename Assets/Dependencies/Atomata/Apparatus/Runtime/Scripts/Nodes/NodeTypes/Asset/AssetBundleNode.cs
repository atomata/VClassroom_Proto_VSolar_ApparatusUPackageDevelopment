#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using HexUN.Engine;
using HexUN.Engine.Utilities;
using System.Threading.Tasks;
using HexUN.Framework;
using System;
using Cysharp.Threading.Tasks;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// In play mode, provides asset managment funcitonality like event processing, loading
    /// and unloading. In editor mode, provides extra saving functionality
    /// </summary>
    public class AssetBundleNode : AApparatusNode
    {
        private const string cLogCategory = nameof(AssetBundleNode);

        private GameObject _managedChild;
        private EApparatusNodeLoadState _loadState = EApparatusNodeLoadState.Unloaded;

        public override EApparatusNodeType Type => EApparatusNodeType.Asset;

        private async UniTask Load()
        {
            if (_loadState == EApparatusNodeLoadState.Loaded) return;

            DestroyAllNonNodeChildren();
            await LoadAsset();
            _loadState = EApparatusNodeLoadState.Loaded;
        }

        private void Unload()
        {
            if (_loadState == EApparatusNodeLoadState.Unloaded) return;

            UTGameObject.Destroy_EditorSafe(_managedChild);
            _managedChild = null;
            _loadState = EApparatusNodeLoadState.Unloaded;
        }

        #region API
        /// <summary>
        /// Create an AssetEditorTotem on a target gameobject.
        /// </summary>
        public static AssetBundleNode CreateOnGameObject(GameObject parent, string assetPath) {
            AssetBundleNode tot = parent.AddComponent<AssetBundleNode>();
            tot.Identifier = assetPath;
            return tot;
        }

        private async Task LoadAsset()
        {
            // check that this object has not been desroyed
            if (this == null)
            {
                OneHexServices.Instance.Log.Warn(cLogCategory, $"Could node load asset for {gameObject.name} because this == null");
                return;
            }

            // want to load ethereal
            GameObject ethereal = null;

            // Attempt to load, may error if timeout occurs
            try
            {
                ethereal = await LoadAsset_Prefab(Identifier);
            }
            catch (Exception e)
            {
                OneHexServices.Instance.Log.Error(cLogCategory, $"Error loading etheral asset", e);
            }

            // if etheral is still null, show a generic error object
            if (ethereal == null)
            {
                LoadEtherealAsFailureObject();
                return;
            }

            ethereal.transform.SetParent(transform, false);
            _managedChild = ethereal;

            // Activate children. By doing this, the children will become part of the tree
            // and the load trigger should automatically propogate too them
            if(_managedChild != null)
                Connect();
        }
        #endregion

        protected override async UniTask TriggerNode(ApparatusTrigger trigger)
        {
            switch (trigger.Type)
            {
                case ETriggerType.Load:
                    if(trigger.TryUnpackTrigger_Load(out bool shouldLoad))
                    {
                        if (shouldLoad) await Load();
                        else Unload();
                    }
                    break;
            }
        }

        private async Task<GameObject> LoadAsset_Prefab(string asset)
        {
            ApparatusRequestObject req = ApparatusRequestObject.LoadAsset(asset);
            ApparatusResponseObject res = null;
            try
            {
                res = await SendRequestAsync(req);
            }
            catch(Exception e)
            {
                OneHexServices.Instance.Log.Error(cLogCategory, $"Failed to load prefab {asset} because request failed. {e.GetType()}: {e.Message}");
                return null;
            }

            if(res.Status == EApparatusResponseStatus.Failed_ReferenceMissing)
            {
                OneHexServices.Instance.Log.Error(cLogCategory, $"Failed to load prefab {asset} because a reference to {res.ResponseData as string} was missing");
                return null;
            }

            GameObject prefab = res.ResponseData as GameObject;

#if UNITY_EDITOR
            GameObject loaded = null;
            
            if(prefab != null)
            {
                // if this works, then it was loadable from the editor asset database
                UnityEngine.Object instance = PrefabUtility.InstantiatePrefab(prefab);
                loaded = instance as GameObject;

                // otherwise, try to instantiate as if it's an asset
                if(loaded == null)
                {
                    loaded = Instantiate(prefab);
                }
            }

            // if asset dosen't exist
            if (loaded == null)
            {
                Debug.LogError($"[AssetEditorTotem] Failed to load prefab {asset}. This means that the prefab is not available in the Assets folder because it was created in another project.");
                return null;
            }
#else
            GameObject loaded = Instantiate(prefab);
#endif

            loaded.name = $"[LoadedAsset] {prefab.name}";

            loaded.transform.DoToSelfAndChildren(
                t => t.gameObject.hideFlags = HideFlags.NotEditable | HideFlags.DontSave
            );

            return loaded;
        }

        private void LoadEtherealAsFailureObject()
        {
            _managedChild = UTGameObject.CreateFailureObject();
            _managedChild.transform.SetParent(transform, false);
        }
    }
}
