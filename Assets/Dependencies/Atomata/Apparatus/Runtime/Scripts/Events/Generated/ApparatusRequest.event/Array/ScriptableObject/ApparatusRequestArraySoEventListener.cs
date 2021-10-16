using HexUN.Events;
using UnityEngine;
using UnityEngine.Events;
using Atomata.VSolar.Apparatus;

namespace Atomata.VSolar.Events
{
   [AddComponentMenu("Atomata/Apparatus/Events/ApparatusRequestArray/ApparatusRequestArraySoEventListener")]
   public class ApparatusRequestArraySoEventListener : ScriptableObjectEventListener<ApparatusRequest[], ApparatusRequestArraySoEvent, ApparatusRequestArrayUnityEvent>
   {
   }
}