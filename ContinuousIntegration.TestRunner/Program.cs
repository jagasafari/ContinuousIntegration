namespace ContinuousIntegration.TestRunner
{
    using System;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.DependencyInjection;

    public class Program
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicatinLogger _logger;

        public Program(IApplicationEnvironment env)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddLogging()
                .AddScoped<ApplicationConfiguration>()
                .AddScoped<CiConfigurationReader>()
                .AddScoped<Mailer>()
                .AddScoped<TestRunner>()
                .AddScoped<ApplicatinLogger>()
                .AddInstance(env);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _logger = _serviceProvider.GetService<ApplicatinLogger>();
            _logger.Info("Test Runner Initialized!");
        }

        public void Main(string[] args)
        {
            try
            {
                _serviceProvider.GetService<TestRunner>().Run();
            }
            catch(Exception e)
            {
                _logger.Error($"Excepion cought : {Environment.NewLine}" +
                              $"{e}");
                var mailer = _serviceProvider.GetService<Mailer>();
                var mailMessage = mailer.CreateMessageEmail(e.StackTrace,"Error");
                mailer.SendReportEmail(mailMessage);
            }
        }
    }
}