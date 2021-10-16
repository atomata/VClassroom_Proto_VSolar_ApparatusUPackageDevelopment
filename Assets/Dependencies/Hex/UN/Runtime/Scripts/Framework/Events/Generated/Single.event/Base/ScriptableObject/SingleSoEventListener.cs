using UnityEngine;
using UnityEngine.Events;
using System;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Single/SingleSoEventListener")]
   public class SingleSoEventListener : ScriptableObjectEventListener<Single, SingleSoEvent, SingleUnityEvent>
   {
   }
}