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
        private const string cLogCategory = nameof(ApparatusContainer_Dev);

        [SerializeField]
        private SoApparatusConfig Config;

        [SerializeField]
        private SerializationNode Node;

        public async void OnEnable()
        {
            if (Node == null)
            {
                OneHexServices.Instance.Log.Warn(cLogCategory, $"Cannot initalize {nameof(SerializationNode)} because node is null", false);
                return;
            }

            OneHexServices.Instance.Log.Info(cLogCategory, "Initializing managed serialization node by Unloading, Disconnecting, Connecting and Reloading", false);

            await Node.Trigger(ApparatusTrigger.LoadTrigger(false));
            Node.Disconnect();

            Node.Connect();
            await Node.Trigger(ApparatusTrigger.LoadTrigger(true));

            OneHexServices.Instance.Log.Info(cLogCategory, "Initializing managed serialization node complete", false);

        }

        public void HandleRequest(ApparatusRequest req)
        {
            OneHexServices.Instance.Log.Info(cLogCategory, $"Received request and handling", false);
            Config.Node_OnRequest(req);
        }
    }
}