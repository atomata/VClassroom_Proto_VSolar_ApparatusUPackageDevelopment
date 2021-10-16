using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/ObjectArray/ObjectArraySoEventListener")]
   public class ObjectArraySoEventListener : ScriptableObjectEventListener<object[], ObjectArraySoEvent, ObjectArrayUnityEvent>
   {
   }
}