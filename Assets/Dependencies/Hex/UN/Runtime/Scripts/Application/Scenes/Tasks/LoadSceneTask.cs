using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexUN.App
{
    /// <summary>
    /// Task to load a scene async
    /// </summary>
    [CreateAssetMenu(fileName = "LoadSceneTask", menuName = "HexUN/App/Scene/Tasks/LoadSceneTask")]
    public class LoadSceneTask : ASceneLoadTask
    {
        public string SceneName;
        public string Tag;

        public LoadSceneTask(string sceneName) => SceneName = sceneName;

        public override bool PerformTask(List<SceneToken> currentScenes, List<AsyncOperation> currentTasks)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            currentTasks.Add(asyncLoad);
            currentScenes.Add(new SceneToken(SceneName, Tag));
            return true;
        }
    }
}
