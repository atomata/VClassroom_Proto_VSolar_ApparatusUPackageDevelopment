using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/String/StringSoEventListener")]
   public class StringSoEventListener : ScriptableObjectEventListener<String, StringSoEvent, StringUnityEvent>
   {
   }
}