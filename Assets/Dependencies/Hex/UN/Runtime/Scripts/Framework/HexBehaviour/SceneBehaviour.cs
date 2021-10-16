using HexUN.Behaviour;

using UnityEngine.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace HexUN.Framework.Behaviour
{
    [ExecuteAlways]
    public abstract class SceneBehaviour : HexBehaviour
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            SceneInitalize();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SceneDenitialize();
        }
        
        /// <summary>
        /// Perform all initializations requires so that the object works
        /// when the scene initializes, recompiles, or the object is
        /// first added to the scene
        /// </summary>
        protected abstract void SceneInitalize();

        /// <summary>
        /// Perform all clean up that should occur when the scene is left
        /// or the object is deleted
        /// </summary>
        protected abstract void SceneDenitialize();
    }
}