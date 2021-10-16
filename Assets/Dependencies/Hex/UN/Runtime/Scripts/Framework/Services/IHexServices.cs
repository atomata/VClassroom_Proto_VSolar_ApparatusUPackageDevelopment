using HexUN.App;
using HexUN.Framework.Debugging;
using HexCS.Core;

namespace HexUN.Framework.Services
{
    public interface IHexServices
    {
        /// <summary>
        /// Subscriber for Update functions
        /// </summary>
        public IEventSubscriber OnUpdate { get; }

        /// <summary>
        /// Subscriber for LateUpdate functions
        /// </summary>
        public IEventSubscriber OnLateUpdate { get; }

        /// <summary>
        /// Subscriber for fixed update functions
        /// </summary>
        public IEventSubscriber OnFixedUpdate { get; }

        /// <summary>
        /// Provides access to application control singleton
        /// </summary>
        public IAppControl AppControl { get; }

        /// <summary>
        /// Returns the loggin mechanism for the framework
        /// </summary>
        public ILog Log { get; }
    }
}