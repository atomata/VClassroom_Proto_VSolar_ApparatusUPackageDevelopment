using HexUN.App;
using HexUN.Framework;

using UnityEngine;

namespace HexUN.Behaviour
{
    /// <summary>
    /// Implementation of Singleton for MonBehavious that insures only a single instance
    /// of the Monobehaviour exists in the scene. The instance lives with the scene, and is destroyed when the scene
    /// is unloaded
    /// </summary>
    /// <typeparam name="TSingleton">The type of the subclass.</typeparam>
    public abstract class AOneHexScene<TSingleton> : AQuitter where TSingleton : AOneHexScene<TSingleton>
    {
        private static TSingleton _instance;

        private static bool _instantiating = false;

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static TSingleton Instance
        {
            get
            {
#if !UNITY_EDITOR
                if (AppQuitting)
                {
                    Debug.LogWarning($"No {nameof(AOneHexScene<TSingleton>)} returned because application is quiting");
                    return null;
                }
#endif
                UTOneHex.TryCreateSingleton(_instance, ref _instance, ref _instantiating);
                return _instance;
            }
        }

        protected override void HexAwake() => UTOneHex.TryCreateSingleton((TSingleton)this, ref _instance, ref _instantiating);

        /// <summary>
        /// When Destroying the instance, set the static instance var to null
        /// </summary>
        protected override void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}