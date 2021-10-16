#if UNITY_EDITOR
using UnityEditor;
#endif

using Cysharp.Threading.Tasks;

using HexCS.Core;

using System;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Allows setting of local varaibles of a transform, by providing
    /// either deltas or positional variables
    /// </summary>
    public class DeltaTransformNode : AApparatusNode
    {
        [Header("[ApparatusDeltaTransformNode]")]
        [SerializeField]
        [Tooltip("When triggered, this node will control the values of some transform")]
        private Transform _managedTransform;

        private bool _isLocal = true;

        private Vector3 _positionDelta;
        private Vector3 _rotationDelta;
        private Vector3 _scaleDelta;

        public bool IsLocal => _isLocal;
        public Vector3 PositionDelta => _positionDelta;
        public Vector3 RotationDelta => _rotationDelta;
        public Vector3 ScaleDelta => _scaleDelta;

        /// <inheritdoc />
        public override EApparatusNodeType Type => EApparatusNodeType.DeltaTransform;

        protected async override UniTask TriggerNode(ApparatusTrigger trigger)
        {
            if (_managedTransform == null) return;

            if (trigger.TryUnpackTrigger_Input(out string type, out string name, out string value))
            {
                switch (type)
                {
                    case UTMeta.cMetaInputVector3Type:
                        try
                        {
                            Vector3 input = JsonUtility.FromJson<Vector3>(value);
                            SetTransformValue(name, input);
                        }
                        catch(Exception e) { } // ISSUE: Not the best way to do this. Non-handled exception
                        break;
                    case UTMeta.cMetaInputBoolType:
                        if (name == "isLocal" && bool.TryParse(value, out bool val)) _isLocal = val;
                        break;
                }
            }
        }

        protected override string[] ResolveMetadata()
        {
            string[] parMeta =  base.ResolveMetadata();

            string[] thisMeta = new string[]
            {
                UTMeta.InputMeta(UTMeta.cMetaInputVector3Type, "position"),
                UTMeta.InputMeta(UTMeta.cMetaInputVector3Type, "rotation"),
                UTMeta.InputMeta(UTMeta.cMetaInputVector3Type, "scale"),
                UTMeta.InputMeta(UTMeta.cMetaInputVector3Type, "position_delta"),
                UTMeta.InputMeta(UTMeta.cMetaInputVector3Type, "rotation_delta"),
                UTMeta.InputMeta(UTMeta.cMetaInputVector3Type, "scale_delta"),
                UTMeta.InputMeta(UTMeta.cMetaInputBoolType, "isLocal")
            };

            return UTArray.Combine(parMeta, thisMeta);
        }

        private void SetTransformValue(string name, Vector3 value)
        {
            if (_isLocal)
            {
                switch (name)
                {
                    case "position": _managedTransform.localPosition = value; break;
                    case "rotation": _managedTransform.localRotation = Quaternion.Euler(value); break;
                    case "scale": _managedTransform.localPosition = value; break;
                    case "position_delta": _positionDelta = value; break;
                    case "rotation_delta": _rotationDelta = value; break;
                    case "scale_delta": _scaleDelta = value; break;
                }
            }
        }

        public void Update()
        {
            if (_managedTransform == null) return;

            float delta = Time.deltaTime;

            if (_isLocal)
            {
                if (_positionDelta != Vector3.zero) _managedTransform.localPosition += _positionDelta * delta;
                if (_rotationDelta != Vector3.zero) _managedTransform.localRotation *= Quaternion.Euler(_rotationDelta * delta);
                if (_scaleDelta != Vector3.zero) _managedTransform.localScale += _scaleDelta * delta;
            }
            else
            {
                if (_positionDelta != Vector3.zero) _managedTransform.position += _positionDelta * delta;
                if (_rotationDelta != Vector3.zero) _managedTransform.localRotation *= Quaternion.Euler(_rotationDelta * delta);
                if (_scaleDelta != Vector3.zero) _managedTransform.localScale += _scaleDelta * delta;
            }
        }

#if UNITY_EDITOR
        public override bool IfUnityEditor_PopulateControls(AApparatusNode rendering, out Action render)
        {
            if (!Application.isPlaying)
            {
                render = null;
                return false;
            }
            
            void Render()
            {
                _positionDelta = EditorGUILayout.Vector3Field("Set Delta Position", _positionDelta);
                _rotationDelta = EditorGUILayout.Vector3Field("Set Delta Rotation", _rotationDelta);
                _scaleDelta = EditorGUILayout.Vector3Field("Set Delta Scale", _scaleDelta);
            }

            render = Render;
            return true;
        }
#endif
    }
}