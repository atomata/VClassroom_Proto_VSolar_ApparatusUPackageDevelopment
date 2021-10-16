using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexUN.App
{
    /// <summary>
    /// Unloads a single scene by name
    /// </summary>
    [CreateAssetMenu(fileName = "UnloadSceneByNameTask", menuName = "HexUN/App/Scene/Tasks/UnloadSceneByNameTask")]
    public class UnloadSceneByNameTask : ASceneLoadTask
    {
        public string SceneName;

        public UnloadSceneByNameTask(string sceneName) => SceneName = sceneName;

        public override bool PerformTask(List<SceneToken> currentScenes, List<AsyncOperation> currentTasks)
        {
            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(SceneName);
            currentTasks.Add(asyncLoad);

            // Remove the scenes that are being unloaded
            foreach (SceneToken rem in currentScenes.Where(e => e.Name == SceneName)) currentScenes.Remove(rem);

            return true;
        }
    }
}
