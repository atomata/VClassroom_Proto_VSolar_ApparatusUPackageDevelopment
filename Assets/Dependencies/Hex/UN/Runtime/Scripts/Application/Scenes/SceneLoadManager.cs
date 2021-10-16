using System.Collections;
using System.Collections.Generic;
using HexUN.Events;
using UnityEngine;
using HexUN.Framework;

namespace HexUN.App
{
    /// <summary>
    /// Scene tracker is a singleton that tracks the loaded scenes and knows what needs unloading
    /// between scenes
    /// </summary>
    public class SceneLoadManager : AOneHexPersistent<SceneLoadManager>
    {
        [SerializeField]
        [Tooltip("This event is fired on awake so that any currenly loaded scenes can be registed using the RegisterLoadedOrLoadingSceneToken function. Normally used in conjunction with the SceneRegisterer so that play mode acts the same way as starting the app from the beginning")]
        private VoidReliableEvent _onRegisterCurrentlyLoadedScenes = null;

        private List<SceneToken> _loadedOrLoadingScenes = new List<SceneToken>();

        private Queue<ASceneLoadTask> _sceneTasks = new Queue<ASceneLoadTask>();
        private List<AsyncOperation> _currentOperations = new List<AsyncOperation>();
        private Coroutine _operationCoroutine = null;

        #region API
        /// <summary>
        /// Manually register a scene token to simulate an async load by the 
        /// scene manager without actuall queuing the task. This is most commonly
        /// used in scenes in the unity editor to simulate the same behaviour as
        /// when they are loaded by the scene manager. 
        /// </summary>
        /// <param name="token"></param>
        public void RegisterLoadedOrLoadingSceneToken(SceneToken token)
        {
            _loadedOrLoadingScenes.Add(token);
        }

        /// <summary>
        /// Queue scene load tasks that will be processed asyncronously by the scene loader
        /// </summary>
        /// <param name="tasks"></param>
        public void QueueTasks(params ASceneLoadTask[] tasks)
        {
            if (tasks == null) return;

            foreach(ASceneLoadTask task in tasks) _sceneTasks.Enqueue(task);

            if(_operationCoroutine == null)
            {
                StartCoroutine(HandleSceneQueue());
            }
        }
        #endregion

        protected override void HexStart()
        {
            base.HexStart();
            _onRegisterCurrentlyLoadedScenes.Invoke();
        }

        private void ResolveQueueHandler()
        {
            if (_operationCoroutine == null)
            {
                _operationCoroutine = StartCoroutine(HandleSceneQueue());
            }
        }

        IEnumerator HandleSceneQueue()
        {
            _currentOperations.Clear();

            while(_sceneTasks.Count > 0)
            {
                ASceneLoadTask task = _sceneTasks.Peek();
                
                if(task.PerformTask(_loadedOrLoadingScenes, _currentOperations))
                {
                    _sceneTasks.Dequeue();
                }

                yield return null;
            }

            _operationCoroutine = null;
        }
    }
}