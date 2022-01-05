using HexCS.Core;

using HexUN.Data;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

using Atomata.VSolar.Apparatus;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using HexUN.Framework.Debugging;

namespace Atomata.VSolar.Apparatus.Example
{
    /// <summary>
    /// This is an example container, that has only been created to show how
    /// a container can load an apparatus from byte[] data that is stored in azure blob storage as assetbundle. 
    /// </summary>
    public abstract class AApparatusContainer : MonoBehaviour, IRequestHandler
    {
        protected abstract string cLogCategory { get; set; }

        /// <summary>
        /// This is the node that is being managed by the container. Null if 
        /// no apparatus has been loaded yet. Serialized only so that it 
        /// can be seen in the editor, should not be populated from editor.
        /// </summary>
        public SerializationNode ManagedNode { get => _managedNode; set => _managedNode = value;}
        [SerializeField] SerializationNode _managedNode = null;

        /// <summary>
        /// The url used to load assetbundles
        /// </summary>
        public string RootURL { get => _rootURL; set => _rootURL = value;}
        [SerializeField] private string _rootURL = "https://addressabletest1.blob.core.windows.net/assetbundles";

        protected IPrefabProvider PrefabProvider;
        protected IApparatusProvider ApparatusProvider;

        protected abstract void Start();
        public abstract void BoolTrigger(string trigger);
        public abstract void VoidTrigger(string trigger);
        public abstract void HandleRequest(ApparatusRequest request, LogWriter log);
        
        /// <summary>
        /// destroys the loaded node
        /// </summary>
        public void DestroyNode()
        {
            if (_managedNode != null) Destroy(_managedNode.gameObject);
            _managedNode = null;
        }

        /// <summary>
        /// Expects an apparatus identifier. Loads the apparatus and makes it a child
        /// of the container.
        /// </summary>
        public async void LoadApparatus(string apparatus)
        {
            LogWriter log = new LogWriter(cLogCategory);
            SetupSerializationNode(apparatus);
            if (_managedNode != null) await _managedNode.Trigger(ApparatusTrigger.LoadTrigger(true), log);
        }

        /// <summary>
        /// Instantiates a serailizedNode then loads that node. 
        /// </summary>
        public void SetupSerializationNode(string identifier)
        {
            DestroyNode();

            // make a serialization node as child
            GameObject serNodeGo = new GameObject($"[SerializationNode] {identifier} Apparatus");
            serNodeGo.transform.SetParent(transform);

            // set the serialization node settings
            SerializationNode serNode = serNodeGo.AddComponent<SerializationNode>();
            serNode.Identifier = identifier;
            serNode.ApparatusKey = identifier;

            // cache reference
            _managedNode = serNode;
        }
    }
}