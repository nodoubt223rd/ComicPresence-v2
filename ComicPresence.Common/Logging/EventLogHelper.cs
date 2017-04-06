using System.Diagnostics;

namespace ComicPresence.Common.Logging
{
    internal static class EventLogHelper
    {
        internal static void LogEvent(string message)
        {
            const string cSource = "Application Error";
            const string cLog = "Application";

            if (!EventLog.SourceExists(cSource))
                EventLog.CreateEventSource(cSource, cLog);

            EventLog.WriteEntry(cSource, message, EventLogEntryType.Error);
        }
    }
}
