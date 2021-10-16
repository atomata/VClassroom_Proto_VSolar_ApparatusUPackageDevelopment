using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexUN.App
{
    /// <summary>
    /// Await all previous load and unload tasks before continuing
    /// </summary>
    [CreateAssetMenu(fileName = "AwaitAllLoadTask", menuName = "HexUN/App/Scene/Tasks/AwaitAllLoadTask")]
    public class AwaitAllLoadTask : ASceneLoadTask
    {
        /// <summary>
        /// Returns false until all currentTasks are complete
        /// </summary>
        /// <param name="currentScenes"></param>
        /// <param name="currentTasks"></param>
        /// <returns></returns>
        public override bool PerformTask(List<SceneToken> currentScenes, List<AsyncOperation> currentTasks)
        {
            int notDone = currentTasks.Count(a => !a.isDone);
            return notDone == 0;
        }
    }
}
