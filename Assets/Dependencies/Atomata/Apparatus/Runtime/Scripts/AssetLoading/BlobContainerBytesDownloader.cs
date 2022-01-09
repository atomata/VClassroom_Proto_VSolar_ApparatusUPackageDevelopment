using Cysharp.Threading.Tasks;

using HexCS.Core;

using HexUN.Data;
using HexUN.Framework;
using HexUN.Framework.Debugging;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Atomata.VSolar.Apparatus
{
    /// <summary>
    /// Downloads the bytes in a blob container based on a key
    /// </summary>
    public class BlobContainerBytesDownloader
    {
        private const string cLogCategory = nameof(BlobContainerBytesDownloader);
        private string _containerURL;

        public BlobContainerBytesDownloader(string containerURL)
            => _containerURL = containerURL;

        public async UniTask<byte[]> Provide(string filename, LogWriter writer)
        {
            string url = _containerURL + $"/{filename}";
            writer.AddInfo(cLogCategory, cLogCategory, $"Getting bytes from blob container:{url}");
            
            UnityWebRequest req = UnityWebRequest.Get(url);
            await req.SendWebRequest();
            
            // TODO: Remove if WebGL isn't angry about the awaiter above
            // UnityWebRequestAsyncOperation op = req.SendWebRequest();
            //
            // while (!op.isDone)
            // {
            //     await UniTask.Delay(100);
            // }

            if (req.responseCode != 200)
            {
                writer.AddInfo(cLogCategory, cLogCategory, $"Failed with status code {req.responseCode}");
                return null;
            }

            return req.downloadHandler.data;
        }
    }
}