using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Halloumi.Common.Helpers;

namespace Halloumi.Shuffler
{
    /// <summary>
    ///     Records how long each step of starting or closing the application takes, and writes them to a log in
    ///     %APPDATA%\Halloumi Shuffler\ (Halloumi.Shuffler.Startup.log and Halloumi.Shuffler.Shutdown.log).
    /// </summary>
    internal sealed class StepTimer
    {
        public static readonly StepTimer Startup = new StepTimer("Start-up", "Halloumi.Shuffler.Startup.log");

        /// <summary>
        ///     Starts timing when the main window begins closing.
        /// </summary>
        public static readonly StepTimer Shutdown = new StepTimer("Shutdown", "Halloumi.Shuffler.Shutdown.log");

        private readonly string _title;
        private readonly string _filename;
        private readonly StringBuilder _log = new StringBuilder();
        private Stopwatch _stopwatch;
        private long _last;

        private StepTimer(string title, string filename)
        {
            _title = title;
            _filename = filename;
        }

        public void Mark(string step)
        {
            lock (_log)
            {
                if (_stopwatch == null) _stopwatch = Stopwatch.StartNew();
                var now = _stopwatch.ElapsedMilliseconds;
                _log.AppendLine($"{now,8:N0} ms  {now - _last,8:N0} ms  {step}");
                _last = now;
            }
        }

        public void Write(string lastStep)
        {
            Mark(lastStep);
            try
            {
                var path = Path.Combine(ApplicationHelper.GetUserDataPath(), _filename);
                lock (_log)
                {
                    File.WriteAllText(path, $"{_title} {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n   total      step\r\n" + _log);
                }
            }
            catch
            {
                // timing is best effort
            }
        }
    }
}
