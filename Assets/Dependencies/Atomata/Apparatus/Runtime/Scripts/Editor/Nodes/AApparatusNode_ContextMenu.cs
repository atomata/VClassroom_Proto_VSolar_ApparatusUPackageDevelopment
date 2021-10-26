#if UNITY_EDITOR
using Atomata.VSolar.Apparatus.UnityEditor;

using HexUN.Framework;
using HexUN.Framework.Debugging;

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
            LogWriter log = new LogWriter("ApparatusContainer HandleTrigger");

            void Load(AApparatusNode a) => a.Trigger(ApparatusTrigger.LoadTrigger(true), log);
            DoNodeAction(Load, true);

            OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
        }

        [ContextMenu("Unload")]
        private void UnloadNode()
        {
            LogWriter log = new LogWriter("ApparatusContainer HandleTrigger");

            void Unload(AApparatusNode a) => a.Trigger(ApparatusTrigger.LoadTrigger(false), log);
            DoNodeAction(Unload);

            OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
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