using UnityEngine;
using System;

namespace HexUN.Events
{
   [CreateAssetMenu(fileName = "SingleArraySoEvent", menuName = "HexUN/Events/SingleArray")]
   public class SingleArraySoEvent : ScriptableObjectEvent<Single[]>
   {
   }
}