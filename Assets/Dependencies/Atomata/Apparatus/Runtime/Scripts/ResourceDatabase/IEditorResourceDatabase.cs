using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Acts as a resource database but provides extra functionalities for updating
    /// resources present in the editor
    /// </summary>
    public interface IEditorResourceDatabase : IResourceDatabase
    {
        /// <summary>
        /// Given an asset name and a prefab, save the asset to the appropriate location
        /// </summary>
        public void UpdatePrefab(GameObject instance);
    }
}