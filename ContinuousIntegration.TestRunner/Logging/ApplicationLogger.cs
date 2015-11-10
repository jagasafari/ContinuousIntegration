namespace ContinuousIntegration.TestRunner.Logging
{
    using Microsoft.Framework.Logging;

    public class ApplicationLogger 
    {
        private readonly ILogger _logger;

        public ApplicationLogger(ILoggerFactory loggerFactory, string name)
        {
            _logger = loggerFactory.
                AddConsole(LogLevel.Debug).CreateLogger(name);
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