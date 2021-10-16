using System;
using System.Collections.Generic;

namespace HexUN.Data
{
    /// <summary>
    /// Contains Guids and can have guids added
    /// </summary>
    public interface IGuidCollection
    {
        /// <summary>
        /// All guids
        /// </summary>
        Guid[] Guids { get; }

        /// <summary>
        /// Add guids with default values
        /// </summary>
        /// <param name="guids"></param>
        void AddGuidRange(params Guid[] guids);

        /// <summary>
        /// Try to add a guid range based on string guids.
        /// Returns false if a string guid fails to parse
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        bool TryAddGuidStringRange(params string[] range);
    }

    /// <summary>
    /// Maps guids to objects. Guids can be added without
    /// an object reference being linked, so that linking can occur later. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGuidCollection<T> : IGuidCollection
    {
        /// <summary>
        /// All elements
        /// </summary>
        T[] Values { get; }

        /// <summary>
        /// Add or create a guid item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        void AddOrCreate(Guid id, T item);

        /// <summary>
        /// Iterate over all stored guids and populate with items found in map
        /// </summary>
        /// <param name="map"></param>
        void Populate(Dictionary<Guid, T> map);

        /// <summary>
        /// Returns item at removed guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T RemoveByGuid(Guid id);

        /// <summary>
        /// Returns guid at removed item
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Guid RemoveByValue(T value);
    }
}
