using UnityEngine;

namespace HexUN.Events
{
   [System.Serializable]
   public class GameObjectArrayReliableEvent : ReliableEvent<GameObject[], GameObjectArrayUnityEvent>
   {
   }
}