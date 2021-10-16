using HexUN.Behaviour;
using UnityEngine;

namespace HexUN.App
{
    /// <summary>
    /// Simple Mono for registering a scene to the SceneLoadManager. Event should be used
    /// to call the RegisterSceneTokenFunction. The most common event is emitted from the 
    /// the SceneLoadManager, the OnRegisterCurrentlyLoadedScenes event. 
    /// </summary>
    public class SceneRegisterer : HexBehaviour
    {
        #region API
        [Tooltip("TokenToRegister")]
        public SceneToken Token;

        /// <summary>
        /// Register the token to the SceneLoadManager
        /// </summary>
        public void RegisterToken()
        {
            SceneLoadManager.Instance.RegisterLoadedOrLoadingSceneToken(Token);
            Destroy(gameObject);
        }
        #endregion
    }
}