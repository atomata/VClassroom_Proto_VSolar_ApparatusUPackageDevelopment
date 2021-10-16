using System;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Response to the environment to the apparatus
    /// </summary>
    [Serializable]
    public class ApparatusResponseObject
    {
        /// <summary>
        /// The status of the response. Is the response successful? Or an error
        /// </summary>
        public EApparatusResponseStatus Status { get; private set; }

        /// <summary>
        /// The data provided by the response
        /// </summary>
        public object ResponseData { get; private set; }

        /// <summary>
        /// Has the response failed
        /// </summary>
        public bool Failed => Status ==
            EApparatusResponseStatus.Failed_Generic ||
            Status == EApparatusResponseStatus.Failed_ReferenceMissing;

        /// <summary>
        /// Make a response object with option data
        /// </summary>
        public ApparatusResponseObject(EApparatusResponseStatus status, object data = null)
        {
            Status = status;
            ResponseData = data;
        }

        /// <summary>
        /// Return a response that provides an asset
        /// </summary>
        public static ApparatusResponseObject AssetResponse(GameObject ob)
        {
            return new ApparatusResponseObject(EApparatusResponseStatus.Success, ob);
        }

        /// <summary>
        /// Return a response that provides an asset
        /// </summary>
        public static ApparatusResponseObject SerializeNodeResponse(SrApparatus node)
        {
            return new ApparatusResponseObject(EApparatusResponseStatus.Success, node);
        }

        /// <summary>
        /// Returns a response with no object body indicating success
        /// </summary>
        public static ApparatusResponseObject SuccessResponse()
        {
            return new ApparatusResponseObject(EApparatusResponseStatus.Success);
        }

        /// <summary>
        /// Returns a status response with no object body indicating some status
        /// </summary>
        public static ApparatusResponseObject StatusResponse(EApparatusResponseStatus status = EApparatusResponseStatus.Success)
        {
            return new ApparatusResponseObject(status);
        }

        /// <summary>
        /// Returns a response indicating that a core resourse is not yet loaded or missing
        /// </summary>
        public static ApparatusResponseObject NotYetLoadedOrMissingReferenceResponse(string resourceName)
        {
            return new ApparatusResponseObject(EApparatusResponseStatus.Failed_ReferenceMissing, resourceName);
        }
    }
}