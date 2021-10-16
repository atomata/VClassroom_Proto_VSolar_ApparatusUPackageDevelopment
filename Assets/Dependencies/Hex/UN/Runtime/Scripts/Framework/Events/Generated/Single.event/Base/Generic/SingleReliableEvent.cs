using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class SingleReliableEvent : ReliableEvent<Single, SingleUnityEvent>
   {
   }
}