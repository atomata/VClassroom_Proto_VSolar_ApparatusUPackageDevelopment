using UnityEngine;
using HexUN.Events;
using Atomata.VSolar.Apparatus;

namespace Atomata.VSolar.Events
{
   [CreateAssetMenu(fileName = "ApparatusRequestArraySoEvent", menuName = "Atomata/Apparatus/Events/ApparatusRequestArray")]
   public class ApparatusRequestArraySoEvent : ScriptableObjectEvent<ApparatusRequest[]>
   {
   }
}