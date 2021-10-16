using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class BooleanReliableEvent : ReliableEvent<Boolean, BooleanUnityEvent>
   {
   }
}