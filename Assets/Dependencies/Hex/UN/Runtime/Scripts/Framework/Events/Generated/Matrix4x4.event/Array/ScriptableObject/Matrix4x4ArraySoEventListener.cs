using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Matrix4x4Array/Matrix4x4ArraySoEventListener")]
   public class Matrix4x4ArraySoEventListener : ScriptableObjectEventListener<Matrix4x4[], Matrix4x4ArraySoEvent, Matrix4x4ArrayUnityEvent>
   {
   }
}