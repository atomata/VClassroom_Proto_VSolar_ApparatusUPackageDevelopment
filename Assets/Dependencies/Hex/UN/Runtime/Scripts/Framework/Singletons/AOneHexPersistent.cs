using HexUN.App;
using UnityEditor;
using UnityEngine;

namespace HexUN.Framework
{
    /// <summary>
    /// Implementation of Singleton for MonBehavious that insures only a single instance
    /// of the Monobehaviour exists in the scene, and that instance is not destroyed
    /// between scene loads
    /// </summary>
    /// <typeparam name="TSingleton">The type of the subclass.</typeparam>
    public abstract class AOneHexPersistent<TSingleton> : AQuitter 
        where TSingleton : AOneHexPersistent<TSingleton>
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
                    Debug.LogWarning($"No {nameof(AOneHexPersistent<TSingleton>)} returned because application is quiting");
                    return null;
                }
#endif
                if(UTOneHex.TryCreateSingleton(_instance, ref _instance, ref _instantiating))
                {
                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }

        protected override void HexAwake()
        {
            UTOneHex.TryCreateSingleton((TSingleton)this, ref _instance, ref _instantiating);
            
            if(_instance == this)
            {
                DontDestroyOnLoad(_instance);
            }
        }
    }
}