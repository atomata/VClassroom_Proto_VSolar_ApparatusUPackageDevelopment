using UnityEngine;
using HexUN.Events;
using Atomata.VSolar.Apparatus;

namespace Atomata.VSolar.Events
{
   [CreateAssetMenu(fileName = "ApparatusRequestSoEvent", menuName = "Atomata/Apparatus/Events/ApparatusRequest")]
   public class ApparatusRequestSoEvent : ScriptableObjectEvent<ApparatusRequest>
   {
   }
}