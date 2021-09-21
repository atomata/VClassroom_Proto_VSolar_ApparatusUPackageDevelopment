using HexCS.Core;

using HexUN.Data;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public class ApparatusContainer_DesktopLocal : MonoBehaviour
    {
        [SerializeField]
        private SerializationNode _managedNode = null;

        public async void ButtonClickTrigger(string trigger)
        {
            if(_managedNode != null)
            {
                string[] pathAndArgs = trigger.Split('?');
                string[] args = pathAndArgs[1].Split(';');

                await _managedNode.Trigger(
                    ApparatusTrigger.Trigger_Bool(args[0], bool.Parse(args[1]), pathAndArgs[0])
                );
            }
        }

        public void ButtonClickDestroy()
        {
            if (_managedNode != null) Destroy(_managedNode.gameObject);
            _managedNode = null;
        }

        public async void ButtonClickLoad(string apparatus)
        {
            Load(apparatus);
            if (_managedNode != null) await _managedNode.Trigger(ApparatusTrigger.LoadTrigger(true));
        }

        public void OnRequest(ApparatusRequest request)
        {
            Debug.Log($"Got a request {request.RequestObject.Type}");

            if (request.TryClaim(this, "MYID"))
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

        private void OnRequest_LoadApparatus(ApparatusRequest request)
        {
            const string cDatabaseName = "vsolarsystem-proto-storage";
            UnityPath databasePath = UnityPath.PersistentDataPath.Path
                .InsertAtEnd("Database")
                .InsertAtEnd(cDatabaseName)
                .InsertAtEnd("Apparatus");

            if(databasePath.Path.TryAsDirectoryInfo(out DirectoryInfo di))
            {
                FileInfo[] files = di.GetFiles();
                ApparatusLoadRequestArgs args = request.RequestObject.Args as ApparatusLoadRequestArgs;
                FileInfo file = files.FirstOrDefault(
                    f =>
                    {
                        PathString ps = f.FullName;
                        return ps.EndWithoutExtension == args.Identifier;
                    }
                );

                string json = null;
                using (FileStream fs = file.OpenRead())
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        json = sr.ReadToEnd();
                    }
                }

                SrApparatus sappa =  JsonUtility.FromJson<SrApparatus>(json);

                request.Respond(
                    ApparatusResponseObject.SerializeNodeResponse(sappa),
                    this
                );
            }
        }

        private void OnRequest_LoadAsset(ApparatusRequest request)
        {
            const string cDatabaseName = "vsolarsystem-proto-storage";
            UnityPath databasePath = UnityPath.PersistentDataPath.Path
                .InsertAtEnd("Database")
                .InsertAtEnd(cDatabaseName)
                .InsertAtEnd("assetbundles");

            if (databasePath.Path.TryAsDirectoryInfo(out DirectoryInfo di))
            {
                FileInfo[] files = di.GetFiles();
                AssetLoadRequestArgs args = request.RequestObject.Args as AssetLoadRequestArgs;
                FileInfo file = files.FirstOrDefault(
                    f =>
                    {
                        PathString ps = f.FullName;
                        return ps.End == args.Name;
                    }
                );

                // Load an assetbundle
                byte[] bytes = null;
                using (FileStream fs = file.OpenRead())
                {
                    bytes = fs.ReadAllBytes();
                }

                AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
                Object[] objects = assetBundle.LoadAllAssets();
                GameObject go = objects[0] as GameObject;

                request.Respond(
                    ApparatusResponseObject.AssetResponse(go),
                    this
                );
            }
        }

        public void Load(string identifier)
        {
            if (_managedNode != null) Destroy(_managedNode);

            GameObject serNodeGo = new GameObject($"[SerializationNode] {identifier} Apparatus");
            serNodeGo.transform.SetParent(transform);
            
            SerializationNode serNode = serNodeGo.AddComponent<SerializationNode>();
            serNode.Identifier = identifier;
            serNode.RequestHandler = OnRequest;

            _managedNode = serNode;
        }
    }
}