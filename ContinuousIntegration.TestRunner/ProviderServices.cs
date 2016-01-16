namespace ContinuousIntegration.TestRunner
{
    using System;
    using Common.Mailer;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;
    using Microsoft.Extensions.Configuration;

    using Microsoft.Extensions.OptionsModel;
    using ContinuousIntegration.TestRunner.Services;

    using ContinuousIntegration.TestRunner.Abstraction;
    using Common.Mailer.Model;
    using ContinuousIntegration.TestRunner.Model;

    public class ProviderServices
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderServices(IApplicationEnvironment env)
        {
            var configuration = new ConfigurationBuilder()
                .BuildConfiguration(env);
                
            _serviceProvider = new ServiceCollection()
            .AddLogging()
            
            .AddOptions()
            .Configure<MailConfiguration>(configuration)
            .Configure<TestOptions>(configuration)
            
            .AddSingleton<TestConfiguration>()
            .AddInstance(configuration)
            .AddProceesProviderServices()
            
            .AddTransient<IModifiedCodeTestsFinder, ModifiedCodeTestsFinder>()
            .AddTransient<IDnxTestRunner, DnxTestRunner>()
            .AddTransient<ITestRunner, TestRunner>()
            .AddTransient<IMailServiceFactory, MailServiceFactory>()

            .AddInstance(env)

            .BuildServiceProvider();

            _serviceProvider.GetService<ILoggerFactory>().AddConsole(LogLevel.Information);
        }
        
        public ILogger<Program> ProgramLogger => _serviceProvider.GetService<ILogger<Program>>();
        
        private MailConfiguration MailConfiguration => _serviceProvider.GetService<IOptions<MailConfiguration>>().Value;

        public IMailService MailService => _serviceProvider.GetService<IMailServiceFactory>().Create();

        public ITestRunner TestRunner => _serviceProvider.GetService<ITestRunner>();

    }
}