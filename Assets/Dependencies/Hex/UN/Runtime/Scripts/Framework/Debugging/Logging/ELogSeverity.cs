using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.Framework.Debugging
{
    public enum ELogSeverity
    {
        /// <summary>
        /// Info meant for devleopers
        /// </summary>
        Info = 1,

        /// <summary>
        /// A warning of code that does not immediately break system,
        /// but may be a problem
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Breaking errors
        /// </summary>
        Error = 3
    }
}