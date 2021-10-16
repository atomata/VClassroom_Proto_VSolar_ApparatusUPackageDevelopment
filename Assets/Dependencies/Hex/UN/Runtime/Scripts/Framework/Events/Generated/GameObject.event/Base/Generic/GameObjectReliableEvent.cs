using UnityEngine;

namespace HexUN.Events
{
   [System.Serializable]
   public class GameObjectReliableEvent : ReliableEvent<GameObject, GameObjectUnityEvent>
   {
   }
}