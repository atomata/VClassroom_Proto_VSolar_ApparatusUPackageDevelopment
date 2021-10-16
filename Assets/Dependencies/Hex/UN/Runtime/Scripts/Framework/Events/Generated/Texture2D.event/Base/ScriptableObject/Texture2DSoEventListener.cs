using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Texture2D/Texture2DSoEventListener")]
   public class Texture2DSoEventListener : ScriptableObjectEventListener<Texture2D, Texture2DSoEvent, Texture2DUnityEvent>
   {
   }
}