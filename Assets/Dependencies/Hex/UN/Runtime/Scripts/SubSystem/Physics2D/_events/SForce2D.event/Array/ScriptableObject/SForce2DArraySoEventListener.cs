using HexUN.Events;
using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Physics2D
{
   [AddComponentMenu("HexUN/Physics2D/Events/SForce2DArray/SForce2DArraySoEventListener")]
   public class SForce2DArraySoEventListener : ScriptableObjectEventListener<SForce2D[], SForce2DArraySoEvent, SForce2DArrayUnityEvent>
   {
   }
}