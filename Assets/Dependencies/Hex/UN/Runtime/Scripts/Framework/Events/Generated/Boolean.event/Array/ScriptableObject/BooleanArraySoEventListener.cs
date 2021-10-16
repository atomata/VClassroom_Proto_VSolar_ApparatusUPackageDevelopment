using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/BooleanArray/BooleanArraySoEventListener")]
   public class BooleanArraySoEventListener : ScriptableObjectEventListener<Boolean[], BooleanArraySoEvent, BooleanArrayUnityEvent>
   {
   }
}