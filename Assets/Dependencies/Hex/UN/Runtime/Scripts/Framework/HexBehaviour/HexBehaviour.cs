using System;
using System.Collections.Generic;
using HexCS.Core;
using UnityEngine;

namespace HexUN.Behaviour
{
    /// <summary>
    /// Adds useful functionality to a monebehaviour that makes it easier to manage certain
    /// timing
    /// 
    /// Currenty includes:
    /// - Ability to register functions to occur after Awake() and Start()
    /// </summary>
    public abstract class HexBehaviour : MonoBehaviour
    {
#if HEXDB
        [SerializeField]
        [Header("Debug(HexBehaviour)")]
        protected ObjectSoEvent Log = null;
#endif

        private List<Action<object>> _callAfterStart = new List<Action<object>>();
        private List<Action<object>> _callAfterAwake = new List<Action<object>>();

        private EventBindingGroup _eventBindingGroup = null;

        #region Protected API
        /// <summary>
        /// MonoEnhanced have a mechanism for holding event bindings that
        /// are automatically subscribed and deactivated when the object is enabled,
        /// disabled and destroyed
        /// </summary>
        protected EventBindingGroup EventBindings
        {
            get
            {
                if (_eventBindingGroup == null) _eventBindingGroup = new EventBindingGroup();
                return _eventBindingGroup;
            }
        }

        /// <summary>
        /// Subscribes all cached event bindings on enable
        /// </summary>
        protected virtual void OnEnable()
        {
            _eventBindingGroup?.SubscribeAll();
        }

        /// <summary>
        /// Unsubscribes all event binds on disable
        /// </summary>
        protected virtual void OnDisable()
        {
            _eventBindingGroup?.UnSubscribeAll();
        }

        /// <summary>
        /// Unsubscribes all event binds on destroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            _eventBindingGroup?.ClearAndUnsubscribeAll();
        }

        /// <summary>
        /// Replaces the Awake function for base classes. The Start() funciton is managed by
        /// The MonoEnhanced and contains special functionalities
        /// </summary>
        protected virtual void HexAwake() { }

        /// <summary>
        /// Replaces the Start function for base classes. The Start() funciton is managed by
        /// The MonoEnhanced and contains special functionalities
        /// </summary>
        protected virtual void HexStart() {}

#if HEXDB
        protected void LogInfo<T>(string message)
        {
            if (Log != null)
            {
                CallAfterStart(o => Log.Invoke($"{gameObject.name}:{nameof(T)} - {message}"));
            }
        }
#endif
        #endregion

        #region Public API
        /// <summary>
        /// Has the Start call finished executing
        /// </summary>
        public bool IsStarted { get; private set; } = false;

        /// <summary>
        /// Has the awake call finished executing
        /// </summary>
        public bool IsAwake { get; private set; } = false;

        /// <summary>
        /// If the monobehaviour has not finished Start(), registers
        /// an action that will be called after at the end of Start(),
        /// otherwise jsut calls the action. 
        /// </summary>
        /// <param name="action">An action who's object parameter is this object</param>
        public void CallAfterStart(Action<object> action)
        {
            if (IsStarted)
            {
                action(this);
            }
            else
            {
                _callAfterStart.Add(action);
            }
        }

        /// <summary>
        /// If the monobehaviour has not finished Awake(), registers
        /// an action that will be called after at the end of Awake(),
        /// otherwise jsut calls the action. 
        /// </summary>
        /// <param name="action">An action who's object parameter is this object</param>
        public void CallAfterAwake(Action<object> action)
        {
            if (IsAwake)
            {
                action(this);
            }
            else
            {
                _callAfterAwake.Add(action);
            }
        }
        #endregion

        /// <summary>
        /// Should be called at the end of inherited Start() calls.
        /// This fires all functions registered to PerformAfterStart()
        /// </summary>
        private void Awake()
        {
            HexAwake();
            IsAwake = true;
            foreach (Action<object> o in _callAfterAwake) o(this);
            _callAfterAwake.Clear();

#if HEXDB
            LogInfo<MonoEnhanced>("Awale called, after awake functions have been performed");
#endif
        }

        /// <summary>
        /// Should be called at the end of inherited Start() calls.
        /// This fires all functions registered to PerformAfterStart()
        /// </summary>
        private void Start()
        {
            HexStart();
            IsStarted = true;
            foreach (Action<object> o in _callAfterStart) o(this);
            _callAfterStart.Clear();
        }
    }
}