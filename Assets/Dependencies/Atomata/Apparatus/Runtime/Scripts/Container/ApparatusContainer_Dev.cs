using Atomata.VSolar.Apparatus.UnityEditor;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Atomata.VSolar.Apparatus {
    /// <summary>
    /// Manages a referenced ApparatusRootNode and reloads it OnEnable
    /// </summary>
    [ExecuteAlways]
    public class ApparatusContainer_Dev : MonoBehaviour
    {
        [SerializeField]
        private SoApparatusConfig Config;

        [SerializeField]
        private SerializationNode Node;

        public async void OnEnable()
        {
            if (Node == null) return;

            await Node.Trigger(ApparatusTrigger.LoadTrigger(false));
            Node.Disconnect();

            Node.Connect();
            await Node.Trigger(ApparatusTrigger.LoadTrigger(true));
        }

        public void HandleRequest(ApparatusRequest req)
        {
            Config.Node_OnRequest(req);
        }
    }
}