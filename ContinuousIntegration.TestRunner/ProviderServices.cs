namespace ContinuousIntegration.TestRunner
{
    using System;
    using Common.Mailer;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;
    using ContinuousIntegration.TestRunner.Services;
    using ContinuousIntegration.TestRunner.Abstraction;

    public class ProviderServices
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderServices(IApplicationEnvironment env)
        {
            _serviceProvider = new ServiceCollection()
            .AddLogging()

            .AddSingleton<ApplicationConfiguration>()
            
            .AddProceesProviderServices()
            
            .AddTransient<ModifiedCodeTestsFinder>()
            .AddTransient<DnxTestRunner>()
            .AddTransient<TestRunner>()
            .AddTransient<IMailServiceFactory, MailServiceFactory>()
            .AddTransient<IConfigurationReader, ConfigurationReader>()

            .AddInstance(env)

            .BuildServiceProvider();

            _serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Information);
        }
        
        public ILogger<Program> ProgramLogger => _serviceProvider.GetService<ILogger<Program>>();

        public IMailService MailService => _serviceProvider.GetService<IMailServiceFactory>().Create();

        public TestRunner TestRunner => _serviceProvider.GetService<TestRunner>();

    }
}