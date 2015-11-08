namespace ContinuousIntegration.TestRunner
{
    using Microsoft.Framework.Logging;

    public class ApplicatinLogger 
    {
        private readonly ILogger _logger;

        public ApplicatinLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.
                AddConsole(LogLevel.Debug).CreateLogger("Ci");
        }


        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Error(string message)
        {
            _logger.Info(message);
        }
    }
}