using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.Engine
{
    /// <summary>
    /// A json serializable local transform
    /// </summary>
    [System.Serializable]
    public class SrLocalTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public static SrLocalTransform FromTransform(Transform transform)
        {
            return new SrLocalTransform()
            {
                Position = transform.localPosition,
                Rotation = transform.localRotation,
                Scale = transform.localScale
            };
        }

        public void ApplyToTransform(Transform transform)
        {
            transform.localPosition = Position;
            transform.localRotation = Rotation;
            transform.localScale = Scale;
        }
    }
}