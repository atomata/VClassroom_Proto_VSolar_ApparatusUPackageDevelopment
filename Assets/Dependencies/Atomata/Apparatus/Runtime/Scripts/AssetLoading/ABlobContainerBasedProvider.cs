using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using HexUN.Framework.Debugging;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    public abstract class ABlobContainerBasedProvider<TProvided>
    {
        private const string cLogCategory = nameof(ABlobContainerBasedProvider<TProvided>);

        private BlobContainerBytesDownloader _downloader;
        private Dictionary<int, TProvided> _cache = new Dictionary<int, TProvided>();

        public ABlobContainerBasedProvider(string containerURL)
            => _downloader = new BlobContainerBytesDownloader(containerURL);

        protected abstract TProvided Convert(byte[] bytes);
        
        public async UniTask<TProvided> Provide(string key, LogWriter writer)
        {
            writer.AddInfo(cLogCategory, cLogCategory, $"Loading: {key}");
            string file_name = key;
            int hashKey = file_name.GetHashCode();

            if (_cache.ContainsKey(hashKey))
            {
                writer.AddInfo(cLogCategory, cLogCategory, "Found in cache. Returning");
                return _cache[hashKey];
            }

            writer.AddInfo(cLogCategory, cLogCategory, "Could not find in cache. Trying from blob.");
            byte[] data = await _downloader.Provide(file_name, writer);

            TProvided toProvide = Convert(data);
            
            if (toProvide != null)
            {
                _cache.Add(hashKey, toProvide);
                return toProvide;
            }

            writer.AddError(cLogCategory, cLogCategory, "Could not find apparatus");
            return default;
        }
    }
}