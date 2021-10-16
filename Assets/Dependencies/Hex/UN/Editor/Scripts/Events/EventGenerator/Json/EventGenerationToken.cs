using System;
using UnityEngine;

namespace HexUN.Events
{
    /// <summary>
    /// Represents a single event being generated
    /// </summary>
    [Serializable]
    public class EventGenerationToken
    {
        /// <summary>
        /// Asset relative path
        /// </summary>
        public string Path;

        /// <summary>
        /// Name of the event type
        /// </summary>
        public string Type;

        /// <summary>
        /// Name of the resulting event namespace
        /// </summary>
        public string Namespace;

        /// <summary>
        /// Menu path to use for events
        /// </summary>
        public string MenuPath;

        /// <summary>
        /// Namespace of the event type, used in generation to not repeat usings
        /// </summary>
        public string EventTypeNamespace;
    }
}