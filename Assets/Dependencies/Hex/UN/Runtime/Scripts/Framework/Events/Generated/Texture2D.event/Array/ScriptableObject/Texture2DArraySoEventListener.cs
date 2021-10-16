using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Texture2DArray/Texture2DArraySoEventListener")]
   public class Texture2DArraySoEventListener : ScriptableObjectEventListener<Texture2D[], Texture2DArraySoEvent, Texture2DArrayUnityEvent>
   {
   }
}