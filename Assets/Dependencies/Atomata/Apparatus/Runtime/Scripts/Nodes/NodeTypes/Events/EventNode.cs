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

        /// <summary>
        /// All bool trigger names available
        /// </summary>
        public string[] BoolEvents => _boolEventNames;

        public override EApparatusNodeType Type =>  EApparatusNodeType.Event;

        public override string NodeType => "Event";

        public void Invoke_Void(string name)
        {
            int index = Array.IndexOf(_voidEventNames, name);

            if(index > -1)
            {
                if (_voidEvents.Length > index) _voidEvents[index]?.Invoke();
            }
        }

        public void Invoke_Bool(string name, bool val)
        {
            int index = Array.IndexOf(_boolEventNames, name);

            if (index > -1)
            {
                if (_boolEvents.Length > index) _boolEvents[index]?.Invoke(val);
            }
        }

        protected async override UniTask TriggerNode(ApparatusTrigger trigger, LogWriter log)
        {
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
            foreach (string trig in VoidEvents) metas.Add(UTMeta.InputMeta(UTMeta.cMetaInputVoidType, trig));
            foreach (string trig in BoolEvents) metas.Add(UTMeta.InputMeta(UTMeta.cMetaInputBoolType, trig));

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