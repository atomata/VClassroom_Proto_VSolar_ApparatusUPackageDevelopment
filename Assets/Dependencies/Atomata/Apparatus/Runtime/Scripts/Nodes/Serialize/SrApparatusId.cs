using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    [System.Serializable]
    public class SrApparatusId
    {
        public EApparatusNodeType Type;
        public string Identifier;

        public SrApparatusId(EApparatusNodeType type, string identifier)
        {
            Type = type;
            Identifier = identifier;
        }
    }
}