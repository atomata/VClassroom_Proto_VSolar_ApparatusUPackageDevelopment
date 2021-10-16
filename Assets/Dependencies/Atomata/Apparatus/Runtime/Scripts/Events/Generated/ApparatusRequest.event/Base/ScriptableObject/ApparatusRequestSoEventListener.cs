using HexUN.Events;
using UnityEngine;
using UnityEngine.Events;
using Atomata.VSolar.Apparatus;

namespace Atomata.VSolar.Events
{
   [AddComponentMenu("Atomata/Apparatus/Events/ApparatusRequest/ApparatusRequestSoEventListener")]
   public class ApparatusRequestSoEventListener : ScriptableObjectEventListener<ApparatusRequest, ApparatusRequestSoEvent, ApparatusRequestUnityEvent>
   {
   }
}