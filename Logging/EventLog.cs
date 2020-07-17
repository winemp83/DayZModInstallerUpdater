using System;
using System.IO;

namespace Logging
{
    public enum EventTyp
    {
        Error,
        Warning,
        Information,
        DEbug,
        Null
    }
    public static class EventLog
    {
        private static readonly string _FileName = $@"C:\steam\log.log";
        public static void WriteEventLog(EventTyp typ, string msg)
        {
            try
            {
                using FileStream aFile = new FileStream(_FileName, FileMode.Append, FileAccess.Write);
                using StreamWriter sw = new StreamWriter(aFile);
                sw.WriteLine(CreateLogEntry(typ, msg));
            }
            catch (Exception)
            {

            }
        }

        private static string CreateLogEntry(EventTyp typ, string msg)
        {
            return DateTime.Now.ToShortDateString() + $@" " + DateTime.Now.ToShortTimeString() + $@" :: " + typ + $@" :: " + msg;
        }
    }
}
