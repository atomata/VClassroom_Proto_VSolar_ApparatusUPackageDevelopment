using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexCS.Core;
using UnityEngine;

namespace Atomata.VSolar.Apparatus
{
    [Serializable]
    public class UiMeta
    {
        public string UiName;
        public string UiDesc;
        public bool UiEnabled;

        public string AsArgs()
        {
            Dictionary<string, string> kvs = new Dictionary<string, string>()
            {
                {nameof(UiName).ToLower(), UiName},
                {nameof(UiDesc).ToLower(), UiDesc},
                {nameof(UiEnabled).ToLower(), UiEnabled.ToString()}
            };

            if (!kvs.QueryContains(kv => !string.IsNullOrEmpty(kv.Value)))
                return string.Empty;
            
            StringBuilder sb = new StringBuilder();
            sb.Append("?");

            bool isFirst = true;            
            foreach (KeyValuePair<string,string> kv in kvs)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    if (isFirst) isFirst = false;
                    else sb.Append("&");
                    sb.Append($"{kv.Key}={kv.Value}");
                }
            }

            return sb.ToString();
        }
    }
}