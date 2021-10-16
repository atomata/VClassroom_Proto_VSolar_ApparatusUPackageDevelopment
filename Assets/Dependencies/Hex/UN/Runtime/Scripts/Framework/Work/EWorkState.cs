using UnityEngine;

namespace HexUN.Work
{
    /// <summary>
    /// The state of some object. Normally used to determine if an
    /// object is too busy to take on more work, or is ready. 
    /// </summary>
    public enum EWorkState 
    {
        /// <summary>
        /// No work is being performed
        /// </summary>
        Idle = 0,

        /// <summary>
        /// Work is being performed
        /// </summary>
        Busy = 1,

        /// <summary>
        /// Some error has aoccured which is blocking work, 
        /// however no work is being performed
        /// </summary>
        Error = 2
    }
}