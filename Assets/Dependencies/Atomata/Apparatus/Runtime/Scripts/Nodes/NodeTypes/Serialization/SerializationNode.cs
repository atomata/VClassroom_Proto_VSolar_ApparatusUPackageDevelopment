using Cysharp.Threading.Tasks;

using HexCS.Core;

using HexUN.Engine.Utilities;
using HexUN.Framework;
using HexUN.Framework.Debugging;

using System;
using System.Collections.Generic;
using System.Linq;

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

            SrApparatus args = res.ResponseData as SrApparatus;

            if(args == null)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"Failed to load apparatus {ApparatusKey} because response object was not a {nameof(SrApparatus)}");
                return;
            }

            // load the apparatus children
            if(args.ApparatusKey != ApparatusKey)
            {
                log.AddError(cLogCategory, NodeIdentityString, $"Failed to load apparatus {ApparatusKey} because provided {nameof(SrApparatus)} object did not contain correct identifer [{Identifier}] as root object");
                return;
            }

            // Create the objects
            log.AddInfo(cLogCategory, NodeIdentityString, $"Json recieved. Unpacking...");


            for(int i = 0; i < args.Metadata.Paths.Length; i++)
            {
                string[] split = args.Metadata.Paths[i].Split('/');

                // When the path length is 2, it's a direct child
                if(split.Length == 2)
                {
                    List<string> metaData = new List<string>();

                    foreach(string data in args.Metadata.Data)
                    {
                        string[] dataSplit = data.Split('@');
                        if (dataSplit[0] == i.ToString())
                            metaData.Add(dataSplit[1]);
                    }

                    // based on metadata deserialize the objects
                    Dictionary<string, string> dataUnpacked = new Dictionary<string, string>();

                    foreach(string data in metaData)
                    {
                        string[] dataSplit = data.Split(':');
                        dataUnpacked.Add(dataSplit[0], dataSplit[1]);
                    }

                    GameObject obj = gameObject.AddChild($"[{dataUnpacked[UTMeta.cMetaTypeType]}] {dataUnpacked[UTMeta.cMetaTypeIdentifer]}");
                    obj.hideFlags = HideFlags.DontSave;

                    // Now spawn stuff based on the data
                    switch (dataUnpacked[UTMeta.cMetaTypeType])
                    {
                        case "AssetBundle":
                            AssetBundleNode asset = obj.AddComponent<AssetBundleNode>();
                            asset.Identifier = dataUnpacked[UTMeta.cMetaTypeIdentifer];
                            asset.AssetBundleKey = dataUnpacked[UTMeta.cMetaTypeKey];
                            if(dataUnpacked.TryGetValue(UTMeta.cMetaTypeTransform, out string valueA))
                            {
                                float[] transform = valueA.Split(',').Select(s => float.Parse(s)).ToArray();
                                asset.transform.position = new Vector3(transform[0], transform[1], transform[2]);
                                asset.transform.rotation = new Quaternion(transform[3], transform[4], transform[5], transform[6]);
                                asset.transform.localScale = new Vector3(transform[7], transform[8], transform[9]);
                            }
                            break;
                        case "Serailization":
                            SerializationNode ser = obj.AddComponent<SerializationNode>();
                            ser.Identifier = dataUnpacked[UTMeta.cMetaTypeIdentifer];
                            ser.ApparatusKey = dataUnpacked[UTMeta.cMetaTypeKey];
                            if (dataUnpacked.TryGetValue(UTMeta.cMetaTypeTransform, out string valueS))
                            {
                                float[] transform = valueS.Split(',').Select(s => float.Parse(s)).ToArray();
                                ser.transform.position = new Vector3(transform[0], transform[1], transform[2]);
                                ser.transform.rotation = new Quaternion(transform[3], transform[4], transform[5], transform[6]);
                                ser.transform.localScale = new Vector3(transform[7], transform[8], transform[9]);
                            }
                            break;
                        case "Event":
                            log.AddError(cLogCategory, NodeIdentityString, $"Currently, event nodes must be children of asset bundle node. Cannot load.");
                            break;
                        case "DeltaTransform":
                            log.AddError(cLogCategory, NodeIdentityString, $"Currently, delta transform nodes must be children of asset bundle node. Cannot load.");
                            break;

                    }
                }
            }

            log.AddInfo(cLogCategory, NodeIdentityString, $"Unpacking success. New node created. Connecting to child");
            Connect(log);
        }

        protected override string[] ResolveMetadata()
        {
            string[] baseMeta = base.ResolveMetadata();
            return UTArray.Combine(baseMeta, new string[] { UTMeta.KeyMeta(ApparatusKey) });
        }

        protected void Unload() => DestroyAllNodeChildren();
    }
}