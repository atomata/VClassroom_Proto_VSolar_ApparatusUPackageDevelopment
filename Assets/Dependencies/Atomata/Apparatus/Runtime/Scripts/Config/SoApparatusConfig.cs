using HexUN.Deps;
using HexUN.Framework;
using HexUN.Framework.Debugging;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Atomata.VSolar.Apparatus.UnityEditor
{
    [CreateAssetMenu(fileName = "ApparatusConfig", menuName = "Atomata/Config/ApparatusConfig")]
    public class SoApparatusConfig : ScriptableObject
    {
        private const string cLogCategory = nameof(SoApparatusConfig);

        [Header(nameof(SoApparatusConfig))]
        public string EditableDatabase = "EditableDatabase";
        public string ApparatusDatabaseName = "vsolarsystem-proto-storage";

        [SerializeField]
        [Tooltip("IEditorResourceDatabase used for loading and saving editable db assets")]
        private Object _iEditorResourceDatabase;

        [SerializeField]
        [Tooltip("IResourceDatabase used for loading db assets")]
        private Object _iResourceDatabase;

        private IEditorResourceDatabase _editorResourceDatabase;
        private IResourceDatabase _resourceDatabase;

        public IEditorResourceDatabase EditorResourceDatabase 
        { 
            get
            {
                if(_editorResourceDatabase == null)
                    UTDependency.Resolve(ref _iEditorResourceDatabase, out _editorResourceDatabase, null, true);

                return _editorResourceDatabase;
            }
        }

        public IResourceDatabase ResourceDatabase 
        { 
            get
            {
                if(_resourceDatabase == null)
                    UTDependency.Resolve(ref _iResourceDatabase, out _resourceDatabase, null, true);

                return _resourceDatabase;
            }
        }

        private static SoApparatusConfig GetConfig()
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<SoApparatusConfig>(Files.EditorOnly_ApparatusConfigPath.AssetDatabaseAssetPath);
#else
            OneHexServices.Instance.Log.Error("SoApparatusConfig", "Trying to get apparatus config, but this only works in editor.");
            return null;
#endif
        }

        public void Node_OnRequest(ApparatusRequest request, LogWriter log)
        {
            if (!request.TryClaim(this)) return;

            switch (request.RequestObject.Type)
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


        // TO DO: Make interfaces for adding this funcitonality
        private void HandleAssetLoadRequest(ApparatusRequest request)
        {
            AssetLoadRequestArgs args = request.RequestObject.Args as AssetLoadRequestArgs;
            GameObject prefab = null;

            if (EditorResourceDatabase == null)
            {
                request.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse(nameof(EditorResourceDatabase)), this);
                return;
            }
            else
            {
                prefab = EditorResourceDatabase?.ResolveAsset(args.Name);
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
                ApparatusResponseObject.SerializeNodeResponse(EditorResourceDatabase.ResolveSerializedNode(args.Identifier)), 
                this
            );
        }
    }
}