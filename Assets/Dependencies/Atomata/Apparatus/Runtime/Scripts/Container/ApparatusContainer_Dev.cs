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
    public class ApparatusContainer_Dev : MonoBehaviour, IRequestHandler
    {
        private const string cLogCategory = nameof(ApparatusContainer_Dev);
        private IPrefabProvider PrefabProvider;
        private IApparatusProvider ApparatusProvider;

        [SerializeField]
        private SoApparatusConfig Config;

        [SerializeField]
        private SerializationNode Node;

        public async void OnEnable()
        {
            LogWriter log = new LogWriter("Dev Container Load");
            if(Config == null)
            {
                OneHexServices.Instance.Log.Error(cLogCategory, "An SoApparatus Config must be assigned for editor containers");
            }

            PrefabProvider = new EditableDatabasePrefabProvider(Config);
            ApparatusProvider = new EditableDatabaseApparatusProvider(Config);

            if (Node == null) return;

            if (Node == null)
            {
                log.AddWarning(cLogCategory, cLogCategory, $"Cannot initalize {nameof(SerializationNode)} because node is null");
                OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
                return;
            }

            log.AddInfo(cLogCategory, cLogCategory, "Triggering Unload");
            await Node.Trigger(ApparatusTrigger.LoadTrigger(false), log);

            log.AddInfo(cLogCategory, cLogCategory, "Triggering Disconnect");
            Node.Disconnect();

            log.AddInfo(cLogCategory, cLogCategory, "Triggering Connected");
            Node.Connect();

            log.AddInfo(cLogCategory, cLogCategory, "Triggering Load");
            await Node.Trigger(ApparatusTrigger.LoadTrigger(true), log);

            log.AddInfo(cLogCategory, cLogCategory, "Initializing complete");
            OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
        }

        public void HandleRequest(ApparatusRequest req, LogWriter log)
        {
            log.AddInfo(cLogCategory, cLogCategory, "Request received, handling...");
            UTApparatusRequest.HandleRequest(PrefabProvider, ApparatusProvider, req, this, cLogCategory, log);
        }
    }
}