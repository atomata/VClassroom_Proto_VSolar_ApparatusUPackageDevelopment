using HexCS.Core;

using HexUN.App;
using HexUN.Framework.Debugging;

using UnityEngine;

namespace HexUN.Framework.Services
{
    [CreateAssetMenu(fileName = "HexServices", menuName = "HexUN/Services/HexServices")]
    public class SoHexServices : ScriptableObject, IHexServices
    {
        private const string cLogCategory = nameof(SoHexServices);

        [SerializeField]
        [Tooltip("Prefab, instance or scriptable object that provides App Lifecycle Control functions")]
        private Object IAppControl = null;

        [SerializeField]
        [Tooltip("Prefab, instance or scriptable object that provides ILog interface dependency to the Unity application")]
        private Object ILog = null;

        #region API
        /// <inheritdoc />
        public IEventSubscriber OnUpdate
        {
            get
            {
                Log.Warn(cLogCategory, $"Trying to subscribe to {nameof(OnUpdate)} in editor mode. This isn't allowed");
                return null;
            }
        }

        /// <inheritdoc />
        public IEventSubscriber OnLateUpdate
        {
            get
            {
                Log.Warn(cLogCategory, $"Trying to subscribe to {nameof(OnLateUpdate)} in editor mode. This isn't allowed");
                return null;
            }
        }

        /// <inheritdoc />
        public IEventSubscriber OnFixedUpdate
        {
            get
            {
                Log.Warn(cLogCategory, $"Trying to subscribe to {nameof(OnFixedUpdate)} in editor mode. This isn't allowed");
                return null;
            }
        }

        /// <inheritdoc />
        public IAppControl AppControl => GetInterface<IAppControl>(ref IAppControl);

        /// <inheritdoc />
        public ILog Log => GetInterface<ILog>(ref ILog);

#endregion
        private T GetInterface<T>(ref Object candidate) where T : class
        {
            if (candidate == null)
            {
                Debug.LogWarning($"[{nameof(SoHexServices)}] contains a null Object reference to {nameof(T)} when it should be populated");
                return default;
            }

            T instance = candidate as T;

            if(instance == null)
            {
                Debug.LogWarning($"[{nameof(SoHexServices)}] contains an Object reference that does not cast to {nameof(T)} when it should");
                return default;
            }

            return instance;
        }
    }
}