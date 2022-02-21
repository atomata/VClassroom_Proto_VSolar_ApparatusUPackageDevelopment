using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    [System.Serializable]
    public class SrNode
    {
        public string Identifier;
        public string Type;
        public string Transform;
        public string[] MetaData;
        public SrNode[] Children;
    }
}