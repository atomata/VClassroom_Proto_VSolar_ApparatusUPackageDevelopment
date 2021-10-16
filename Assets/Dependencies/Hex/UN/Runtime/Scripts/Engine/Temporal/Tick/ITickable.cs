using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.Temporal
{
    /// <summary>
    /// This class can be ticked witha float delta representing tick proportion, 
    /// normally Time.DeltaTime;
    /// </summary>
    public interface ITickable
    {
        /// <summary>
        /// Call the tick function with the time delta, normally Time.deltaTime
        /// </summary>
        /// <param name="delta"></param>
        void Tick(float delta);
    }
}