using HexCS.Core;

using HexUN.Behaviour;
using HexUN.Deps;
using HexUN.Framework;
using HexUN.Framework.Debugging;

using System.Collections.Generic;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Provides apparatus access to environmental services used by apparatus children
    /// </summary>
    [ExecuteAlways]
    public class ApparatusContainer : DependentBehaviour
    {
        private const string cLogCategory = nameof(ApparatusContainer);

        [Header("[ApparatusContainer]")]
        [SerializeField]
        [Tooltip("IEditorResourceDatabase used for loading and saving editable db assets")]
        private Object _iEditorResourceDatabase;

        [SerializeField]
        [Tooltip("IResourceDatabase used for loading db assets")]
        private Object _iResourceDatabase;

        [SerializeField]
        [Tooltip("All apparatus root object manged by the container")]
        private SerializationNode[] _managedApparatus;

        private IEditorResourceDatabase _editorResourceDatabase;
        private IResourceDatabase _resourceDatabase;


        protected override void SceneInitalize()
        {
            LogWriter log = new LogWriter("ApparatusContainer SceneInitialize");

            PopulateApparatusRoot();
            foreach (SerializationNode root in _managedApparatus) root.Connect(log);
            foreach (SerializationNode root in _managedApparatus) root.Trigger( ApparatusTrigger.LoadTrigger(true), log);

            OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
        }

        protected override void SceneDenitialize() 
        {
            LogWriter log = new LogWriter("ApparatusContainer SceneDenitialize");

            foreach (SerializationNode root in _managedApparatus) root.Disconnect(log);
            foreach (SerializationNode root in _managedApparatus) root.Trigger(ApparatusTrigger.LoadTrigger(false), log);
            _managedApparatus = new SerializationNode[0];

            OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
        }

        protected override void ResolveDependencies()
        {
            base.ResolveDependencies();
            UTDependency.Resolve(ref _iEditorResourceDatabase, out _editorResourceDatabase);
            UTDependency.Resolve(ref _iResourceDatabase, out _resourceDatabase);
        }

        public void HandleTrigger(string trigger)
        {
            LogWriter log = new LogWriter("ApparatusContainer HandleTrigger");

            string[] vars = trigger.Split('?');
            PathString pth = vars[0];

            if(vars.Length > 1)
            {
                string[] vars2 = vars[1].Split('=');
                _managedApparatus[0].Trigger(ApparatusTrigger.Trigger_Bool(pth.End, vars2[1] == "True", pth.RemoveAtEnd()), log);
            }
            else
            {
                _managedApparatus[0].Trigger(ApparatusTrigger.DirectEvent_Void(pth.End, pth.RemoveAtEnd()), log);
            }

            OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
        }

        public void PopulateApparatusRoot()
        {
            List<SerializationNode> roots = new List<SerializationNode>();

            foreach(Transform t in transform)
            {
                SerializationNode r = t.gameObject.GetComponent<SerializationNode>();
                if (r != null) roots.Add(r);
            }

            _managedApparatus = roots.ToArray();
        }
        

        public void HandleRequest(ApparatusRequest request)
        {
            ApparatusRequestObject aro = request.RequestObject;

            switch (aro.Type)
            {
                case EApparatusRequestType.LoadAsset:
                    HandleAssetLoadRequest(request);
                    break;
                case EApparatusRequestType.SaveAsset:
                    HandleAssetSaveRequest(request);
                    break;
                case EApparatusRequestType.LoadApparatus:
                    HandleApparatusLoadRequest(request);
                    break;
            }

        }

        // NOTE: SHARED LOGIC WITH SoApparatusConfig NEEDS DEALING WITH

        // TO DO: Make interfaces for adding this funcitonality
        private void HandleAssetLoadRequest(ApparatusRequest request)
        {
            if (!request.TryClaim(this)) return;

            AssetLoadRequestArgs args = request.RequestObject.Args as AssetLoadRequestArgs;
            GameObject prefab = null;

            if(_editorResourceDatabase == null)
            {
                request.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse(nameof(_editorResourceDatabase)), this);
                return;
            }
            else
            {
                prefab = _editorResourceDatabase?.ResolveAsset(args.Name);
            }

            request.Respond(ApparatusResponseObject.AssetResponse(prefab), this);
        }

        // TO DO: Make interfaces for adding this funcitonality
        private void HandleAssetSaveRequest(ApparatusRequest request)
        {
#if UNITY_EDITOR
            AssetSaveRequestArgs args = request.RequestObject.Args as AssetSaveRequestArgs;
            UTAssets.ConvertToAssetBundleAndSaveToDatabase(args.Instance);
            request.Respond(ApparatusResponseObject.SuccessResponse(), this);

            Debug.Log($"[{nameof(ApparatusContainer)}] Saved {args.Name} resource to local database");
#endif
        }

        // TO DO: Make interfaces for adding this funcitonality
        private void HandleApparatusLoadRequest(ApparatusRequest request)
        {
            ApparatusLoadRequestArgs args = request.RequestObject.Args as ApparatusLoadRequestArgs;

            request.Respond(
                ApparatusResponseObject.SerializeNodeResponse(_editorResourceDatabase.ResolveSerializedNode(args.Identifier)),
                this
            );
        }

    }
}