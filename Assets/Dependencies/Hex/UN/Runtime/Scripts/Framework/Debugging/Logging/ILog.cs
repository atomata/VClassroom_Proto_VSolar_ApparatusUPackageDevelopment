using System;

namespace HexUN.Framework.Debugging
{
    /// <summary>
    /// Capable of logging messages
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Function used to log info. Can be overriden. Exposed so that testing could do LogAsserts
        /// </summary>
        public Action<string> LogInfoAction { get; set; }

        /// <summary>
        /// Function used to log warning. Can be overriden. Exposed so that testing could do LogAsserts
        /// </summary>
        public Action<string> LogWarnAction { get; set; }

        /// <summary>
        /// Function used to log error. Can be overriden. Exposed so that testing could do LogAsserts
        /// </summary>
        public Action<string> LogErrorAction { get; set; }

        /// <summary>
        /// Logs non-critical info
        /// </summary>
        void Info(string category, string message, bool forUser = false);

        /// <summary>
        /// Logs a warning that may need paying attention to
        /// </summary>
        void Warn(string category, string message, bool forUser = false);

        /// <summary>
        /// Logs errors in the applicaiton that break functionality
        /// </summary>
        void Error(string category, string message, bool forUser = false);

        /// <summary>
        /// Logs errors in the applicaiton that break functionality. Appends standard exception message format at end
        /// </summary>
        void Error(string category, string message, Exception e, bool forUser = false);
    }
}