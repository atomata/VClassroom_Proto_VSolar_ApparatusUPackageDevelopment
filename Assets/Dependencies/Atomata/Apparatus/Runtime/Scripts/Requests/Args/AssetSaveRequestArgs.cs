using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Arguments for an asset load request. This request is made to the environment
    /// and expects a prefab in return.
    /// </summary>
    public class AssetSaveRequestArgs
    {
        /// <summary>
        /// What is the name of the asset that should be loaded
        /// </summary>
        public string Name;

        /// <summary>
        /// Instance of the gameobject that is trying to be saved
        /// </summary>
        public GameObject Instance;

        public AssetSaveRequestArgs(string name, GameObject instance)
        {
            Name = name;
            Instance = instance;
        }
    }
}