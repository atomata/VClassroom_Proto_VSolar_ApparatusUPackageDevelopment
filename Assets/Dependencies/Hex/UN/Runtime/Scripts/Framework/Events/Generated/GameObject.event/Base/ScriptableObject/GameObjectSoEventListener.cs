using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/GameObject/GameObjectSoEventListener")]
   public class GameObjectSoEventListener : ScriptableObjectEventListener<GameObject, GameObjectSoEvent, GameObjectUnityEvent>
   {
   }
}