using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Vector2Array/Vector2ArraySoEventListener")]
   public class Vector2ArraySoEventListener : ScriptableObjectEventListener<Vector2[], Vector2ArraySoEvent, Vector2ArrayUnityEvent>
   {
   }
}