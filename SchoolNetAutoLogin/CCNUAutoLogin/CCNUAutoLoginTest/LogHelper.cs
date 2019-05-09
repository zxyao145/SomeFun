using System;
using System.IO;

namespace CCNUAutoLogin
{
    static class LogHelper
    {
        private static string LogsDir;
        static LogHelper()
        {
            var tempPath = Path.GetTempPath();
            LogsDir = Path.Combine(tempPath, "./CCNUAutoLoginLogs");
            Directory.CreateDirectory(LogsDir);
        }

        public static void WriteError(string msg)
        {
            string logPath = GetLogPath();
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine($"{DateTime.Now.ToShortTimeString()}");
                sw.Write($"Error:\t");
                sw.WriteLine(msg);
                sw.Close();
            }
        }

        public static void WriteInfo(string msg)
        {
            string logPath = GetLogPath();
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine($"{DateTime.Now.ToShortTimeString()}");
                sw.Write($"Info:\t");
                sw.WriteLine(msg);
                sw.Close();
            }
        }

        private static string GetLogPath()
        {
            var logName = DateTime.Now.ToString("yyyyMMdd")+".log";
            string logPath = Path.Combine(LogsDir, logName);
            return logPath;
        }

    }
}
