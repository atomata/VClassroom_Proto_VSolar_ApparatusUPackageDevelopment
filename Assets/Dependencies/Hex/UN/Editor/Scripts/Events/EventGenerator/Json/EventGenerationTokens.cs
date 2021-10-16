using System;
using UnityEngine;

namespace HexUN.Events
{
    /// <summary>
    /// Represents a collection of events that need generating
    /// </summary>
    [Serializable]
    public class EventGenerationTokens
    {
        public EventGenerationToken[] Tokens;
    }
}