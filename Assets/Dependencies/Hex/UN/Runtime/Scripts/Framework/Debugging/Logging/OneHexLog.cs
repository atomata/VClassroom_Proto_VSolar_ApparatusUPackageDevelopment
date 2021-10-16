using HexUN.Behaviour;

using System;
using System.Text;

using UnityEngine;

namespace HexUN.Framework.Debugging
{
    /// <summary>
    /// Logs to the unity logger and to a shared resource object if avalable
    /// </summary>
    public class OneHexLog : AOneHexPersistent<OneHexLog>, ILog
    {
        private const string cUnknownCategory = "Unknown";

        private const string cOpenBrack = "[";
        private const string cCloseBrack = "]";
        private const string cSep = " - ";

        private Action<string> _logInfoAction = Debug.Log;
        private Action<string> _logWarningAction = Debug.LogWarning;
        private Action<string> _logErrorAction = Debug.LogError;

        [SerializeField]
        [Tooltip("The logs shared resource")]
        private ReLogs _logs;

        /// <inheritdoc />
        public Action<string> LogInfoAction { get => _logInfoAction; set => _logInfoAction = value; }

        /// <inheritdoc />
        public Action<string> LogWarnAction { get => _logWarningAction; set => _logWarningAction = value; }

        /// <inheritdoc />
        public Action<string> LogErrorAction { get => _logErrorAction; set => _logErrorAction = value; }

        private StringBuilder _sb = new StringBuilder();

        #region API
        /// <summary>
        /// User facing info with unknown category
        /// </summary>
        public void SimpleInfo(string message) => Info(cUnknownCategory, message, true);

        /// <inheritdoc />
        public void Info(string category, string message, bool forUser = false)
            => PerformLog(
                ELogSeverity.Info,
                category,
                message,
                forUser, 
                LogInfoAction
            );

        /// <summary>
        /// User facing warn with unknown category
        /// </summary>
        public void SimpleWarn(string message) => Warn(cUnknownCategory, message, true);

        /// <inheritdoc />
        public void Warn(string category, string message, bool forUser = false)
            => PerformLog(
                ELogSeverity.Warning,
                category,
                message,
                forUser,
                LogWarnAction
            );

        /// <summary>
        /// User facing error with unknown category and no execption
        /// </summary>
        public void SimpleError(string message) => Error(cUnknownCategory, message, true);

        /// <inheritdoc />
        public void Error(string category, string message, bool forUser = false)
            => PerformLog(
                ELogSeverity.Error,
                category,
                message,
                forUser,
                LogErrorAction
            );

        /// <inheritdoc />
        public void Error(string category, string message, Exception e, bool forUser = false)
            => PerformLog(
                ELogSeverity.Error, 
                category, 
                WriteLog(category, $"{message}\n Exception: {e.Message}\nStack Trace:\n {e.StackTrace}"), 
                forUser,
                LogErrorAction
            );
        #endregion

        private void PerformLog(ELogSeverity severity, string category, string message, bool isUser, Action<string> logAction)
        {
            logAction(WriteLog(category, message));
            PushLog(severity, category, message, isUser);
        }

        private string WriteLog(string category, string message)
        {
            _sb.Clear();
            _sb.Append(cOpenBrack);
            _sb.Append(category);
            _sb.Append(cCloseBrack);
            _sb.Append(cSep);
            _sb.AppendLine(message);
            return _sb.ToString();
        }

        private void PushLog(ELogSeverity severity, string category, string log, bool isUser)
        {
            if(_logs != null)
            {
                _logs.AddLog( new SrLog(severity, category, log, isUser) );
                _logs.PushUpdate();
            }
        }
    }
}
