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
using HexCS.Core;
using System.Linq;
using System.Collections.Generic;
using HexUN.Framework.Debugging;

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

        public string AssetBundleKey;

        public override string NodeType => "AssetBundle";

        private async UniTask Load(LogWriter log)
        {
            log.AddInfo(cLogCategory, NodeIdentityString, $"Performing assetbundle load");

            if (_loadState == EApparatusNodeLoadState.Loaded)
            {
                log.AddInfo(cLogCategory, NodeIdentityString, $"Already in loaded state. Aborting");
                return;
            }

            DestroyAllNonNodeChildren();
            await LoadAsset(log);
            _loadState = EApparatusNodeLoadState.Loaded;

            log.AddInfo(cLogCategory, NodeIdentityString, $"Load complete");
        }

        private void Unload(LogWriter log)
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

        private async Task LoadAsset(LogWriter log)
        {
            log.AddInfo(cLogCategory, NodeIdentityString, $"Starting async load operation");

            // check that this object has not been desroyed
            if (this == null)
            {
                log.AddWarning(cLogCategory, NodeIdentityString, $"Could node load asset for {gameObject.name} because this == null");
                return;
            }

            // want to load ethereal
            GameObject ethereal = null;

            // Attempt to load, may error if timeout occurs
            try
            {
                ethereal = await LoadAsset_Prefab(AssetBundleKey, log);
            }
            catch (Exception e)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"Error loading etheral asset. \n{e.Message}\n{e.StackTrace}");
            }

            // if etheral is still null, show a generic error object
            if (ethereal == null)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"After full load operation prefab is still null. Loading failure object");
                LoadEtherealAsFailureObject();
                return;
            }

            ethereal.transform.SetParent(transform, false);
            _managedChild = ethereal;

            // Activate children. By doing this, the children will become part of the tree
            // and the load trigger should automatically propogate too them
            if(_managedChild != null)
                Connect(log);

            log.AddInfo(cLogCategory, NodeIdentityString, $"Async load operation complete");
        }
        #endregion

        protected override async UniTask TriggerNode(ApparatusTrigger trigger, LogWriter log)
        {
            log.AddInfo(cLogCategory, NodeIdentityString, $"<TRIG#{trigger.GetHashCode()}> Applying trigger to node. Trigger Type: {trigger.Type}");

            switch (trigger.Type)
            {
                case ETriggerType.Load:
                    if(trigger.TryUnpackTrigger_Load(out bool shouldLoad))
                    {
                        if (shouldLoad) await Load(log);
                        else Unload(log);
                    }
                    break;
            }

            log.AddInfo(cLogCategory, NodeIdentityString, $"<TRIG#{trigger.GetHashCode()}> Complete");
        }

        private async Task<GameObject> LoadAsset_Prefab(string asset, LogWriter log)
        {
            log.AddInfo(cLogCategory, NodeIdentityString, $"Starting async load of prefab {asset}");

            ApparatusRequestObject req = ApparatusRequestObject.LoadAsset(asset);
            ApparatusResponseObject res = null;

            try
            {
                res = await SendRequestAsync(req, log);
            }
            catch(Exception e)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"Failed prefab because request attempt threw an exception. \nException:{e?.Message}\n{e?.StackTrace}");
                return null;
            }

            if(res.Status == EApparatusResponseStatus.Failed_ReferenceMissing)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"Failed to load prefab {asset} because a reference to {res.ResponseData as string} was missing");
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
                log.AddError(cLogCategory, NodeIdentityString, $"[AssetEditorTotem] Failed to load prefab {asset}. This means that the prefab is not available in the Assets folder because it was created in another project.");
                return null;
            }
#else
            GameObject loaded = Instantiate(prefab);
#endif

            loaded.name = $"[LoadedAsset] {prefab.name}";

            loaded.transform.DoToSelfAndChildren(
                t => t.gameObject.hideFlags = HideFlags.NotEditable | HideFlags.DontSave
            );

            log.AddInfo(cLogCategory, NodeIdentityString, $"Loaded object: {loaded.name}");
            return loaded;
        }

        private void LoadEtherealAsFailureObject()
        {
            _managedChild = UTGameObject.CreateFailureObject();
            _managedChild.transform.SetParent(transform, false);
        }

        /// <inheritdoc/>
        protected override string[] ResolveMetadata()
        {
            string[] baseMeta =  base.ResolveMetadata();
            return UTArray.Combine(baseMeta, new string[] { UTMeta.KeyMeta(AssetBundleKey) });
        }
    }
}
