using System;
using UnityEngine;

namespace HexUN.Serialization
{
    /// <summary>
    /// Static utilities for using UNities builtin json serialization
    /// </summary>
    public static class UTJsonUtility
    {
        /// <summary>
        /// Attempt to deserailize string into json, if fails returns false
        /// </summary>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="json"></param>
        /// <param name="deserialized"></param>
        /// <returns></returns>
        public static bool TryDeserialize<TObj>(string json, out TObj deserialized)
        {
            try
            {
                deserialized  = JsonUtility.FromJson<TObj>(json);
            }
            catch (Exception)
            {
                deserialized = default;
                return false;
            }

            return true;
        }

    }
}
