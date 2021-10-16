using HexUN.Events;
using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Physics2D
{
   [AddComponentMenu("HexUN/Physics2D/Events/SForce2D/SForce2DSoEventListener")]
   public class SForce2DSoEventListener : ScriptableObjectEventListener<SForce2D, SForce2DSoEvent, SForce2DUnityEvent>
   {
   }
}