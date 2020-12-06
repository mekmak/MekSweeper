using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace MekSweeper.Logging
{
    public class Log4NetLogger
    {
        private static readonly ILog _log;
        static Log4NetLogger()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %message%newline"
            };

            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender
            {
                AppendToFile = false,
                File = @"Logs\MekSweeperLogs.txt",
                Layout = patternLayout,
                MaxSizeRollBackups = 5,
                MaximumFileSize = "1GB",
                RollingStyle = RollingFileAppender.RollingMode.Size,
                StaticLogFileName = true
            };

            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;

            _log = LogManager.GetLogger("MekSweeper");
        }

        public static void Trace(string message)
        {
            _log.Debug(message);
        }

        public static void Info(string message)
        {
            _log.Info(message);
        }

        public static void Warn(string message)
        {
            _log.Warn(message);
        }

        public static void Error(string message)
        {
            _log.Error(message);
        }

        public static bool IsTraceEnabled => _log.IsDebugEnabled;
    }
}
