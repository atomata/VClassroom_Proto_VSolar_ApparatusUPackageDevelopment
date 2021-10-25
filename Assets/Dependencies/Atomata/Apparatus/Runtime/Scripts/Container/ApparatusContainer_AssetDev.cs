using Atomata.VSolar.Apparatus.UnityEditor;
using Atomata.VSolar.Utilities;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Atomata.VSolar.Apparatus {
    /// <summary>
    /// Manages a referenced ApparatusRootNode and reloads it OnEnable
    /// </summary>
    [ExecuteAlways]
    public class ApparatusContainer_AssetDev : MonoBehaviour
    {
        [SerializeField]
        private SoApparatusConfig Config;

        [SerializeField]
        private AssetBundleNode Node;

        public async void OnEnable()
        {
            Node.RequestHandler = HandleRequest;

            if (Node == null) return;

            await Node.Trigger(ApparatusTrigger.LoadTrigger(false));
            Node.Disconnect();

            Node.Connect();
            await Node.Trigger(ApparatusTrigger.LoadTrigger(true));
        }

        public void HandleRequest(ApparatusRequest req, LogWriter log)
        {
            Config.Node_OnRequest(req, log);
        }
    }
}