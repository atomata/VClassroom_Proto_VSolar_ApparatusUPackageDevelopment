using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Arguments for an asset load request. This request is made to the environment
    /// and expects a prefab in return.
    /// </summary>
    public class AssetLoadRequestArgs
    {
        /// <summary>
        /// What is the name of the asset that should be loaded. This will be used to do an asset lookup
        /// in a database somewhere, so it's important that the name corresponds to an existing asset. 
        /// </summary>
        public string Name;

        public AssetLoadRequestArgs(string name)
        {
            Name = name;
        }
    }
}