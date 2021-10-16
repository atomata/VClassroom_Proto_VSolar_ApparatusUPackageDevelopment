using System.Collections;
using System.Collections.Generic;
using HexCS.Core;
using UnityEngine;

namespace HexUN.Work
{
    /// <summary>
    /// A completion token provides an event subscriber that can be used to subscribe
    /// to the completion of work
    /// </summary>
    public struct CompletionToken
    {
        public IEventSubscriber OnWorkComplete;
    }
}