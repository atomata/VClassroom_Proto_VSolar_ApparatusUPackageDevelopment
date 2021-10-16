using HexCS.Core;
using UnityEngine;
using UnityEngine.Events;

namespace HexUN.Events
{
    /// <summary>
    /// Used to take in a boolean event and do a different void function if true or false
    /// </summary>
    public class BooleanEventSwitch : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The scriptable object the listener listens to")]
        private BooleanSoEvent _userChosenEvent = null;

        // The event used by the listener. This should be _userChosenEvent after OnValidate
        private BooleanSoEvent _currentEvent = null;

        private EventBinding _currentEventBinding = null; // The binding of current event and callback

        [SerializeField]
        [Tooltip("The event callback that is called when SO event is invoked")]
        private UnityEvent _trueEvent = new UnityEvent();

        [Tooltip("The event callback that is called when SO event is invoked")]
        private UnityEvent _falseEvent = new UnityEvent();

        #region API
        /// <summary>
        /// The Unity Event called on true
        /// </summary>
        public UnityEvent TrueEvent => _trueEvent;

        /// <summary>
        /// The Unity Event called on false
        /// </summary>
        public UnityEvent FalseEvent => _falseEvent;

        /// <summary>
        /// Change the current SO event attached to this listener to another SO event. Setting it to null
        /// effectively stops the listener completely. 
        /// </summary>
        public BooleanSoEvent Event
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
                _currentEventBinding = _currentEvent.Event.Subscribe(
                    (bool args) =>
                    {
                        if (args) _trueEvent?.Invoke();
                        else _falseEvent.Invoke();
                    });
                _userChosenEvent = _currentEvent;
            }
        }
    }
}