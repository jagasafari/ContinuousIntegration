namespace ContinuousIntegration.TestRunner
{
    using System;
    using Logging;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.DependencyInjection;
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
                .AddScoped<ModifiedFileFinder>()

                .AddInstance(env)

                .BuildServiceProvider()
                .GetService<ProviderServices>();

            _providerServices.ApplicationLogger
                .Info("Test Runner Initialized!");
        }

        public void Main(string[] args)
        {
            try
            {
                _providerServices.TestRunner
                    .Run();
            }
            catch(Exception e)
            {
                _providerServices.ApplicationLogger
                    .Error($"Excepion cought : {Environment.NewLine} {e}");

                _providerServices.Mailer
                    .BuildMailMessage("Error",
                    e.StackTrace).Send();
            }
        }
    }
}