using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexUN.Framework.Debugging
{
    [Serializable]
    public class SrLog
    {
        private ELogSeverity _severity;
        private string _category;
        private string _message;
        private bool _isUserLog;

        #region API
        /// <summary>
        /// Severity of log
        /// </summary>
        public ELogSeverity Severity => _severity;
        
        /// <summary>
        /// Category of log, normally shows logging class name
        /// </summary>
        public string Category => _category;

        /// <summary>
        /// The message the log provided
        /// </summary>
        public string Message => _message;

        /// <summary>
        /// Is this a user facing log
        /// </summary>
        public bool IsUserLog => _isUserLog;

        public SrLog(ELogSeverity severity, string category, string message, bool isUserLog)
        {
            _severity = severity;
            _category = category;
            _message = message;
            _isUserLog = isUserLog;
        }
        #endregion
    }
}