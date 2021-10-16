using System;
using HexCS.Core;
using UnityEngine;
using UnityEngine.Events;
using Event = HexCS.Core.Event;

namespace HexUN.Events
{
   [System.Serializable]
   public class VoidReliableEvent : IEvent
   {
      public Event _event = new Event();

      [SerializeField]
      private UnityEvent _soEvent = null;

      public void Invoke()
      {
         _event.Invoke();
         if (_soEvent != null) _soEvent.Invoke();
      }

      public EventBinding Subscribe(Action callback)
      {
         return _event.Subscribe(callback);
      }

      public EventBinding SubscribeSingleUse(Action callback)
      {
         return _event.SubscribeSingleUse(callback);
      }

      public void Unsubscribe(Action callback)
      {
         _event.Unsubscribe(callback);
      }

   }
}