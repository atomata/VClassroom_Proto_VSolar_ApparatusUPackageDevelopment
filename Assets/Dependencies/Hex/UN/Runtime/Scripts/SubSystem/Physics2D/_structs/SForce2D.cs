using UnityEngine;

namespace HexUN.Physics2D
{
    /// <summary>
    /// Represents a force applied to an object
    /// </summary>
    public struct SForce2D
    {
        /// <summary>
        /// Direction and power of the force
        /// </summary>
        public Vector2 Force;

        /// <summary>
        /// Time over which a force should be applied
        /// </summary>
        public float Time;

        public SForce2D(Vector2 force, float time)
        {
            Force = force;
            Time = time;
        }
    }
}