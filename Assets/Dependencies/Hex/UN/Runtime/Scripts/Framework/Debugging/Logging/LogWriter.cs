using System;
using System.Collections.Generic;
using System.Text;

namespace HexUN.Framework.Debugging
{
    public class LogWriter
    {
        private List<string> _logs = new List<string>();
        private string _title;
        private string _lastIdentity = null;

        public LogWriter(string title)
        {
            _title = title;
        }

        public void AddError(string category, string identity, string log) => LogAndKeyword("ERROR", category, identity, log);
        public void AddWarning(string category, string identity, string log) => LogAndKeyword("WARNING", category, identity, log);
        public void AddInfo(string category, string identity, string log) => LogAndKeyword("INFO", category, identity, log);

        public string GetLog()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("\n**********");
            sb.AppendLine($"Timestamp: {DateTime.Now.ToShortDateString()} : {DateTime.Now.ToLongTimeString()}");
            sb.AppendLine(_title);
            sb.AppendLine("----------");
            foreach (string line in _logs) sb.AppendLine(line);
            sb.AppendLine("----------");

            return sb.ToString();
        }

        private void LogAndKeyword(string keyword, string category, string identity, string log)
        {
            if (identity != _lastIdentity) _logs.Add(identity);
            _logs.Add($"  [{keyword}] [{category}] {log}");
            _lastIdentity = identity;
        }
    }
}