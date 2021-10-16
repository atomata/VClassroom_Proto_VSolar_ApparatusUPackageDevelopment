using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Texture3DArray/Texture3DArraySoEventListener")]
   public class Texture3DArraySoEventListener : ScriptableObjectEventListener<Texture3D[], Texture3DArraySoEvent, Texture3DArrayUnityEvent>
   {
   }
}