using UnityEngine;
using System;

namespace HexUN.Events
{
   [CreateAssetMenu(fileName = "StringArraySoEvent", menuName = "HexUN/Events/StringArray")]
   public class StringArraySoEvent : ScriptableObjectEvent<String[]>
   {
   }
}