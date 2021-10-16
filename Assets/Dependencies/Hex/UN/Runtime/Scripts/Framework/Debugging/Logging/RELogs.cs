using HexUN.Framework.SharedResource;

using System.Collections.Generic;

using UnityEngine;

namespace HexUN.Framework.Debugging
{
    /// <summary>
    /// Shared resource for caching logs
    /// </summary>
    [CreateAssetMenu(fileName = "RELogs", menuName = "HexUN/Framework/SharedResources/Logs")]
    public class ReLogs : ASOSharedResource
    {
        [SerializeField]
        [Tooltip("The maximum number of logs that will be stored")]
        private int _max;

        private Queue<SrLog> _logs = new Queue<SrLog>();

        #region API
        public IEnumerable<SrLog> Logs => _logs;

        /// <summary>
        /// Adds a log to the logs
        /// </summary>
        public void AddLog(SrLog log)
        {
            _logs.Enqueue(log);

            if(_logs.Count > _max)
            {
                _logs.Dequeue();
            }
        }

        public override void Clear()
        {
            _logs.Clear();
        }
        #endregion
    }
}