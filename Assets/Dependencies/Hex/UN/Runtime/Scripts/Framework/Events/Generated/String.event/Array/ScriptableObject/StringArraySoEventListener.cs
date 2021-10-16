using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/StringArray/StringArraySoEventListener")]
   public class StringArraySoEventListener : ScriptableObjectEventListener<String[], StringArraySoEvent, StringArrayUnityEvent>
   {
   }
}