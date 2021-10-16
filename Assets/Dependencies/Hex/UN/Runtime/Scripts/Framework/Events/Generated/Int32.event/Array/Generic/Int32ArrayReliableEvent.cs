using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class Int32ArrayReliableEvent : ReliableEvent<Int32[], Int32ArrayUnityEvent>
   {
   }
}