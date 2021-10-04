using HexCS.Core;

using HexUN.Data;

using System.Runtime.InteropServices;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

namespace Atomata.VSolar.Apparatus.WebGL
{
    /// <summary>
    /// This is an example container, that has only been created to show quickly how
    /// a container can load and unload an apparatus from byte[] data. The byte data in
    /// this case is stored on the filesystem of the user in a specific place. The container
    /// is useless if the database hasn't been prepared manually.
    /// </summary>
    public class ApparatusContainer_WebGL : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void LogToBrowser(string str);
        [DllImport("__Internal")]
        private static extern void AssetBundleRequestToBrowser();

        /// <summary>
        /// This is the node that is being managed by the container. Null if 
        /// no apparatus has been loaded yet. Serialized only so that it 
        /// can be seen in the editor, should not be populated from editor.
        /// </summary>
        [SerializeField]
        private SerializationNode _managedNode = null;


        private string requestApparatusJSON = "";

        /// <summary>
        /// Handles triggers, sent as strings with following format 
        /// path/to/node?eventName;(True|False). Only supported boolean
        /// triggers
        /// </summary>
        public async void ButtonClickTrigger(string trigger)
        {
            if (_managedNode != null)
            {
                // unpack the info
                string[] pathAndArgs = trigger.Split('?');
                string[] args = pathAndArgs[1].Split(';');

                // convert the info to a bool trigger object
                await _managedNode.Trigger(
                    ApparatusTrigger.Trigger_Bool(args[0], bool.Parse(args[1]), pathAndArgs[0])
                );
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
            Load(apparatus);
            if (_managedNode != null) await _managedNode.Trigger(ApparatusTrigger.LoadTrigger(true));
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
                        OnRequest_LoadApparatus(request);
                        return;
                    case EApparatusRequestType.LoadAsset:
                        OnRequest_LoadAsset(request);
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
        private void OnRequest_LoadApparatus(ApparatusRequest request)
        {
            // Make the correct path to the target folder
            const string cDatabaseName = "vsolarsystem-proto-storage";
            UnityPath databasePath = UnityPath.PersistentDataPath.Path
                .InsertAtEnd("Database")
                .InsertAtEnd(cDatabaseName)
                .InsertAtEnd("Apparatus");

            AssetBundle.UnloadAllAssetBundles(true);

            if (databasePath.Path.TryAsDirectoryInfo(out DirectoryInfo di))
            {
                // find the file based on the request args
                FileInfo[] files = di.GetFiles();
                ApparatusLoadRequestArgs args = request.RequestObject.Args as ApparatusLoadRequestArgs;
                FileInfo file = files.FirstOrDefault(
                    f =>
                    {
                        PathString ps = f.FullName;
                        return ps.EndWithoutExtension == args.Identifier;
                    }
                );

                // read the json
                string json = null;
                using (FileStream fs = file.OpenRead())
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        json = sr.ReadToEnd();
                    }
                }

                // deserialize the object
                SrApparatus sappa = JsonUtility.FromJson<SrApparatus>(json);

                // respond to the request
                request.Respond(
                    ApparatusResponseObject.SerializeNodeResponse(sappa),
                    this
                );
            }
        }

        /// <summary>
        /// Loads and assetbundle from the filesystem based on request args
        /// </summary>
        private void OnRequest_LoadAsset(ApparatusRequest request)
        {
            // find the file based on the request args
            const string cDatabaseName = "vsolarsystem-proto-storage";
            UnityPath databasePath = UnityPath.PersistentDataPath.Path
                .InsertAtEnd("Database")
                .InsertAtEnd(cDatabaseName)
                .InsertAtEnd("assetbundles");

            if (databasePath.Path.TryAsDirectoryInfo(out DirectoryInfo di))
            {
                // find the file based on the request args
                FileInfo[] files = di.GetFiles();
                AssetLoadRequestArgs args = request.RequestObject.Args as AssetLoadRequestArgs;
                FileInfo file = files.FirstOrDefault(
                    f =>
                    {
                        PathString ps = f.FullName;
                        return ps.End == args.Name;
                    }
                );

                // Load an assetbundle from bytes
                byte[] bytes = null;
                using (FileStream fs = file.OpenRead())
                {
                    bytes = fs.ReadAllBytes();
                }

                AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
                Object[] objects = assetBundle.LoadAllAssets();
                GameObject go = objects[0] as GameObject;

                // respond to the request
                request.Respond(
                    ApparatusResponseObject.AssetResponse(go),
                    this
                );
            }
        }


        /////////

        /// <summary>
        /// Expects an apparatus identifier. Loads the apparatus and makes it a child
        /// of the container.
        /// </summary>
        public async void ButtonClickLoad_WebGL(string apparatus)
        {
            LoadFromJSON(apparatus);
            if (_managedNode != null) await _managedNode.Trigger(ApparatusTrigger.LoadTrigger(true));
        }

        ///

        public void LoadFromJSON(string json)
        {
            LogToBrowser("1");
            requestApparatusJSON = json;

            string identifier = "earth";

            if (_managedNode != null) Destroy(_managedNode);

            LogToBrowser("2");

            // make a serialization node as child
            GameObject serNodeGo = new GameObject($"[SerializationNode] {identifier} Apparatus");

            LogToBrowser(transform.ToString());
            serNodeGo.transform.SetParent(transform);

            // set the serialization node settings
            SerializationNode serNode = serNodeGo.AddComponent<SerializationNode>();
            serNode.Identifier = identifier;

            // make sure that request handeling is performed by the container,
            // so that apparatus resources are pulled from the right places
            serNode.RequestHandler = OnRequest_WebGL;

            // cache reference
            _managedNode = serNode;
        }

        public void OnRequest_WebGL(ApparatusRequest request)
        {
            Debug.Log($"Got a request {request.RequestObject.Type}");
            LogToBrowser("3");

            // claim the request so that you're a legitimate responder
            if (request.TryClaim(this))
            {
                switch (request.RequestObject.Type)
                {
                    case EApparatusRequestType.LoadApparatus:
                        OnRequest_LoadApparatus_JSON(request);
                        return;
                    case EApparatusRequestType.LoadAsset:
                        OnRequest_LoadAsset(request);
                        return;
                }
            }
            else
            {
                Debug.LogError("[ApparatusContainer] Something else claimed a request. This should never happen for the container");
            }
        }

        ///
        private void OnRequest_LoadApparatus_JSON(ApparatusRequest request)
        {
            // deserialize the object
            SrApparatus sappa = JsonUtility.FromJson<SrApparatus>(requestApparatusJSON);

            LogToBrowser("4");

            // respond to the request
            request.Respond(
                ApparatusResponseObject.SerializeNodeResponse(sappa),
                this
            );
        }
    }
}