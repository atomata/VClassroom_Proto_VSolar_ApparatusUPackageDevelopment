using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    [System.Serializable]
    public class SrApparatusNode
    {
        public EApparatusNodeType Type;
        public string Identifier;

        public SrApparatusNode(EApparatusNodeType type, string identifier)
        {
            Type = type;
            Identifier = identifier;
        }
    }
}