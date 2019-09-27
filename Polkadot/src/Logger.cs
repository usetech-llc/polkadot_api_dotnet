namespace Polkadot
{
    using NLog;

    public class Logger : ILogger
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public Logger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt", DeleteOldFileOnStartup = true };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Info(string message)
        {
            if (message.Length > 200)
            {
                _logger.Info($"{message.Substring(0, 150)}.....{message.Substring(message.Length - 50)}");
            }
            else
            {
                _logger.Info(message);
            }
        }

        public void Warning(string message)
        {
            _logger.Warn(message);
        }
    }
}