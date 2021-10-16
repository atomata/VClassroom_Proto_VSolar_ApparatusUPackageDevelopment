using HexCS.Core;
using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
    /// <summary>
    /// Can be added as a component to any game object ot listen to events. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TScriptableObjectEvent"></typeparam>
    /// <typeparam name="TUnityEvent"></typeparam>
    public class ScriptableObjectEventListener<T, TScriptableObjectEvent, TUnityEvent> : MonoBehaviour
        where TScriptableObjectEvent : ScriptableObjectEvent<T>
        where TUnityEvent : UnityEvent<T>, new()
    {
        [SerializeField]
        [Tooltip("The scriptable object the listener listens to")]
        private TScriptableObjectEvent _userChosenEvent = null;

        // The event used by the listener. This should be _userChosenEvent after OnValidate
        private TScriptableObjectEvent _currentEvent = null;

        private EventBinding _currentEventBinding = null; // The binding of current event and callback

        [SerializeField]
        [Tooltip("The event callback that is called when SO event is invoked")]
        private TUnityEvent _unityEvent = new TUnityEvent();

        #region API
        /// <summary>
        /// The Unity Event
        /// </summary>
        public TUnityEvent UnityEvent => _unityEvent;

        /// <summary>
        /// Change the current SO event attached to this listener to another SO event. Setting it to null
        /// effectively stops the listener completely. 
        /// </summary>
        public TScriptableObjectEvent Event
        {
            get => _currentEvent;
            set
            {
                if (_currentEvent == value) return;
                UnregisterCallback();
                _currentEvent = value;
                RegisterCallback();
            }
        }

        /// <summary>
        /// Invoke event listener as though a scriptable object event had been invoked
        /// </summary>
        /// <param name="args"></param>
        public void Invoke(T args)
        {
            _unityEvent?.Invoke(args);
        }
        #endregion

        private void Awake() => OnValidate();

        private void OnValidate()
        {
            Event = _userChosenEvent;
        }

        private void UnregisterCallback()
        {
            if (_currentEventBinding != null)
            {
                _currentEventBinding.UnSubscribe();
                _currentEventBinding = null;
                _userChosenEvent = null;
            }
        }

        private void RegisterCallback()
        {
            if (_currentEvent != null)
            {
                _currentEventBinding = _currentEvent.Event.Subscribe((T args) => _unityEvent?.Invoke(args));
                _userChosenEvent = _currentEvent;
            }
        }
    }
}