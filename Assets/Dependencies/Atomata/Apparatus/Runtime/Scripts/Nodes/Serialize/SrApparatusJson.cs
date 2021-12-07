using HexUN.Engine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    [System.Serializable]
    public class SrApparatus
    {
        public string ApparatusKey;
        public SrApparatusMetadata Metadata;

        public SrApparatus(SerializationNode node)
        {
            // Get the id
            ApparatusKey = node.ApparatusKey;
            Metadata = node.GetMetadata();
        }
    }
}