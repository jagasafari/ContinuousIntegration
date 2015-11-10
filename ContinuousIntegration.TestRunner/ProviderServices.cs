namespace ContinuousIntegration.TestRunner
{
    using System;
    using Logging;
    using Mailer;
    using Microsoft.Framework.Logging;

    public class ProviderServices
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public LoggerFactory LoggerFactory
            =>
                ( (LoggerFactory)
                    _serviceProvider.GetService(typeof(LoggerFactory)) );

        public ApplicationLogger ApplicationLogger => new ApplicationLogger(LoggerFactory, "Ci");

        public ProviderModels ProviderModels =>
            ( (ProviderModels)
                _serviceProvider.GetService(typeof(ProviderModels)) );

        public TestRunner TestRunner => new TestRunner(ProviderModels.TestConfiguration);

        public Mailer Mailer => new Mailer(ProviderModels.MailConfiguration, 
            eventSource => $"CI {eventSource} {DateTime.UtcNow}");

        public ModifiedFileFinder ModifiedFileFinder
            =>
                ( (ModifiedFileFinder)
                    _serviceProvider.GetService(typeof(ModifiedFileFinder)) );

    }
}