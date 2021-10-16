using HexUN.Deps;
using HexUN.Framework.Debugging;
using HexUN.Framework.Services;
using HexUN.Behaviour;

using UnityEngine;
using HexUN.App;
using HexCS.Core;
using Event = HexCS.Core.Event;

namespace HexUN.Framework
{
    /// <summary>
    /// For use in the Hex Framework. Provides application level dependencies
    /// such as logging, etc. Allows for configuration of these components. 
    /// </summary>
    public class OneHexServices : AOneHexPersistent<OneHexServices>
    {
        [SerializeField]
        [Tooltip("Prefab, instance or scriptable object that provides App Lifecycle Control functions")]
        private Object IAppControl = null;

        [SerializeField]
        [Tooltip("Prefab, instance or scriptable object that provides ILog interface dependency to the Unity application")]
        private Object ILog = null;

        private Event _OnUpdate = new Event();

        private Event _OnLateUpdate = new Event();

        private Event _OnFixedUpdate = new Event();

        private IAppControl _appControl;
        private ILog _log;

#if UNITY_EDITOR
        protected void OnValidate()
        {
            DoResolves();
        }
#endif

        protected override void HexAwake()
        {
            base.HexAwake();
            DoResolves();
        }

        #region API
        /// <inheritdoc />
        public IEventSubscriber OnUpdate => _OnUpdate;

        /// <inheritdoc />
        public IEventSubscriber OnLateUpdate => _OnLateUpdate;

        /// <inheritdoc />
        public IEventSubscriber OnFixedUpdate => _OnFixedUpdate;

        /// <inheritdoc />
        public IAppControl AppControl => GetService<IAppControl, OneAppControl>(ref _appControl);

        /// <inheritdoc />
        public ILog Log => GetService<ILog, OneHexLog>(ref _log);
        #endregion

        private TService GetService<TService, TDefault>(ref TService expected)
            where TDefault : AOneHexPersistent<TDefault>, TService
        {
            if (expected == null) expected = AOneHexPersistent<TDefault>.Instance;
            return expected;
        }

        private void Update()
        {
            _OnUpdate.Invoke();
        }

        private void LateUpdate()
        {
            _OnLateUpdate.Invoke();
        }

        private void FixedUpdate()
        {
            _OnFixedUpdate.Invoke();
        }

        private void DoResolves()
        {
            if (ILog == null) ILog = OneHexLog.Instance;
            UTDependency.Resolve(ref ILog, out _log, this);

            if (IAppControl == null) IAppControl = OneAppControl.Instance;
            UTDependency.Resolve(ref IAppControl, out _appControl, this);
        }
    }
}