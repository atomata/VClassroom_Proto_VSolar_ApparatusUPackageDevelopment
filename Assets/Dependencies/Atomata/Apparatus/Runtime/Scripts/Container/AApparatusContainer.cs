using System.Text;
using UnityEngine;
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

        public abstract void HandleRequest(ApparatusRequest request, LogWriter log);
        
        /// <summary>
        /// destroys the loaded apparatus by calling Destory() on the node
        /// </summary>
        public void UnloadApparatus()
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
            _managedNode.TriggerDefaultCamera();
            log.PrintToConsole(cLogCategory);
        }

        /// <summary>
        /// Instantiates a serailizedNode then loads that node. 
        /// </summary>
        public void SetupSerializationNode(string identifier)
        {
            UnloadApparatus();

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

        public void Debug_PrintTree()
        {
            StringBuilder sb = new StringBuilder();
            _managedNode.PrintTreeToStringBuilder(sb, "");
            Debug.Log(sb.ToString());
        }
    }
}