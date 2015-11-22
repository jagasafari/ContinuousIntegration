namespace ContinuousIntegration.TestRunner
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;
    using ProcessExecution;

    public class Program
    {
        private readonly ProviderServices _providerServices;

        public Program(IApplicationEnvironment env)
        {
            _providerServices = new ServiceCollection()
                .AddLogging()

                .AddSingleton<ApplicationConfiguration>()
                .AddSingleton<ProviderServices>()
                .AddSingleton<ProviderModels>()

                .AddScoped<ProcessProviderServices>()
                .AddScoped<ConfigurationReader>()
                .AddScoped<FileFinder>()

                .AddInstance(env)

                .BuildServiceProvider()
                .GetService<ProviderServices>();

           _providerServices.LoggerFactory
                .AddConsole(LogLevel.Information);
                
                _providerServices
                .Logger("TestRunner")
                .LogInformation("Test Runner Initialized!");
        }

        public void Main(string[] args)
        {
            try
            {
                _providerServices.TestRunner
                    .Run();
            }
            catch (Exception e)
            {
                _providerServices
                    .Logger(nameof(Program))
                    .LogError($"Excepion cought : {Environment.NewLine} {e}");

                _providerServices.Mailer
                    .BuildMailMessage("Error",
                    e.StackTrace).Send();
            }
        }
    }
}