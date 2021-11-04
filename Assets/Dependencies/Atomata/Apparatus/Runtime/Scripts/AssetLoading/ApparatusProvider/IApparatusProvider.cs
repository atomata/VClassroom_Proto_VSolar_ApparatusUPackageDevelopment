using Cysharp.Threading.Tasks;

using HexUN.Framework.Debugging;

using UnityEngine;


namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// A prefab provider is able to load a apparatus from a key. The provider
    /// should handle all caching mechanisms and completely abstract the origin
    /// of the apparatus from the consumer. 
    /// </summary>
    public interface IApparatusProvider
    {
        /// <summary>
        /// Provides an apparatus based on the key. If the apparatus cannot be found,
        /// return null;
        /// </summary>
        UniTask<SrApparatus> Provide(string key, LogWriter logWriter);
    }
}