using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class SingleArrayReliableEvent : ReliableEvent<Single[], SingleArrayUnityEvent>
   {
   }
}