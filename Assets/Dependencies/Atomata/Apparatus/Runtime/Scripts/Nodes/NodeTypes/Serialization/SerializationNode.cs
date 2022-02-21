using Cysharp.Threading.Tasks;

using HexCS.Core;

using HexUN.Engine.Utilities;
using HexUN.Framework;
using HexUN.Framework.Debugging;

using System;
using System.Collections.Generic;
using System.Linq;
using Atomata.VSolar.Apparatus;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Acts as the root node of an apparatus object
    /// </summary>
    public partial class SerializationNode : AApparatusNode
    {
        private const string cLogCategory = nameof(SerializationNode);

        [Header("[Requests]")]
        [SerializeField]
        [Tooltip("Event used by the apparatus to make requests to the environment")]
        private UEApparatusRequest _onRequest;

        public override string NodeType => "Serialization";

        public string ApparatusKey;

        private EApparatusNodeLoadState _loadState = EApparatusNodeLoadState.Unloaded;

        protected override void OnConnected()
        {
            base.OnConnected();

            if (IsRoot) return;

            AApparatusNode node = Parent;
            SerializationNode root = node as SerializationNode;

            while(root == null)
            {
                if (node.IsRoot) return;
                node = node.Parent;
                root = node as SerializationNode;
            }

            root.RegisterChildRequests(this);
        }

        public void RegisterChildRequests(SerializationNode childRoot)
        {
            // BIT OF A HACK
            // This sets the event listener to the same listener as the root, essentially
            // resulting in all ApparatusRoots using the same event listener
            childRoot._onRequest = _onRequest;
        }

        protected override async UniTask TriggerNode(ApparatusTrigger trigger, LogWriter log)
        {
            log.AddInfo(cLogCategory, NodeIdentityString, $"<TRIG#{trigger.GetHashCode()}> Applying trigger to node. Trigger Type: {trigger.Type}");

            switch (trigger.Type)
            {
                case ETriggerType.Load:
                    if(trigger.TryUnpackTrigger_Load(out bool shouldLoad))
                    {
                        if (shouldLoad) await Load(log);
                        else Unload();
                    }
                    break;
            }

            log.AddInfo(cLogCategory, NodeIdentityString, $"<TRIG#{trigger.GetHashCode()}> Complete");
        }

        private async UniTask Load(LogWriter log)
        {
            log.AddInfo(cLogCategory, NodeIdentityString, $"Attempting load of apparatus. Key: {ApparatusKey}");

            // Get the SRApparatusNode to load from
            log.AddInfo(cLogCategory, NodeIdentityString, $"Sending request to retrieve the apparatus json...");
            ApparatusRequestObject req = ApparatusRequestObject.LoadApparatus(ApparatusKey);
            ApparatusResponseObject res = null;

            try
            {
                res = await SendRequestAsync(req, log);
            }
            catch (Exception e)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"Failed to load apparatus {ApparatusKey} because request failed. {e.GetType()}: {e.Message}");
                
                return;
            }

            if (res.Failed)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"Failed to load apparatus {ApparatusKey} with failure type {res.Status}");
                return;
            }

            SrNode args = res.ResponseData as SrNode;

            if(args == null)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"Failed to load apparatus {ApparatusKey} because response object was not a {nameof(SrNode)}");
                return;
            }

            // Create the objects
            log.AddInfo(cLogCategory, NodeIdentityString, $"Json recieved. Unpacking...");
            
            Deserialize(args, log);

            log.AddInfo(cLogCategory, NodeIdentityString, $"Unpacking success. New node created. Connecting to child");
            Connect(log);
        }

        public override void CompleteDeserialization(MetaDataReader reader, LogWriter log)
        {
            ApparatusKey = reader.Key;
        }

        protected override string[] ResolveMetadata()
        {
            return new string[] { UTMeta.KeyMeta(ApparatusKey) };
        }

        protected void Unload() => DestroyAllNodeChildren();
    }
}

public class MetaDataReader
{
    public string Identifier;
    public string Key;
    public string Type;
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;
    
    public MetaDataReader(string[] metadata)
    {
        Dictionary<string, List<string>> dataUnpacked = new Dictionary<string, List<string>>();

        foreach(string data in metadata)
        {
            string[] dataSplit = data.Split(':');
            
            if (!dataUnpacked.ContainsKey(dataSplit[0]))
            {
                dataUnpacked[dataSplit[0]] = new List<string>();
            }
            dataUnpacked[dataSplit[0]].Add(dataSplit[1]);
        }

        if (dataUnpacked.TryGetValue(UTMeta.cMetaTypeKey, out List<string> key))
            Key = key[0];
    }
}