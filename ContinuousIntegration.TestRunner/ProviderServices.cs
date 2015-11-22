namespace ContinuousIntegration.TestRunner
{
    using System;
    using Mailer;
    using Microsoft.Extensions.Logging;
    using ProcessExecution;

    public class ProviderServices
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILoggerFactory LoggerFactory
            =>
                ((ILoggerFactory)
                    _serviceProvider.GetService(typeof(ILoggerFactory)));

        public ILogger Logger(string name) => LoggerFactory
            .CreateLogger($"{name}: [{DateTime.UtcNow}]");

        public ProviderModels ProviderModels =>
            ((ProviderModels)
                _serviceProvider.GetService(typeof(ProviderModels)));

        public TestRunner TestRunner => new TestRunner(ProcessProviderServices,
            this);

        public Mailer Mailer => new Mailer(ProviderModels.MailConfiguration,
            eventSource => $"CI {eventSource} {DateTime.UtcNow}");

        public FileFinder ModifiedFileFinder
            =>
                ((FileFinder)
                    _serviceProvider.GetService(typeof(FileFinder)));

        private ProcessProviderServices ProcessProviderServices =>
            (ProcessProviderServices)_serviceProvider.GetService(typeof(ProcessProviderServices));

        public DnxTestRunner DnxTestRunner =>
            new DnxTestRunner(this, ProcessProviderServices);
    }
}