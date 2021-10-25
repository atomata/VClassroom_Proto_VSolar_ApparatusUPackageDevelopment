using Atomata.VSolar.Utilities;

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