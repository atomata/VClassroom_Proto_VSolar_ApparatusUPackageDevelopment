using HexUN.Framework.Request;

using System;

namespace Atomata.VSolar.Apparatus
{
    [Serializable]
    public class ApparatusRequest : Request<ApparatusRequestObject, ApparatusResponseObject> 
    {
        public ApparatusRequest(ApparatusRequestObject request) : base(request) { }

        /// <summary>
        /// Used in debugging to provide a string formated <TRIG#-HashCode> to 
        /// identify the trigger in logs
        /// </summary>
        /// <returns></returns>
        public string GetIDString()
        {
            return $"<REQU#-{GetHashCode()}>";
        }

    }
}