#if UNITY_EDITOR
using UnityEditor;
#endif

using Cysharp.Threading.Tasks;

using HexCS.Core;

using HexUN.Events;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;
using HexUN.Framework.Debugging;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// A group of named UnityEvents
    /// </summary>
    public class CameraFocusNode : AApparatusNode
    {
        private const string cLogCategory = nameof(CameraFocusNode);
        
        [Header("[CameraFocusNode]")]

        [SerializeField]
        [Tooltip("Associated node")]
        private AApparatusNode _associatedNode;

        public bool ShowCamera;

        private Camera _mainCamera;
        private Camera _mangedCamera;
        
        public override string NodeType => "CameraFocus";

        private void OnValidate()
        {
            if (_mangedCamera == null)
            {
                _mangedCamera = gameObject.GetComponent<Camera>();

                if (_mangedCamera == null)
                {
                    _mangedCamera = gameObject.AddComponent<Camera>();
                    _mangedCamera.hideFlags = HideFlags.NotEditable;
                }
            }

            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            if(_mainCamera != null) _mainCamera.enabled = !ShowCamera;
            if(Camera.current != null) Camera.current.enabled = !ShowCamera;
            _mangedCamera.enabled = ShowCamera;
        }

        protected async override UniTask TriggerNode(ApparatusTrigger trigger, LogWriter log)
        {
            if(trigger.Type == ETriggerType.Load)
            {
                _associatedNode = Parent;
            }
            if (trigger.TryUnpackTrigger_Input(out string type, out string name, out string value))
            {
                int index = -1;

                switch (type)
                {
                    case UTMeta.cMetaInputVoidType:
                        if (name != "focus")
                        {
                            log.AddInfo(cLogCategory, cLogCategory, "Event is not a focus event. Only focus events are supported by camera");
                            return;
                        }
                        
                        ApparatusRequestObject req = ApparatusRequestObject.CameraFocus(transform);
                        await SendRequestAsync(req, log);
                        break;
                }
            }
        }

        protected override string[] ResolveMetadata()
        {
            string[] b = base.ResolveMetadata();

            List<string> metas = new List<string>();
            metas.Add(UTMeta.AssociatedNodeMeta(_associatedNode?.Path().ToString('/')));
            metas.Add(UTMeta.InputMeta(UTMeta.cMetaInputVoidType, "focus"));
            return UTArray.Combine(b, metas.ToArray());
        }

#if UNITY_EDITOR
        public override bool IfUnityEditor_PopulateControls(AApparatusNode rendering, out Action render)
        {
            if(!Application.isPlaying)
            {
                render = null;
                return false;
            }

            render = Render;
            return true;

            async void  Render()
            {
                
                if (GUILayout.Button("Focus", EditorStyles.miniButtonRight))
                {
                    LogWriter log = new LogWriter("Editor Debug");
                    ApparatusRequestObject req = ApparatusRequestObject.CameraFocus(transform);
                    await SendRequestAsync(req, log);
                    log.PrintToConsole(cLogCategory);
                }
                
            }
        }
#endif
    }
}