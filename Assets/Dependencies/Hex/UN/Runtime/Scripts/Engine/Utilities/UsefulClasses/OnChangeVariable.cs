using System;

namespace HexUN.Utilities
{
    /// <summary>
    /// Holds a variable. This variable can be set externally.
    /// Whenever the variable is set, a function is called on it
    /// </summary>
    public class OnChangeVariable<T>
    {
        private Action<T> _changeAction;

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    _changeAction(_value);
                }
            }
        }

        public OnChangeVariable(Action<T> changeAction, T defaultValue = default)
        {
            _value = defaultValue;
            _changeAction = changeAction;
        }
    }
}