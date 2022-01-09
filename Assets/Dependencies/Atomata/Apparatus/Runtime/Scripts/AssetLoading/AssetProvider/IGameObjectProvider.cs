using Cysharp.Threading.Tasks;

using HexUN.Framework.Debugging;

using UnityEngine;


namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// A prefab provider is able to load a prefab from a key. The provider
    /// should handle all caching mechanisms and completely abstract the origin
    /// of the prefab from the consumer. 
    /// </summary>
    public interface IGameObjectProvider
    {
        /// <summary>
        /// Provides a prefab based on the key. If the prefab cannot be found,
        /// return null;
        /// </summary>
        UniTask<GameObject> Provide(string key, LogWriter log);
    }
}