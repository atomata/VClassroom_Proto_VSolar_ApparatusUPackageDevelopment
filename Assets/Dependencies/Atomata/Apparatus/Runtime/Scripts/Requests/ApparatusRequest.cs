using HexUN.Framework.Request;

using System;

namespace Atomata.VSolar.Apparatus
{
    [Serializable]
    public class ApparatusRequest : Request<ApparatusRequestObject, ApparatusResponseObject> 
    {
        public ApparatusRequest(ApparatusRequestObject request) : base(request) { }
    }
}