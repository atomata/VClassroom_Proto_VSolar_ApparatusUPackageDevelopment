using HexCS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexUN.Utilities 
{
    /// <summary>
    /// Invokes on update whenenver the list is modified. i.e. Add, remove, set, etc. 
    /// Provides the List as argument
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OnUpdateList<T> : List<T>, IUpdateable<IReadOnlyList<T>>
    {
        private Event<IReadOnlyList<T>> _onUpdate = new Event<IReadOnlyList<T>>();

        #region API
        public IEventSubscriber<IReadOnlyList<T>> OnUpdate => _onUpdate;

        public OnUpdateList() : base() { }
        public OnUpdateList(IEnumerable<T> collection) : base(collection) { }
        public OnUpdateList(int capacity) : base(capacity) { }

        public new T this[int index] {
            get => base[index];
            set => base[index] = value;
        }

        public new void Add(T item)
        {
            base.Add(item);
            InvokeUpdate();
        }

        public new void AddRange(IEnumerable<T> collection) {
            base.AddRange(collection);
            InvokeUpdate();
        }

        public new void Insert(int index, T item) {
            base.Insert(index, item);
            InvokeUpdate();
        }
        public new void InsertRange(int index, IEnumerable<T> collection) {
            base.InsertRange(index, collection);
            InvokeUpdate();
        }

        public new bool Remove(T item) {
            if (base.Remove(item))
            {
                InvokeUpdate();
                return true;
            }

            return false;
        }

        public new int RemoveAll(Predicate<T> match)
        {
            int c = base.RemoveAll(match);
            if(c > 0) InvokeUpdate();
            return c;
        }

        public new void RemoveAt(int index) {
            base.RemoveAt(index);
            InvokeUpdate();
        }

        public new void RemoveRange(int index, int count) {
            base.RemoveRange(index, count);
            InvokeUpdate();
        }

        public new void Reverse(int index, int count) {
            base.Reverse(index, count);
            InvokeUpdate();
        }

        public new void Reverse() {
            base.Reverse();
            InvokeUpdate();
        }

        public new void Sort(Comparison<T> comparison) {
            base.Sort(comparison);
            InvokeUpdate();
        }

        public new void Sort(int index, int count, IComparer<T> comparer) {
            base.Sort(index, count, comparer);
            InvokeUpdate();
        }

        public new void Sort() {
            base.Sort();
            InvokeUpdate();
        }

        public new void Sort(IComparer<T> comparer) {
            base.Sort(comparer);
            InvokeUpdate();
        }
        #endregion

        private void InvokeUpdate()
        {
            _onUpdate.Invoke(AsReadOnly());
        }
    }
}