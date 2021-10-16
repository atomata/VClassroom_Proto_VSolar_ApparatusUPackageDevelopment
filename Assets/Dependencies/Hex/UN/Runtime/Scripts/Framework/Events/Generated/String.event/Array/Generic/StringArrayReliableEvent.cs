using System;

namespace HexUN.Events
{
   [System.Serializable]
   public class StringArrayReliableEvent : ReliableEvent<String[], StringArrayUnityEvent>
   {
   }
}