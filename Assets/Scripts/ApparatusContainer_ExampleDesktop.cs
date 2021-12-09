using HexCS.Core;

using HexUN.Data;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using HexUN.Framework.Debugging;
using HexUN.Framework;

using Atomata.VSolar.Apparatus;

namespace Atomata.VSolar.Apparatus.Example
{
    /// <summary>
    /// This is an example container, that has only been created to show quickly how
    /// a container can load and unload an apparatus from byte[] data. The byte data in
    /// this case is stored on the filesystem of the user in a specific place. The container
    /// is useless if the database hasn't been prepared manually.
    /// </summary>
    public class ApparatusContainer_ExampleDesktop : MonoBehaviour, IRequestHandler
    {
        private const string cLogCategory = nameof(ApparatusContainer_ExampleDesktop);

        IPrefabProvider PrefabProvider = new LocalAssetBundleProvider("vsolarsystem-proto-storage");
        IApparatusProvider ApparatusProvider = new LocalApparatusProvider("vsolarsystem-proto-storage");

        /// <summary>
        /// This is the node that is being managed by the container. Null if 
        /// no apparatus has been loaded yet. Serialized only so that it 
        /// can be seen in the editor, should not be populated from editor.
        /// </summary>
        [SerializeField]
        private SerializationNode _managedNode = null;

        private Dictionary<int, GameObject> _cachedBundles = new Dictionary<int, GameObject>();

        /// <summary>
        /// Handles triggers, sent as strings with following format 
        /// path/to/node?eventName;(True|False). Only supported boolean
        /// triggers
        /// </summary>
        public async void ButtonClickTrigger(string trigger)
        {
            if(_managedNode != null)
            {
                LogWriter log = new LogWriter(cLogCategory);

                // unpack the info
                string[] pathAndArgs = trigger.Split('?');
                string[] args = pathAndArgs[1].Split(';');

                // convert the info to a bool trigger object
                await _managedNode.Trigger(
                    ApparatusTrigger.Trigger_Bool(args[0], bool.Parse(args[1]), pathAndArgs[0]), log
                );

                OneHexServices.Instance.Log.Info(cLogCategory, log.GetLog());
            }
        }

        /// <summary>
        /// Simplyt destroys the loaded node
        /// </summary>
        public void ButtonClickDestroy()
        {
            if (_managedNode != null) Destroy(_managedNode.gameObject);
            _managedNode = null;
        }

        /// <summary>
        /// Expects an apparatus identifier. Loads the apparatus and makes it a child
        /// of the container.
        /// </summary>
        public async void ButtonClickLoad(string apparatus)
        {
            LogWriter log = new LogWriter(cLogCategory);

            Load(apparatus);
            if (_managedNode != null) await _managedNode.Trigger(ApparatusTrigger.LoadTrigger(true), log);

            log.PrintToConsole(cLogCategory);
        }

        /// <summary>
        /// Instantiates a serailizedNode then loads that node. 
        /// </summary>
        public void Load(string identifier)
        {
            if (_managedNode != null) Destroy(_managedNode);

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

        /// <summary>
        /// This function handles requests from the apparatus and resolves them
        /// based on a Desktop platform with relevent data existing in the file system.
        /// </summary>
        public void HandleRequest(ApparatusRequest request, LogWriter log)
        {
            log.AddInfo(cLogCategory, cLogCategory, $"Received request {request.RequestObject.Type}");
            UTApparatusRequest.HandleRequest(PrefabProvider, ApparatusProvider, request, this, cLogCategory, log);
        }
    }
}