using System;
using System.IO;
using System.Diagnostics;

namespace Logging
{
    public enum EventTyp
    {
        Error,
        Warning,
        Information,
        Debug,
        Null
    }
    public static class EventLog
    {
        private static readonly string _FileName = $@"C:\steam\log.log";
        public static void WriteEventLog(EventTyp typ, string msg)
        {
#if DEBUG
            Debug.WriteLine(CreateLogEntry(typ, msg));
#else
            try
            {
                using FileStream aFile = new FileStream(_FileName, FileMode.Append, FileAccess.Write);
                using StreamWriter sw = new StreamWriter(aFile);
                sw.WriteLine(CreateLogEntry(typ, msg));
            }
            catch (Exception)
            {

            }
#endif
        }

        private static string CreateLogEntry(EventTyp typ, string msg)
        {
            return DateTime.Now.ToShortDateString() + $@" " + DateTime.Now.ToShortTimeString() + $@" :: " + typ + $@" :: " + msg;
        }
    }
}
