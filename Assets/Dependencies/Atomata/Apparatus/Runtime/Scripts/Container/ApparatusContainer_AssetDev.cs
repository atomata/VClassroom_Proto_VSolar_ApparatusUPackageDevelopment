using Atomata.VSolar.Apparatus.UnityEditor;

using Cysharp.Threading.Tasks;

using HexUN.Framework;
using HexUN.Framework.Debugging;

using UnityEngine;

namespace Atomata.VSolar.Apparatus {
    /// <summary>
    /// Manages a referenced ApparatusRootNode and reloads it OnEnable
    /// </summary>
    [ExecuteAlways]
    public class ApparatusContainer_AssetDev : MonoBehaviour
    {
        private const string cLogCategory = nameof(ApparatusContainer_AssetDev);

        [SerializeField]
        private SoApparatusConfig Config;

        [SerializeField]
        private AssetBundleNode Node;

        public async void OnEnable()
        {
            LogWriter log = new LogWriter("ApparatusContainer HandleTrigger");

            Node.RequestHandler = HandleRequest;

            if (Node == null) return;

            await Node.Trigger(ApparatusTrigger.LoadTrigger(false), log);
            Node.Disconnect();

            Node.Connect();
            await Node.Trigger(ApparatusTrigger.LoadTrigger(true), log);

            OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
        }

        public void HandleRequest(ApparatusRequest req, LogWriter log)
        {
            Config.Node_OnRequest(req, log);
        }
    }
}