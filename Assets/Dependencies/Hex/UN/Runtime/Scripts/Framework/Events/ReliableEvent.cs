using System;
using HexCS.Core;
using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
    [System.Serializable]
    public class ReliableEvent<TType, TUnityEvent> : IEvent<TType>
        where TUnityEvent : UnityEvent<TType>
    {
        public Event<TType> _event = new Event<TType>();

        [SerializeField]
        private TUnityEvent _unityEvent = null;

        public void Invoke(TType args)
        {
            _event.Invoke(args);
            if (_unityEvent != null) _unityEvent.Invoke(args);
        }

        public EventBinding Subscribe(Action<TType> callback)
        {
            return _event.Subscribe(callback);
        }

        public EventBinding SubscribeSingleUse(Action<TType> callback)
        {
            return _event.SubscribeSingleUse(callback);
        }

        public void Unsubscribe(Action<TType> callback)
        {
            _event.Unsubscribe(callback);
        }
    }
}