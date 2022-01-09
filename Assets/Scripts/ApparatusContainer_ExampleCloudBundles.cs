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
    public class ApparatusContainer_ExampleCloudBundles : MonoBehaviour, IRequestHandler
    {

        //[DllImport("__Internal")]
        //private static extern void LogToBrowser(string str);

        /// <summary>
        /// This is the node that is being managed by the container. Null if 
        /// no apparatus has been loaded yet. Serialized only so that it 
        /// can be seen in the editor, should not be populated from editor.
        /// </summary>
        [SerializeField]
        private SerializationNode _managedNode = null;

        [SerializeField]
        private string _rootURL = "https://addressabletest1.blob.core.windows.net/assetbundles";

        private const string cLogCategory = nameof(ApparatusContainer_ExampleCloudBundles);

        IGameObjectProvider _gameObjectProvider;
        IApparatusProvider ApparatusProvider;


        private void Start()
        {
            _gameObjectProvider = new CloudAssetProvider(_rootURL);
            ApparatusProvider = new CloudApparatusProvider(_rootURL);
        }
        /// <summary>
        /// Handles boolean triggers, sent as strings with following format 
        /// path/to/node@eventName?(True|False). 
        /// </summary>
        public async void BoolTrigger(string trigger)
        {
            if (_managedNode != null)
            {
                LogWriter log = new LogWriter(cLogCategory);
                // unpack the info
                string[] pathAndArgs = trigger.Split('@');
                string[] args = pathAndArgs[1].Split('?');

                // convert the info to a bool trigger object
                await _managedNode.Trigger(
                    ApparatusTrigger.Trigger_Bool(args[0], bool.Parse(args[1]), pathAndArgs[0]), log
                );

            }
            //LogToBrowser("bool trigger successfully handled");
        }

        /// <summary>
        /// Handles void triggers, sent as strings with following format 
        /// path/to/node@eventName. 
        /// </summary>
        public async void VoidTrigger(string trigger)
        {
            if (_managedNode != null)
            {
                LogWriter log = new LogWriter(cLogCategory);
                // unpack the info
                string[] pathAndName = trigger.Split('@');

                //convert the info to a void trigger object
                await _managedNode.Trigger(
                    ApparatusTrigger.DirectEvent_Void(pathAndName[1], pathAndName[0]), log
                );
            }
            //LogToBrowser("void trigger successfully handled");
        }

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
            //LogToBrowser("load trigger successfully handled");
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
            UTApparatusRequest.HandleRequest(_gameObjectProvider, ApparatusProvider, request, this, cLogCategory, log);
        }
    }
}