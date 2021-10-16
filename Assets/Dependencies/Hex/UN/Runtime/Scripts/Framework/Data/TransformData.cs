using UnityEngine;

namespace HexUN.Data
{
    /// <summary>
    /// Light weight data struct for Transform data
    /// </summary>
    public struct TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public void ApplyToTransform(Transform trans)
        {
            trans.position = Position;
            trans.rotation = Rotation;
            trans.localScale = Scale;
        }

        public void ApplyToTransformLocal(Transform trans)
        {
            trans.localScale = Position;
            trans.localRotation = Rotation;
            trans.localScale = Scale;
        }
    }
}