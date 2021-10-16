using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Int32/Int32SoEventListener")]
   public class Int32SoEventListener : ScriptableObjectEventListener<Int32, Int32SoEvent, Int32UnityEvent>
   {
   }
}