using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class ObjectReliableEvent : ReliableEvent<object, ObjectUnityEvent>
   {
   }
}