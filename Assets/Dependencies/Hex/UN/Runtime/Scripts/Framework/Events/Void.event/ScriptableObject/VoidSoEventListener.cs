using UnityEngine;
using UnityEngine.Events;
using System;
using HexUN.Behaviour;
using HexCS.Core;

namespace HexUN.Events
{
   [AddComponentMenu("HexUN/Events/Void/VoidSoEventListener")]
   public class VoidSoEventListener : MonoBehaviour
   {
        [SerializeField]
        [Tooltip("The scriptable object the listener listens to")]
        private VoidSoEvent _userChosenEvent = null;

        // The event used by the listener. This should be _userChosenEvent after OnValidate
        private VoidSoEvent _currentEvent = null;

        private EventBinding _currentEventBinding = null; // The binding of current event and callback

        [SerializeField]
        [Tooltip("The event callback that is called when SO event is invoked")]
        private UnityEvent _unityEvent = new UnityEvent();

        #region API
        /// <summary>
        /// The Unity Event
        /// </summary>
        public UnityEvent UnityEvent => _unityEvent;

        /// <summary>
        /// Change the current SO event attached to this listener to another SO event. Setting it to null
        /// effectively stops the listener completely. 
        /// </summary>
        public VoidSoEvent Event
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
        /// Invokes the event as if there was a scriptable object event invoked
        /// </summary>
        public void Invoke()
        {
            _unityEvent?.Invoke();
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
                _currentEventBinding = _currentEvent.Event.Subscribe(() => _unityEvent?.Invoke());
                _userChosenEvent = _currentEvent;
            }
        }
    }
}