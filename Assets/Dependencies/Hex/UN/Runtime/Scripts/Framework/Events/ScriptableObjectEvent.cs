using System;
using HexCS.Core;
using UnityEngine;

namespace HexUN.Events
{
    /// <summary>
    /// An event defined by a scriptable object
    /// </summary>
    public class ScriptableObjectEvent<T> : ScriptableObject
    {
        private Event<T> _event = new Event<T>();

        #region API
        /// <summary>
        /// Get the event subscriber that allows you to subscribe actions
        /// directly to event from code
        /// </summary>
        public IEventSubscriber<T> Event => _event;

        /// <summary>
        /// Invoke the event
        /// </summary>
        /// <param name="args"></param>
        public void Invoke(T args)
        {
            _event.Invoke(args);
        }
        #endregion
    }
}