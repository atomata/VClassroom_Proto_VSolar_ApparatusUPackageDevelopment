using System.Collections;
using HexUN.Behaviour;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexUN.App
{
    /// <summary>
    /// Loads scenes
    /// </summary>
    public class SceneLoader : HexBehaviour
    {
        [SerializeField]
        [Tooltip("Scene load instruction")]
        private SceneLoadTaskList _tasks = null;

        public void LoadScene()
        {
            if (_tasks == null) return;
            SceneLoadManager.Instance.QueueTasks(_tasks.LoadTasks);
        }
    }
}