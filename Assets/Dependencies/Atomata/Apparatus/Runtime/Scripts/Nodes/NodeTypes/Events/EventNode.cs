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
    public class EventNode : AApparatusNode
    {
        [Header("[EventNode]")]

        [SerializeField]
        [Tooltip("associted node")]
        private AApparatusNode _associatedNode;

        [SerializeField]
        [Tooltip("Names of void triggers")]
        private string[] _voidEventNames;

        [SerializeField]
        [Tooltip("The triggers")]
        private UnityEvent[] _voidEvents;

        [SerializeField]
        [Tooltip("Names of bool triggers")]
        private string[] _boolEventNames;

        [SerializeField]
        [Tooltip("The triggers")]
        private BooleanReliableEvent[] _boolEvents;

        /// <summary>
        /// All void trigger names available
        /// </summary>
        public string[] VoidEvents => _voidEventNames;

        public UiMeta[] VoidEventMetas;

        /// <summary>
        /// All bool trigger names available
        /// </summary>
        public string[] BoolEvents => _boolEventNames;

        public UiMeta[] BoolEventMetas;

        public override string NodeType => "Event";

     
        public void Invoke_Void(string name)
        {
            int index = Array.IndexOf(_voidEventNames, name);

            if(index > -1)
            {
                if (_voidEvents.Length > index) _voidEvents[index]?.Invoke();
            }
        }

        /// <summary>
        /// Invoke a bool event. Useable from UnityEvents. Must provide argument ?bool
        /// </summary>
        public void Invoke_Bool(string command)
        {
            string[] args = command.Split('?');

            if (args.Length != 2)
                return;
            
            int index = Array.IndexOf(_boolEventNames, args[0]);

            if (index > -1)
            {
                if (_boolEvents.Length > index) _boolEvents[index]?.Invoke(bool.Parse(args[1]));
            }
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
                        index = Array.IndexOf(_voidEventNames, name);
                        if (index != -1) _voidEvents[index].Invoke();
                        break;
                    case UTMeta.cMetaInputBoolType:
                        index = Array.IndexOf(_boolEventNames, name);
                        if (index != -1) _boolEvents[index].Invoke(bool.Parse(value));
                        break;
                }
            }
        }

        protected override string[] ResolveMetadata()
        {
            string[] b = base.ResolveMetadata();

            List<string> metas = new List<string>();
            metas.Add(UTMeta.AssociatedNodeMeta(_associatedNode?.Path().ToString('/')));

            for (int i = 0; i < VoidEvents.Length; i++)
                metas.Add(UTMeta.InputMeta(UTMeta.cMetaInputVoidType, VoidEvents[i], VoidEventMetas.Length > i ?VoidEventMetas[i] : null));
            

            for (int i = 0; i < BoolEvents.Length; i++)
                metas.Add(UTMeta.InputMeta(UTMeta.cMetaInputBoolType, BoolEvents[i], BoolEventMetas.Length > i ? BoolEventMetas[i] : null));
            

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

            void Render()
            {
                string path = Path(rendering.Identifier);

                // voids
                for(int i = 0; i<_voidEventNames.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{path}\\{_voidEventNames[i]}");

                    if (GUILayout.Button("Do", EditorStyles.miniButtonRight)) 
                        _voidEvents[i]?.Invoke();

                    EditorGUILayout.EndHorizontal();
                }

                // bools
                for (int i = 0; i < _boolEventNames.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{path}\\{_boolEventNames[i]}");
                    
                    if(GUILayout.Button("True", EditorStyles.miniButtonLeft))
                        _boolEvents[i]?.Invoke(true);

                    if(GUILayout.Button("False", EditorStyles.miniButtonRight))
                        _boolEvents[i]?.Invoke(false);

                    EditorGUILayout.EndHorizontal();
                }
            }

            render = Render;
            return true;
        }
#endif
    }
}