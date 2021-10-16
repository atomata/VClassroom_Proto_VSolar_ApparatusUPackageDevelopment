using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Int32Array/Int32ArraySoEventListener")]
   public class Int32ArraySoEventListener : ScriptableObjectEventListener<Int32[], Int32ArraySoEvent, Int32ArrayUnityEvent>
   {
   }
}