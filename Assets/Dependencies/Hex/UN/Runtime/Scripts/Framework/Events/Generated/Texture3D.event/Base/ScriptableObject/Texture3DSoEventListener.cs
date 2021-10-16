using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Texture3D/Texture3DSoEventListener")]
   public class Texture3DSoEventListener : ScriptableObjectEventListener<Texture3D, Texture3DSoEvent, Texture3DUnityEvent>
   {
   }
}