using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/GameObjectArray/GameObjectArraySoEventListener")]
   public class GameObjectArraySoEventListener : ScriptableObjectEventListener<GameObject[], GameObjectArraySoEvent, GameObjectArrayUnityEvent>
   {
   }
}