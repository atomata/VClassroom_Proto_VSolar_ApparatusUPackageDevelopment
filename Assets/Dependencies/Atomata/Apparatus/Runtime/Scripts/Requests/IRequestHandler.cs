using HexUN.Framework.Debugging;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public interface IRequestHandler
    {
        void HandleRequest(ApparatusRequest req, LogWriter log);
    }
}