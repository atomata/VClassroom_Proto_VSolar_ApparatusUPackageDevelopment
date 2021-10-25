using Atomata.VSolar.Utilities;

using Cysharp.Threading.Tasks;

using HexCS.Core;

using HexUN.Behaviour;
using HexUN.Engine.Utilities;
using HexUN.Framework;
using HexUN.Framework.Debugging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Parent class for all apparatus nodes
    /// </summary>
    [ExecuteAlways]
    public abstract partial class AApparatusNode : HexBehaviour
    {
        private string cLogCategory = nameof(AApparatusNode);
        private string cNodeLogCategory = "NodeLog";

        [Header("[AApparatusNode]")]
        [SerializeField]
        [Tooltip("Identifier used to target node with commands, etc. Must be unique from siblings.")]
        private string _identifier = "id";

        private EApparatusNodeConnectionState _connectionState = EApparatusNodeConnectionState.Disconnected;
        private AApparatusNode _parent;
        private List<AApparatusNode> _children = new List<AApparatusNode>();

        private Action<ApparatusRequest, LogWriter> _requestHandler = null;

        public abstract string NodeType { get; }

        /// <summary>
        /// Is this node currently connected to it's parent and children. If unconnected, 
        /// none of the nodes functions will work reliably
        /// </summary>
        public EApparatusNodeConnectionState ConnectionState => _connectionState;

        /// <summary>
        /// The node type
        /// </summary>
        public abstract EApparatusNodeType Type { get; }

        /// <summary>
        /// The identifier of the node
        /// </summary>
        public string Identifier { get => _identifier; set => _identifier = value; }

        /// <summary>
        /// Children of the apparatus node
        /// </summary>
        public AApparatusNode[] Children => _children.ToArray();

        /// <summary>
        /// Is this the root node in the apparatus tree
        /// </summary>
        public bool IsRoot => _parent == null;

        /// <summary>
        /// The parent node
        /// </summary>
        public AApparatusNode Parent => _parent;

        /// <summary>
        /// Get the root node in the apparatus tree
        /// </summary>
        public AApparatusNode Root
        {
            get 
            {
                if (IsRoot) return this;

                AApparatusNode candidate = _parent;
                while (!candidate.IsRoot) candidate = candidate._parent;
                return candidate;
            }
        }

        /// <summary>
        /// The path from the relative node to this node as a <see cref="PathString"/>. If relativeTo
        /// is set to null, then searchs until root
        /// </summary>
        public PathString Path(string relativeTo = null) => CalculatePath(relativeTo);

        /// <summary>
        /// Used to catch a single request. Can be used by editor scripts to redirect 
        /// requests to editor config based resolution. Can be set to override the request 
        /// handler implemented by the node with a custom request handler
        /// </summary>
        public Action<ApparatusRequest, LogWriter> RequestHandler
        {
            get
            {
                if (IsRoot)
                {
                    IRequestHandler handler = transform.parent.GetComponent<IRequestHandler>();
                    _requestHandler = handler == null ? NullHandler : new Action<ApparatusRequest, LogWriter>(handler.HandleRequest);
                }
                else
                {
                    _requestHandler = Parent.RequestHandler ?? NullHandler;
                }

                return _requestHandler;
            }
            set => _requestHandler = value;
        }

        /* CONNECTION SYSTEM
         * How does the node move from the Inactive state to the active state and
         * vice versa. Nodes always start un connected and need to be told when to
         * try to connect. The action of connecting connects the node to it's children
         */
        #region Connections
        /// <summary>
        /// Connect the node to it's children
        /// </summary>
        public void Connect()
        {
            TraverseHierachyAndSetChildren();
            ConnectChildren();
            _connectionState = EApparatusNodeConnectionState.Connected;
            OnConnected();
        }

        /// <summary>
        /// Disconnected the node from it's children and parent
        /// </summary>
        public void Disconnect()
        {
            DisconnectChildren();
            _parent = null;
            _children.Clear();
            _connectionState = EApparatusNodeConnectionState.Disconnected;
            OnDisconnected();
        }

        /// <summary>
        /// Called when the node is connected
        /// </summary>
        protected virtual void OnConnected() { }

        /// <summary>
        /// Called when the node is disconnected
        /// </summary>
        protected virtual void OnDisconnected() { }

        private void ConnectChildren()
        {
            foreach (AApparatusNode node in _children) node.Connect();
        }

        private void DisconnectChildren()
        {
            if(_children != null)
            {
                foreach (AApparatusNode node in _children)
                {
                    if(node != null) node.Disconnect();
                }
            }
        }

        private void CalculateParentFromNode()
        {
            Transform target = transform.parent;

            while (target != null)
            {
                AApparatusNode parent = target.GetComponent<AApparatusNode>();

                if (parent != null)
                {
                    _parent = parent;
                    _parent.CalculateParentFromNode();
                    break;
                }

                target = target.parent;
            }
        }

        private PathString CalculatePath(string relativeTo = null, PathString last = null)
        {
            if ( (relativeTo==null && IsRoot) || (Identifier == relativeTo) )
            {
                if (last == null) return new PathString(Identifier);
                return last.InsertAtStart(Identifier);
            }

            return Parent.CalculatePath(relativeTo, last == null ? new PathString(Identifier) : last.InsertAtStart(Identifier));
        }

        private void TraverseHierachyAndSetChildren()
        {
            // deregister from all current children and clear list
            foreach(AApparatusNode node in _children)
            {
                if (node != null) node.DeregisterParent(this);
            }

            _children.Clear();

            // reperform registration
            UTGameObject.DoToHierarchy(gameObject, IndicateContinueOrSetNodeParentAndActivate);

            bool IndicateContinueOrSetNodeParentAndActivate(GameObject target)
            {
                AApparatusNode node = target.GetComponent<AApparatusNode>();

                if (node == null) return true;

                node.RegisterParent(this);
                _children.Add(node);
                return false;
            }
        }

        private void RegisterParent(AApparatusNode parent) => _parent = parent;

        private void DeregisterParent(AApparatusNode parent)
        {
            if (_parent == parent) _parent = null;
        }
        #endregion

        /*
         * TRIGGER SYSTEM
         * This allows the environment to send commands to nodes in the apparatus.
         * Triggers follow a URI syntax
         */
        #region Triggers
        /// <summary>
        /// Start relaying a trigger from this node to it's children
        /// </summary>
        public async UniTask Trigger(ApparatusTrigger trigger)
        {
            await RelayTrigger(new ApparatusTriggerCarriage(trigger));
        }

        /// <summary>
        /// The node recieves an apparatus trigger and handles it accordingly
        /// </summary>
        protected abstract UniTask TriggerNode(ApparatusTrigger trigger);

        private async UniTask RelayTrigger(ApparatusTriggerCarriage trigger)
        {
            if (trigger.IsTarget(_identifier))
            {
                await TriggerNode(trigger.Trigger);

                if (trigger.IsGlobal)
                    foreach (AApparatusNode child in _children) await child.RelayTrigger(trigger);
            }
            else
            {
                if (trigger.MoveToNext())
                {
                    foreach (AApparatusNode child in _children)
                    {
                        if(child.Identifier == trigger.Next)
                        {
                            await child.RelayTrigger(trigger);
                        }
                    }
                }
            }
        }
        #endregion

        /*
         * SERAILIZATION SYSTEM
         * How is the node serialized/deserialized
         */
        #region Serialization
        /// <summary>
        /// Returns a serializable id version of the node so that it can be identified
        /// </summary>
        public SrApparatusNode SerializableId() =>  new SrApparatusNode(Type, Identifier);

        /// <summary>
        /// Deserailize the basic load info based on id
        /// </summary>
        public virtual void Deserialize(SrApparatusNode node, string[] metas) => Identifier = node.Identifier;
        #endregion

        /*
         * METADATA SYSTEM
         * How does a node describe itself to the outside world. Format is
         * metadata_type:args
         */
        #region Metadata
        /// <summary>
        /// Gets all meta data from the node
        /// </summary>
        public SrApparatusMetadata GetMetadata()
        {
            Queue<AApparatusNode> nodes = new Queue<AApparatusNode>();
            nodes.Enqueue(this);

            List<string> metaPaths = new List<string>();
            List<string> metaData = new List<string>();

            int index = 0;

            while (nodes.Count > 0)
            {
                AApparatusNode processNode = nodes.Dequeue();
                string[] metas = processNode.ResolveMetadata();

                metaPaths.Add(processNode.Path().ToString('/'));
                foreach (string meta in metas) metaData.Add($"{index}@{meta}");
                index++;

                foreach (AApparatusNode child in processNode.Children) nodes.Enqueue(child);
            }

            return new SrApparatusMetadata
            {
                Paths = metaPaths.ToArray(),
                Data = metaData.ToArray()
            };
        }

        /// <summary>
        /// Return all metadata pertaining to the node
        /// </summary>
        protected virtual string[] ResolveMetadata()
        {
            return new string[]
            {
                UTMeta.IdentifierMeta(Identifier)
            };
        }

        #endregion
        /*
         * REQUEST SYSTEM
         * The request system uses ApparatusRequestObjects to request things from the environment.
         * The environment is expected to respond through the request object.
         */
        #region Requests
        /// <summary>
        /// Sends a request by pushing it to the request pipeline. Returns the constructed
        /// request so that is can be awaited
        /// </summary>
        protected ApparatusRequest SendRequest(ApparatusRequestObject request, LogWriter writer)
        {
            ApparatusRequest req = new ApparatusRequest(request);
            RequestHandler(req, writer);
            return req;
        }

        /// <summary>
        /// Sends a request by pushing it to the request pipeline. automatically awaits the
        /// request and provides a response
        /// </summary>
        protected async Task<ApparatusResponseObject> SendRequestAsync(ApparatusRequestObject request, LogWriter writer)
        {
            ApparatusRequest req = new ApparatusRequest(request);

            RequestHandler(req, writer);
            return await req.AwaitAsync();
        }

        /// <summary>
        /// Handles a request received by the request pipeline
        /// </summary>
        private void NullHandler(ApparatusRequest request, LogWriter log)
        {
            log.AddError(GetNodeLog($"Node request has reached the root and is not being handled"));
        }
        #endregion

        /// <summary>
        /// Destroys all children that are not AApparatus nodes
        /// </summary>
        protected void DestroyAllNonNodeChildren()
        {
            if (this == null) return; // catch after destroy access

            gameObject.DestroyAllChildren_EditorSafe(IsNodePredicate);

            bool IsNodePredicate(GameObject ob)
            {
                if (ob == null) return false;

                AApparatusNode node = ob.GetComponent<AApparatusNode>();
                return node != null;
            }
        }

        /// <summary>
        /// Destroys all children that are <see cref=" AApparatusNode"/>
        /// </summary>
        protected void DestroyAllNodeChildren()
        {
            foreach(AApparatusNode node in _children)
            {
                if(node != null)
                {
                    UTGameObject.Destroy_EditorSafe(node.gameObject);
                }
            }

            _children.Clear();
        }

#if UNITY_EDITOR
        /// <summary>
        /// <para>Used when populating controls available to the node and it's children. In the unity editor,
        /// when a node is clicked, if the drop down for that nodes controls is active the node is
        /// given the ability to populate it's own controls.</para>
        /// 
        /// <para>If False is returned, then no controls should be rendered and the render action will be null.
        /// Otherwise, the render action can be used to render editor controls</para>
        /// </summary>
        public virtual bool IfUnityEditor_PopulateControls(AApparatusNode rendering, out Action render)
        {
            render = null;
            return false;
        }
#endif

        protected void LogInfo(string message) 
            => OneHexServices.Instance.Log.Info(cNodeLogCategory, GetNodeLog(message));

        protected void LogWaring(string message)
            => OneHexServices.Instance.Log.Warn(cNodeLogCategory, GetNodeLog(message));

        protected void LogError(string message)
            => OneHexServices.Instance.Log.Error(cNodeLogCategory, GetNodeLog(message));
        protected void LogError(string message, Exception exception)
            => OneHexServices.Instance.Log.Error(cNodeLogCategory, GetNodeLog(message), exception);

        protected void LogWriter(string title, LogWriter writer)
        {
            OneHexServices.Instance.Log.Info("LOG", writer.GetLog());
        }

        protected string GetNodeLog(string message) => $"{message} \n\tId: {Identifier} \n\tPath: {Path()}\n\tType{NodeType}";
    }
}