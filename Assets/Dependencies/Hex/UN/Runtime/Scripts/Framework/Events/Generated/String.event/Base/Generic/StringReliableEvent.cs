using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class StringReliableEvent : ReliableEvent<String, StringUnityEvent>
   {
   }
}