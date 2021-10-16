using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class BooleanArrayReliableEvent : ReliableEvent<Boolean[], BooleanArrayUnityEvent>
   {
   }
}