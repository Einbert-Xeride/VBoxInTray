using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBoxInTray
{
    class VboxLogWatcher : Logging.ILogWatcher, IDisposable
    {
        private StreamWriter sw;
        private string logPath;
        public string LogPath
        {
            get { return logPath; }
        }

        public VboxLogWatcher(VirtualBox.IMachine vm)
        {
            string logdir = vm.LogFolder;
            string filename = string.Format("VBoxInTray-{0}.log", Utils.DateTimeShortRepr(DateTime.UtcNow));
            logPath = Path.Combine(logdir, filename);
            sw = new StreamWriter(logPath, false);
        }

        public Logging.LogLevel FiltLevel
        {
            get { return Logging.LogLevel.EVERYTHING; }
        }

        public string FiltTag
        {
            get { return null; }
        }

        public void OnLog(string tag, Logging.LogLevel level, string message)
        {
            sw.WriteLine(string.Format("[{0}] {1}: [{2}] {3}",
                Utils.SecondsSinceStartUp(),
                Logging.GetLevelName(level),
                tag,
                message));
            sw.Flush();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    sw.Dispose();
                }

                sw.Close();

                disposedValue = true;
            }
        }
        
        ~VboxLogWatcher()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
