using HexCS.Core;

using HexUN.Data;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

using Atomata.VSolar.Apparatus;
using UnityEngine.Networking;

namespace Atomata.VSolar.Apparatus.Example
{
    /// <summary>
    /// This is an example container, that has only been created to show quickly how
    /// a container can load and unload an apparatus from byte[] data. The byte data in
    /// this case is stored on the filesystem of the user in a specific place. The container
    /// is useless if the database hasn't been prepared manually.
    /// </summary>
    public class ApparatusContainer_ExampleCloudBundles : MonoBehaviour
    {
        IPrefabProvider PrefabProvider = new LocalAssetBundleProvider("vsolarsystem-proto-storage");

        /// <summary>
        /// This is the node that is being managed by the container. Null if 
        /// no apparatus has been loaded yet. Serialized only so that it 
        /// can be seen in the editor, should not be populated from editor.
        /// </summary>
        [SerializeField]
        private SerializationNode _managedNode = null;

        [SerializeField]
        private string _rootURL = "https://addressabletest1.blob.core.windows.net/assetbundles";


        /// <summary>
        /// Handles triggers, sent as strings with following format 
        /// path/to/node?eventName;(True|False). Only supported boolean
        /// triggers
        /// </summary>
        /// <summary>
        /// Handles triggers, sent as strings with following format 
        /// path/to/node?eventName;(True|False). Only supported boolean
        /// triggers
        /// </summary>
        public async void BoolTrigger(string trigger)
        {
            if (_managedNode != null)
            {
                // unpack the info
                string[] pathAndArgs = trigger.Split('@');
                string[] args = pathAndArgs[1].Split('?');

                // convert the info to a bool trigger object
                await _managedNode.Trigger(
                    ApparatusTrigger.Trigger_Bool(args[0], bool.Parse(args[1]), pathAndArgs[0])
                );

            }
        }


        public async void VoidTrigger(string trigger)
        {
            if (_managedNode != null)
            {
                // unpack the info
                string[] pathAndName = trigger.Split('@');

                //convert the info to a void trigger object
                await _managedNode.Trigger(
                    ApparatusTrigger.DirectEvent_Void(pathAndName[1], pathAndName[0])
                );
            }
        }

        /// <summary>
        /// Simplyt destroys the loaded node
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
            SetupSerializationNode(apparatus);
            if (_managedNode != null) await _managedNode.Trigger(ApparatusTrigger.LoadTrigger(true));
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

            // make sure that request handeling is performed by the container,
            // so that apparatus resources are pulled from the right places
            serNode.RequestHandler = OnRequest;

            // cache reference
            _managedNode = serNode;
        }

        /// <summary>
        /// This function handles requests from the apparatus and resolves them
        /// based on a Desktop platform with relevent data existing in the file system.
        /// </summary>
        public void OnRequest(ApparatusRequest request)
        {
            Debug.Log($"Got a request {request.RequestObject.Type}");

            // claim the request so that you're a legitimate responder
            if (request.TryClaim(this))
            {
                switch (request.RequestObject.Type)
                {
                    case EApparatusRequestType.LoadApparatus:
                        StartCoroutine(OnRequest_LoadApparatus(request));
                        return;
                    case EApparatusRequestType.LoadAsset:
                        StartCoroutine(OnRequest_LoadAsset(request));
                        return;
                }
            }
            else
            {
                Debug.LogError("[ApparatusContainer] Something else claimed a request. This should never happen for the container");
            }
        }

        /// <summary>
        /// Handles loading an apparatus
        /// </summary>
        private IEnumerator OnRequest_LoadApparatus(ApparatusRequest request)
        {

            string identifier = (request.RequestObject.Args as ApparatusLoadRequestArgs).Identifier;
            string url = _rootURL + "/" + identifier+".json";
            UnityWebRequest req = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation op = req.SendWebRequest();

            while (!op.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            SrApparatus sappa = JsonUtility.FromJson<SrApparatus >(req.downloadHandler.text);
            
            // respond to the request
            request.Respond(
                ApparatusResponseObject.SerializeNodeResponse(sappa),
                this
            );
        }




        /// <summary>
        /// Loads and assetbundle from the filesystem based on request args
        /// </summary>
        private IEnumerator OnRequest_LoadAsset(ApparatusRequest request)
        {

            string name = (request.RequestObject.Args as AssetLoadRequestArgs).Name;
            string url = _rootURL + "/" + name + "_webgl";
            UnityWebRequest req = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation op = req.SendWebRequest();

            while (!op.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            byte[] data = req.downloadHandler.data;
            AssetBundle ab = AssetBundle.LoadFromMemory(data);
            GameObject obj = ab.LoadAsset<GameObject>(name);
            ab.Unload(false);


            // respond to the request
            request.Respond(
                ApparatusResponseObject.AssetResponse(obj),
                this
            );

        }
    }
}