using HexCS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HexUN.Utilities
{
    /// <summary>
    /// A list of update variables that automatically listens to the update of each variable and emits an event when one variable is updated.
    /// Also emits updates when variables are added
    /// </summary>
    public class OnUpdateGroup<T> : IUpdateable<T>, IEnumerable<T>
    {
        private Event<T> _onUpdate = new Event<T>();

        private List<BoundVariable> _variables = new List<BoundVariable>();


        #region API
        public IEventSubscriber<T> OnUpdate => _onUpdate;


        public T this[int index]
        {
            get => _variables[index].Variable.Value;
            set
            {
                if(_variables[index] != null)
                {
                    _variables[index].Dispose();
                }

                _variables[index] = new BoundVariable(value, _onUpdate);
            }
        }

        public void Add(T value)
        {
            _variables.Add(new BoundVariable(value, _onUpdate));
        }

        public void AddRange(params T[] values)
        {
            _variables.AddRange(values.Select( e => new BoundVariable(e, _onUpdate)));
        }

        public bool TryGetFirst(Predicate<T> test, out T value)
        {
            T val = _variables.FirstOrDefault(t => test(t.Variable.Value)).Variable;

            if (val != null)
            {
                value = val;
                return true;
            }

            value = default;
            return false;
        }

        public void Remove(T value)
        {
            BoundVariable b = _variables.FirstOrDefault(v => v.Variable.Value.Equals(value));

            if(b != null)
            {
                b.Dispose();
                _variables.Remove(b);
            }
        }

        public void Clear()
        {
            foreach(BoundVariable var in _variables)
            {
                if (var != null) var.Dispose();
            }

            _variables.Clear();
        }

        public T[] ToArray() =>_variables.Select(e => e.Variable.Value).ToArray();

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => _variables.Select(e => e.Variable.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _variables.Select(e => e.Variable.Value).GetEnumerator();
        #endregion

        private class BoundVariable
        {
            public EventBinding Binding;
            public OnUpdateVariable<T> Variable;

            public BoundVariable(T val, Event<T> evt)
            {
                IOnUpdateType<T> ut = val as IOnUpdateType<T>;

                if (ut != null) Variable = ut.AsUpdateVariable();
                else Variable = new OnUpdateVariable<T>(val);

                Binding = Variable.OnUpdate.Subscribe(evt.Invoke);
            }

            public void Dispose()
            {
                Binding.UnSubscribe();
                Variable.Dispose();
            }
        }
    }
}
