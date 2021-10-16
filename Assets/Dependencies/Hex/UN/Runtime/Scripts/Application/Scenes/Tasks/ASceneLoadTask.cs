using System.Collections.Generic;
using UnityEngine;

namespace HexUN.App
{
    /// <summary>
    /// A task that can be provided to the scene tracker
    /// </summary>
    public abstract class ASceneLoadTask : ScriptableObject
    {
        /// <summary>
        /// Returns true if next task can be performed, otherwise false.
        /// currentTasks are the current operating load tasks, meaning the task has access to other async operations that are occuring. Used for checks, or adding tasks. 
        /// currentScenes is a list of token representing the current scenes that have been, or are being loaded.
        /// </summary>
        /// <returns></returns>
        public abstract bool PerformTask(List<SceneToken> currentScenes, List<AsyncOperation> currentTasks);
    }
}
