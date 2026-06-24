using System;
using System.Collections.Generic;

namespace CybersecurityChatBot
{
    public class ActivityLogger
    {
        private static List<string> _log = new List<string>();

        public static void Log(string action)
        {
            string entry = DateTime.Now.ToString("[HH:mm] ") + action;
            _log.Add(entry);
        }

        public static string GetRecentLog(int count = 10)
        {
            if (_log.Count == 0)
                return "No actions logged yet.";

            int start = _log.Count > count ? _log.Count - count : 0;
            var recent = _log.GetRange(start, _log.Count - start);

            string result = "Here's a summary of recent actions:\n";
            for (int i = 0; i < recent.Count; i++)
                result += $"  {i + 1}. {recent[i]}\n";

            return result;
        }
        // Counts the  number of actions logged and returns a summary of all actions
        public static string GetFullLog()
        {
            if (_log.Count == 0)
                return "No actions logged yet.";

            string result = "Full activity log:\n";
            for (int i = 0; i < _log.Count; i++)
                result += $"  {i + 1}. {_log[i]}\n";

            return result;
        }

        public static int GetCount() => _log.Count;
    }
}