using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBoxInTray
{
    sealed class Logging
    {
        #region Implements Singleton pattern

        private static readonly Logging instance = new Logging();

        private Logging() { }

        public static Logging Instance
        {
            get { return instance; }
        }

        #endregion

        private List<ILogWatcher> watchers = new List<ILogWatcher>();

        public void AddWatcher(ILogWatcher watcher)
        {
            watchers.Add(watcher);
        }

        public int WatcherCount
        {
            get { return watchers.Count; }
        }

        public void Log(string tag, LogLevel level, string message)
        {
            if (level == LogLevel.NOTHING) return;
            if (tag == null) tag = "";

            foreach (var watcher in watchers)
            {
                if (watcher.FiltLevel > level) continue;
                string filtTag = watcher.FiltTag;
                if (filtTag == null) filtTag = "";
                if (filtTag != "" && !filtTag.Equals(tag)) continue;
                watcher.OnLog(tag, level, message);
            }
        }

        public void Log(LogLevel level, string message)
        {
            Log(null, level, message);
        }

        public void Verbose(string message)
        {
            Log(null, LogLevel.VERBOSE, message);
        }

        public void Verbose(string tag, string message)
        {
            Log(tag, LogLevel.VERBOSE, message);
        }

        public void Info(string message)
        {
            Log(null, LogLevel.INFO, message);
        }

        public void Info(string tag, string message)
        {
            Log(tag, LogLevel.INFO, message);
        }

        public void Warning(string message)
        {
            Log(null, LogLevel.WARNING, message);
        }

        public void Warning(string tag, string message)
        {
            Log(tag, LogLevel.WARNING, message);
        }

        public void Error(string message)
        {
            Log(null, LogLevel.ERROR, message);
        }

        public void Error(string tag, string message)
        {
            Log(tag, LogLevel.ERROR, message);
        }

        public void Fatal(string message)
        {
            Log(null, LogLevel.FATAL, message);
        }

        public void Fatal(string tag, string message)
        {
            Log(tag, LogLevel.FATAL, message);
        }

        public enum LogLevel
        {
            EVERYTHING = 1, VERBOSE = 1, INFO = 2, WARNING = 3, ERROR = 4, FATAL = 5, NOTHING = 6
        }

        public static string GetLevelName(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.VERBOSE: return "VERBOSE";
                case LogLevel.INFO: return "INFO";
                case LogLevel.WARNING: return "WARNING";
                case LogLevel.ERROR: return "ERROR";
                case LogLevel.FATAL: return "FATAL";
            }
            return "UNKNOWN";
        }

        public interface ILogWatcher
        {
            string FiltTag { get; }
            LogLevel FiltLevel { get; }
            void OnLog(string tag, LogLevel level, string message);
        }
    }
}
