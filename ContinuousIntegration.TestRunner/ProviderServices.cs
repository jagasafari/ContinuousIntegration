namespace ContinuousIntegration.TestRunner
{
    using System;
    using Common.Mailer;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution;
    using Common.Core;

    public class ProviderServices
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILoggerFactory LoggerFactory
            =>
                Check.NotNull<ILoggerFactory>((ILoggerFactory)
                    _serviceProvider.GetService(typeof(ILoggerFactory)));

        public ILogger Logger(string name) => Check.NotNull<ILogger>(LoggerFactory
            .CreateLogger($"{name}"));

        public ModelProvider ModelProvider =>
            Check.NotNull<ModelProvider>((ModelProvider)
                _serviceProvider.GetService(typeof(ModelProvider)));

        public TestRunner TestRunner => new TestRunner(ProcessProviderServices,
            this);

        public Mailer Mailer => new Mailer(ModelProvider.MailConfiguration,
            eventSource => $"CI {eventSource} {DateTime.UtcNow}");

        public FileFinder ModifiedFileFinder
            =>
                ((FileFinder)
                    _serviceProvider.GetService(typeof(FileFinder)));

        private ProcessProviderServices ProcessProviderServices =>
            (ProcessProviderServices)_serviceProvider.GetService(typeof(ProcessProviderServices));

        public DnxTestRunner DnxTestRunner =>
            new DnxTestRunner(this, ProcessProviderServices);
            
        public ConfigurationReader ConfigurationReader =>
            Check.NotNull<ConfigurationReader>((ConfigurationReader)_serviceProvider.GetService(typeof(ConfigurationReader)));
    }
}