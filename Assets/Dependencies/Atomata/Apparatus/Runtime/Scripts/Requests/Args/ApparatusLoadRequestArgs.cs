using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Arguments for an asset load request. This request is made to the environment
    /// and expects a prefab in return.
    /// </summary>
    public class ApparatusLoadRequestArgs
    {
        /// <summary>
        /// What is the name of the apparatus that should be loaded
        /// </summary>
        public string Identifier;

        public ApparatusLoadRequestArgs(string name)
        {
            Identifier = name;
        }
    }
}