using System.Collections;
using System.Collections.Generic;
using Atomata.VSolar.Apparatus;
using Cysharp.Threading.Tasks;
using HexCS.Core;
using HexUN.Framework.Debugging;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class HumanMetaNode : AApparatusNode
    {
        [Header("[HumanMeta]")] 
        [SerializeField] private string Name;
        [SerializeField] private string Description;
        
        public override string NodeType { get; } = nameof(HumanMetaNode);
        
        protected async override UniTask TriggerNode(ApparatusTrigger trigger, LogWriter log)
        {
            return;
        }

        protected override string[] ResolveMetadata()
        {
            string[] data = base.ResolveMetadata();

            return UTArray.Combine(
                data,
                new[]
                {
                    UTMeta.BasicMeta("name", Name),
                    UTMeta.BasicMeta("description", Description)
                }
            );
        }
    }
}