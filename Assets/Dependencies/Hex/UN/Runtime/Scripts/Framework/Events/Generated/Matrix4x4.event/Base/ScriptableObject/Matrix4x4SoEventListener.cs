using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Matrix4x4/Matrix4x4SoEventListener")]
   public class Matrix4x4SoEventListener : ScriptableObjectEventListener<Matrix4x4, Matrix4x4SoEvent, Matrix4x4UnityEvent>
   {
   }
}