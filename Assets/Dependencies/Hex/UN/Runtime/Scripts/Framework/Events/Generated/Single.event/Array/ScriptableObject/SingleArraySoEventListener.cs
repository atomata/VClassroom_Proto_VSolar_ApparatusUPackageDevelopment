using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/SingleArray/SingleArraySoEventListener")]
   public class SingleArraySoEventListener : ScriptableObjectEventListener<Single[], SingleArraySoEvent, SingleArrayUnityEvent>
   {
   }
}