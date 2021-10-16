using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class ObjectArrayReliableEvent : ReliableEvent<object[], ObjectArrayUnityEvent>
   {
   }
}