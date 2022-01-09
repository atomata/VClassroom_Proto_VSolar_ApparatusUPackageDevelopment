using System;

using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Requets made from the apparatus to the environment
    /// </summary>
    [Serializable]
    public class ApparatusRequestObject
    {
        /// <summary>
        /// The type of request
        /// </summary>
        public EApparatusRequestType Type { get; private set; }

        /// <summary>
        /// Arguments used in the request
        /// </summary>
        public object Args { get; private set; }

        /// <summary>
        /// Create an ApparatusRequestObject
        /// </summary>
        private ApparatusRequestObject(EApparatusRequestType type, object args = null)
        {
            Type = type;
            Args = args;
        }

        /// <summary>
        /// Request an asset
        /// </summary>
        public static ApparatusRequestObject LoadAsset(string identifier)
        {
            return new ApparatusRequestObject(
                EApparatusRequestType.LoadAsset, 
                new AssetLoadRequestArgs(identifier)
            );
        }

        /// <summary>
        /// Request an asset is saved
        /// </summary>
        public static ApparatusRequestObject SaveAsset(string identifier, GameObject instance)
        {
            return new ApparatusRequestObject(
                EApparatusRequestType.SaveAsset,
                new AssetSaveRequestArgs(identifier, instance)
            );
        }

        /// <summary>
        /// Request an asset
        /// </summary>
        public static ApparatusRequestObject LoadApparatus(string identifier)
        {
            return new ApparatusRequestObject(
                EApparatusRequestType.LoadApparatus,
                new ApparatusLoadRequestArgs(identifier)
            );
        }

        /// <summary>
        /// Request that the given transform be focused on. The transform represents
        /// the final transform of the camera in world space
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ApparatusRequestObject CameraFocus(Transform t)
            => new ApparatusRequestObject(EApparatusRequestType.CameraFocus, t);
    }
}