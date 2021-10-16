#if UNITY_EDITOR
using Atomata.VSolar.Apparatus.UnityEditor;

using System;
using UnityEditor;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Contains context menu controls used by AparatusNodeObjects
    /// </summary>
    public abstract partial class AApparatusNode
    {
        [ContextMenu("Reload")]
        private void ReloadNode()
        {
            UnloadNode();
            LoadNode();
        }

        [ContextMenu("Load")]
        private void LoadNode()
        {
            void Load(AApparatusNode a) => a.Trigger(ApparatusTrigger.LoadTrigger(true));
            DoNodeAction(Load, true);
        }

        [ContextMenu("Unload")]
        private void UnloadNode()
        {
            void Unload(AApparatusNode a) => a.Trigger(ApparatusTrigger.LoadTrigger(false));
            DoNodeAction(Unload);
        }

        [ContextMenu("Connect")]
        private void ActivateNode()
        {
            void Connect(AApparatusNode a) => a.Connect();
            DoNodeAction(Connect);
        }

        [ContextMenu("Unconnect")]
        private void UnconnectNode()
        {
            void Unconnect(AApparatusNode a) => a.Disconnect();
            DoNodeAction(Unconnect);
        }

        private void DoNodeAction(Action<AApparatusNode> nodeAction, bool catchReq = false)
        {
            SoApparatusConfig config = Files.EditorOnly_ApparatusConfig;

            if (catchReq)
            {
                RequestHandler = config.Node_OnRequest;
            }
            nodeAction(this);
        }
    }
}
#endif