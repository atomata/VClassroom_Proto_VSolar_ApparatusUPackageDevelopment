using HexCS.Core;

using System;

namespace HexUN.Framework.SharedResource
{
    /// <summary>
    /// A resource that is shared between multiple classes. Updates
    /// can be pushed
    /// </summary>
    public interface ISharedResource
    {
        /// <summary>
        /// Subscribe to the on update event, invoked whenever an update is pushed to the shared resource
        /// </summary>
        IEventSubscriber OnUpdated { get; }

        /// <summary>
        /// Indicate to the resource that all updates have been pushed
        /// </summary>
        void PushUpdate();
    }
}