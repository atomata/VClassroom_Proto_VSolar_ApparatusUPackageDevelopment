using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Managed actions on a ResourceDatabase 
    /// </summary>
    public interface IResourceDatabase
    {
        /// <summary>
        /// Get an asset based on the assets name. Returns null if the asset could
        /// not be resolved. The provided GameObject is a prefab. 
        /// </summary>
        public GameObject ResolveAsset(string identifier);

        /// <summary>
        /// Find the serialized node with the given identifier and deserialize
        /// it to a <see cref="SrNode"/>
        /// </summary>
        public SrNode ResolveSerializedNode(string identifier);
    }
}