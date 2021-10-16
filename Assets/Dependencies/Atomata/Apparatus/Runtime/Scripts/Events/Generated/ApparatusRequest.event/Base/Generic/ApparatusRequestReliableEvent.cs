using HexUN.Events;
using Atomata.VSolar.Apparatus;

namespace Atomata.VSolar.Events
{
   [System.Serializable]
   public class ApparatusRequestReliableEvent : ReliableEvent<ApparatusRequest, ApparatusRequestUnityEvent>
   {
   }
}