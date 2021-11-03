using Atomata.VSolar.Apparatus.UnityEditor;

using Cysharp.Threading.Tasks;

using HexUN.Framework;

using UnityEngine;

namespace Atomata.VSolar.Apparatus {
    /// <summary>
    /// Manages a referenced ApparatusRootNode and reloads it OnEnable
    /// </summary>
    [ExecuteAlways]
    public class ApparatusContainer_Dev : MonoBehaviour
    {
        private const string cLogCategory = "ApparatusContainer_Dev";

        private IPrefabProvider PrefabProvider;

        [SerializeField]
        private SoApparatusConfig Config;

        [SerializeField]
        private SerializationNode Node;

        public async void OnEnable()
        {
            if(Config == null)
            {
                OneHexServices.Instance.Log.Error(cLogCategory, "An SoApparatus Config must be assigned for editor containers");
            }

            PrefabProvider = new EditableDatabasePrefabProvider(Config);

            if (Node == null) return;

            await Node.Trigger(ApparatusTrigger.LoadTrigger(false));
            Node.Disconnect();

            Node.Connect();
            await Node.Trigger(ApparatusTrigger.LoadTrigger(true));
        }

        public void HandleRequest(ApparatusRequest req)
        {
            switch (req.RequestObject.Type)
            {
                case EApparatusRequestType.LoadAsset:
                    HandleAssetLoadRequest(req);
                    break;
            }

            // TO DO: Make all requests get handled at this level, instead of in config
            Config.Node_OnRequest(req);
        }

        private async void HandleAssetLoadRequest(ApparatusRequest req)
        {
            if (!req.TryClaim(this)) return;

            AssetLoadRequestArgs args = req.RequestObject.Args as AssetLoadRequestArgs;

            if (args == null)
            {
                OneHexServices.Instance.Log.Error(cLogCategory, "Failed to get AssetLoadRequestArgs from AssetLoadRequest");
                req.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse(args.Name), this);
                return;
            }

            GameObject prefab = await PrefabProvider.Provide(args.Name);

            if (prefab == null)
            {
                OneHexServices.Instance.Log.Warn(cLogCategory, $"Could not load prefab with name {args.Name}, does not exist");
                req.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse(args.Name), this);
            }

            req.Respond(ApparatusResponseObject.AssetResponse(prefab), this);
        }
    }
}