using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Object/ObjectSoEventListener")]
   public class ObjectSoEventListener : ScriptableObjectEventListener<object, ObjectSoEvent, ObjectUnityEvent>
   {
   }
}