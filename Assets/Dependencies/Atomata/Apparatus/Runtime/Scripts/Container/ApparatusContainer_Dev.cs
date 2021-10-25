using Atomata.VSolar.Apparatus.UnityEditor;
using Atomata.VSolar.Utilities;

using Cysharp.Threading.Tasks;

using HexUN.Framework;

using UnityEngine;

namespace Atomata.VSolar.Apparatus {
    /// <summary>
    /// Manages a referenced ApparatusRootNode and reloads it OnEnable
    /// </summary>
    [ExecuteAlways]
    public class ApparatusContainer_Dev : MonoBehaviour, IRequestHandler
    {
        private const string cLogCategory = nameof(ApparatusContainer_Dev);

        [SerializeField]
        private SoApparatusConfig Config;

        [SerializeField]
        private SerializationNode Node;

        public async void OnEnable()
        {
            LogWriter log = new LogWriter("Dev Container Load");

            if (Node == null)
            {
                log.AddWarning($"Cannot initalize {nameof(SerializationNode)} because node is null");
                OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
                return;
            }

            log.AddInfo("Triggering Unload");
            await Node.Trigger(ApparatusTrigger.LoadTrigger(false));

            log.AddInfo("Triggering Disconnect");
            Node.Disconnect();

            log.AddInfo("Triggering Connected");
            Node.Connect();

            log.AddInfo("Triggering Load");
            await Node.Trigger(ApparatusTrigger.LoadTrigger(true));

            log.AddInfo("Initializing complete");
            OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
        }

        public void HandleRequest(ApparatusRequest req, LogWriter log)
        {
            OneHexServices.Instance.Log.Info(cLogCategory, $"Received request and handling", false);
            Config.Node_OnRequest(req, log);
        }
    }
}