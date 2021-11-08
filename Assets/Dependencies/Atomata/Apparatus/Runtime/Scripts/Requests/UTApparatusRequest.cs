using HexUN.Framework.Debugging;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public static class UTApparatusRequest
    {
        public static async void HandleRequest(IPrefabProvider prefabProvider, IApparatusProvider apparatusProvider, ApparatusRequest req, object callObject, string callerName, LogWriter log)
        {
            switch (req.RequestObject.Type)
            {
                case EApparatusRequestType.LoadAsset:
                    log.AddInfo(callerName, callerName, $"{req.GetIDString()} Routing to {nameof(UTApparatusRequest.AssetLoad)}");
                    AssetLoad(prefabProvider, req, callObject, callerName, log);
                    break;
                case EApparatusRequestType.LoadApparatus:
                    log.AddInfo(callerName, callerName, $"{req.GetIDString()} Routing to {nameof(UTApparatusRequest.ApparatusLoadAndDeserialize)}");
                    ApparatusLoadAndDeserialize(apparatusProvider, req, callObject, callerName, log);
                    break;
                default:
                    log.AddWarning(callerName, callerName, $"{req.GetIDString()} Could not find route for request of type: {req.RequestObject.Type}");
                    break;
            }
        }

        /// <summary>
        /// Generic method for loading assets using apparatus requests. 
        /// </summary>
        /// <param name="provider">Implementation of asset loading</param>
        /// <param name="req">The origin request of the load</param>
        /// <param name="callObject">The object calling</param>
        /// <param name="callerName">The object name to be used in logs</param>
        /// <param name="log">The log write to log to</param>
        public static async void AssetLoad(IPrefabProvider provider, ApparatusRequest req, object callObject, string callerName, LogWriter log)
        {
            log.AddInfo(callerName, callerName, $"{req.GetIDString()} Starting asset load");
            if (!req.TryClaim(callObject))
            {
                log.AddWarning(callerName, callerName, $"{req.GetIDString()} AssetLoad request already claimed. Aborting");
                return;
            }

            AssetLoadRequestArgs args = req.RequestObject.Args as AssetLoadRequestArgs;

            if (args == null)
            {
                log.AddError(callerName, callerName, "{req.GetIDString()} Failed to get AssetLoadRequestArgs from AssetLoadRequest");
                req.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse(args.Name), callObject);
                return;
            }

            GameObject prefab = await provider.Provide(args.Name, log);

            if (prefab == null)
            {
                log.AddWarning(callerName, callerName, $"{req.GetIDString()} Could not load prefab with name {args.Name}, does not exist");
                req.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse(args.Name), callObject);
            }

            log.AddInfo(callerName, callerName, $"{req.GetIDString()} Assetloaded. Responding with asset {prefab.name}");
            req.Respond(ApparatusResponseObject.AssetResponse(prefab), callObject);
        }


        /// <summary>
        /// Generic method for loading and deserailizing apparatus using apparatus requests. 
        /// </summary>
        /// <param name="provider">Implementation of asset loading</param>
        /// <param name="req">The origin request of the load</param>
        /// <param name="callObject">The object calling</param>
        /// <param name="callerName">The object name to be used in logs</param>
        /// <param name="log">The log write to log to</param>
        public static async void ApparatusLoadAndDeserialize(IApparatusProvider provider, ApparatusRequest req, object callObject, string callerName, LogWriter log)
        {
            log.AddInfo(callerName, callerName, $"{req.GetIDString()} Starting apparatus deserailization");
            if (!req.TryClaim(callObject))
            {
                log.AddWarning(callerName, callerName, $"{req.GetIDString()} Apparatus request already claimed. Aborting");
                return;
            }

            ApparatusLoadRequestArgs args = req.RequestObject.Args as ApparatusLoadRequestArgs;

            if (args == null)
            {
                log.AddError(callerName, callerName, $"{req.GetIDString()} Failed to get AssetLoadRequestArgs from AssetLoadRequest");
                req.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse(args.Identifier), callObject);
                return;
            }

            SrApparatus appa = await provider.Provide(args.Identifier, log);

            if (appa == null)
            {
                log.AddWarning(callerName, callerName, $"{req.GetIDString()} Could not load prefab with name {args.Identifier}, does not exist");
                req.Respond(ApparatusResponseObject.NotYetLoadedOrMissingReferenceResponse(args.Identifier), callObject);
            }

            log.AddInfo(callerName, callerName, $"{req.GetIDString()} Assetloaded. Responding with asset {appa.Identifier}");
            req.Respond(ApparatusResponseObject.SerializeNodeResponse(appa), callObject);
        }
    }
}