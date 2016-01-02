namespace ContinuousIntegration.TestRunner
{
    using System;
    using Common.Mailer;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;

    public class ProviderServices
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderServices(IApplicationEnvironment env)
        {
            _serviceProvider = new ServiceCollection()
            .AddLogging()

            .AddSingleton<ApplicationConfiguration>()
            .AddTransient<ModelProvider>()

            .AddTransient<ProcessProviderServices>()
            .AddTransient<ConfigurationReader>()
            .AddTransient<FileFinder>()
            .AddTransient<DnxTestRunner>()

            .AddInstance(env)

            .BuildServiceProvider();

            _serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Information);
        }
        
        public ILogger<TestRunner> TestRunnerLogger => _serviceProvider.GetService<ILogger<TestRunner>>();
        public ILogger<Program> ProgramLogger => _serviceProvider.GetService<ILogger<Program>>();

        public ModelProvider ModelProvider => _serviceProvider.GetService<ModelProvider>();

        public Mailer Mailer => new Mailer(ModelProvider.MailConfiguration,
            eventSource => $"CI {eventSource} {DateTime.UtcNow}");

        public FileFinder ModifiedFileFinder => _serviceProvider.GetService<FileFinder>();

        public DnxTestRunner DnxTestRunner => _serviceProvider.GetService<DnxTestRunner>();

    }
}