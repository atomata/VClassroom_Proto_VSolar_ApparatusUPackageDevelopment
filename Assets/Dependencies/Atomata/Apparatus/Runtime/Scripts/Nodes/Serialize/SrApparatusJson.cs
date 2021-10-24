using HexUN.Engine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    [System.Serializable]
    public class SrApparatus
    {
        public SrApparatusNode Id;

        public SrLocalTransform[] Transforms;
        public SrApparatusNode[] Children;
        public SrApparatusMetadata Metadata;

        public string Identifier => Id.Identifier;
        public EApparatusNodeType Type => Id.Type;

        public SrApparatus(SerializationNode node)
        {
            // Get the id
            Id = new SrApparatusNode(EApparatusNodeType.Apparatus, node.Identifier);

            // Get the direct children ids
            AApparatusNode[] children = node.Children;
            Children = new SrApparatusNode[children.Length];
            Transforms = new SrLocalTransform[children.Length];
            for (int i = 0; i<children.Length; i++)
            {
                Children[i] = children[i].SerializableId();
                Transforms[i] = SrLocalTransform.FromTransform(children[i].transform);
            }

            Metadata = node.GetMetadata();
        }
    }
}