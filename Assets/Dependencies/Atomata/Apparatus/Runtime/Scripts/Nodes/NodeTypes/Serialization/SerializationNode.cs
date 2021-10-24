using Cysharp.Threading.Tasks;

using HexCS.Core;

using HexUN.Engine.Utilities;
using HexUN.Framework;

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
        private const string cNodeType = nameof(SerializationNode);

        [Header("[Requests]")]
        [SerializeField]
        [Tooltip("Event used by the apparatus to make requests to the environment")]
        private UEApparatusRequest _onRequest;

        public override EApparatusNodeType Type => EApparatusNodeType.Apparatus;

        private EApparatusNodeLoadState _loadState = EApparatusNodeLoadState.Unloaded;
        protected override void SetCustomRequestHandler() =>  RequestHandler = HandleRequest;
        

        private void HandleRequest(ApparatusRequest request)
        {
            // if the serialization node dosen't have an onRequest handler it needs to find one somewhere else. 
            if(_onRequest == null)
            {
                if (IsRoot)
                {
                    LogError(cNodeType, $"Failed to handle request. The root seralization node does not have a request handler. This is not allowed.");
                }
                else
                {
                    LogInfo(cNodeType, $"Relaying request to parent node");
                    Parent.RequestHandler.Invoke(request);
                }

                return;
            }

            LogInfo(cNodeType, $"Received a request and calling registered function: <{_onRequest.GetPersistentMethodName(0)}> on object <{_onRequest.GetPersistentTarget(0)}>");
            _onRequest.Invoke(request);

            // If after the request has been invoked no responder has claimed it, it means there
            // is nothing listening for request. Provide an error
            if (!request.IsClaimed)
            {
                LogWaring(cNodeType, $"Failed to handle request. After relaying request the request was never claimed. Sending missing reference error.");
                request.TryClaim(this);
                request.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse("RequestEventListener"), this);
                return;
            }
        }

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

        protected override async UniTask TriggerNode(ApparatusTrigger trigger)
        {
            switch (trigger.Type)
            {
                case ETriggerType.Load:
                    if(trigger.TryUnpackTrigger_Load(out bool shouldLoad))
                    {
                        if (shouldLoad) await Load();
                        else Unload();
                    }
                    break;
            }
        }

        private async UniTask Load()
        {
            // Get the SRApparatusNode to load from
            ApparatusRequestObject req = ApparatusRequestObject.LoadApparatus(Identifier);
            ApparatusResponseObject res = null;
            
            try
            {
                res = await SendRequestAsync(req);
            }
            catch (Exception e)
            {
                LogError(cNodeType, $"Failed to load apparatus {Identifier} because request failed. {e.GetType()}: {e.Message}");
                return;
            }

            if (res.Failed)
            {
                LogError(cNodeType, $"Failed to load apparatus {Identifier} with failure type {res.Status}");
                return;
            }

            SrApparatus args = res.ResponseData as SrApparatus;

            if(args == null)
            {
                LogError(cNodeType, $"Failed to load apparatus {Identifier} because response object was not a {nameof(SrApparatus)}");
                return;
            }

            // load the apparatus children
            if(args.Identifier != Identifier)
            {
                LogError(cNodeType, $"Failed to load apparatus {Identifier} because provided {nameof(SrApparatus)} object did not contain correct identifier [{Identifier}] as root object");
                return;
            }

            // Create the objects
            for(int i = 0; i<args.Children.Length; i++)
            {
                SrApparatusNode node = args.Children[i];

                GameObject obj = gameObject.AddChild($"[{node.Type}] {node.Identifier}");
                obj.hideFlags = HideFlags.DontSave;
                
                if(args.Transforms != null)
                {
                    args.Transforms[i].ApplyToTransform(obj.transform);                
                }

                // Get the meta that directly relates to this node
                List<string> metas = new List<string>();
                
                string metaPath = $"{Identifier}/{node.Identifier}";
                int metaIndex = Array.IndexOf<string>(args.Metadata.Paths, metaPath);

                if (metaIndex != -1)
                {
                    foreach (string s in args.Metadata.Data)
                    {
                        string[] split = s.Split('@');
                        if (int.Parse(split[0]) == metaIndex) metas.Add(split[1]);
                    }
                }


                switch (node.Type)
                {
                    case EApparatusNodeType.Asset:
                        AssetBundleNode asset = obj.AddComponent<AssetBundleNode>();
                        asset.Deserialize(node, metas.ToArray());
                        break;
                    case EApparatusNodeType.Apparatus:
                        SerializationNode appa = obj.AddComponent<SerializationNode>();
                        appa.Deserialize(node, metas.ToArray());
                        break;
                }
            }

            Connect();
        }

        protected void Unload() => DestroyAllNodeChildren();
    }
}