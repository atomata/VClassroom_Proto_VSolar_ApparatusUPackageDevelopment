using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexUN.Data
{
    /// <summary>
    /// Datastructure used to identify object references with a Guid, then resolve those references later
    /// </summary>
    public class GuidCollection<T> : IGuidCollection<T>
    {
        // TO DO: Foreach compatibility
        private Dictionary<Guid, T> _items = new Dictionary<Guid, T>();

        /// <inheritdoc/>
        public T[] Values => _items.Values.ToArray();

        /// <inheritdoc/>
        public Guid[] Guids => _items.Keys.ToArray();

        /// <inheritdoc/>
        public void AddGuidRange(params Guid[] guids)
        {
            foreach (Guid id in guids) _items.Add(id, default);
        }

        /// <inheritdoc/>
        public bool TryAddGuidStringRange(params string[] range)
        {
            Guid[] guids = new Guid[range.Length];

            for (int i = 0; i < range.Length; i++)
            {
                if (!Guid.TryParse(range[i], out guids[i])) return false;
            }

            AddGuidRange(guids);
            return true;
        }

        /// <inheritdoc/>
        public void AddOrCreate(Guid id, T item)
        {
            _items[id] = item;
        }

        /// <inheritdoc/>
        public void Populate(Dictionary<Guid, T> map)
        {
            List<(Guid, T)> tuples = new List<(Guid, T)>();

            foreach(Guid g in _items.Keys)
            {
                if(map.TryGetValue(g, out T val))
                {
                    tuples.Add((g, val));
                }
            }

            foreach((Guid, T) t in tuples)
            {
                _items[t.Item1] = t.Item2;
            }
        }

        /// <inheritdoc/>
        public T RemoveByGuid(Guid id)
        {
            T val = _items[id];
            _items.Remove(id);
            return val;
        }

        /// <inheritdoc/>
        public Guid RemoveByValue(T value)
        {
            Guid id = default;

            foreach(KeyValuePair<Guid, T> kv in _items)
            {
                if (kv.Value.Equals(value)) id = kv.Key; 
            }

            _items.Remove(id);
            return id;
        }
    }
}