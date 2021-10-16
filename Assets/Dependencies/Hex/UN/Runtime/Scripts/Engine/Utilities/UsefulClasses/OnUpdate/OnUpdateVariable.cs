using System;
using HexCS.Core;

namespace HexUN.Utilities
{
    /// <summary>
    /// A variable that emits an event when updated
    /// </summary>
    public class OnUpdateVariable<T> : IUpdateable<T>
    {
        private EventBindingGroup _bindings = new EventBindingGroup();

        private Event<T> _onUpdate = new Event<T>();
        private T _value;

        #region API
        public IEventSubscriber<T> OnUpdate => _onUpdate;

        /// <summary>
        /// The current value of the update variable. 
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                if (value == null && _value == null) return;
                if (_value != null && _value.Equals(value)) return;
                _value = value;
                _onUpdate.Invoke(_value);
            }
        }

        /// <summary>
        /// Constructs a new update variable
        /// </summary>
        /// <param name="val"></param>
        public OnUpdateVariable(T val)
        {
            Value = val;            
        }

        /// <summary>
        /// Register a child update variable. If this child update variable 
        /// is updated, then will invoke the update method of this variable. 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="var"></param>
        public void RegisterUpdateVariable<T1>( IUpdateable<T1> var)
        {
            _bindings.Add(var.OnUpdate.Subscribe(t => ForceInvokeUpdate()));
        }

        /// <inheritdoc/>

        public static implicit operator T(OnUpdateVariable<T> val) => val.Value;

        /// <inheritdoc/>

        public static implicit operator OnUpdateVariable<T> (T val) => new OnUpdateVariable<T>(val);

        /// <summary>
        /// Emits the current value as an update
        /// </summary>
        public void ForceInvokeUpdate()
        {
            _onUpdate.Invoke(_value);
        }

        public void Dispose()
        {
            _bindings.ClearAndUnsubscribeAll();
        }
        #endregion
    }
}
