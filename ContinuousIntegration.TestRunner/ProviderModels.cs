namespace ContinuousIntegration.TestRunner
{
    using System;
    using Mailer.Model;
    using Model;

    public class ProviderModels
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderModels(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TestConfiguration TestConfiguration => 
            ((ConfigurationReader)_serviceProvider.GetService(typeof(ConfigurationReader)))
            .GetTestConfiguration();

        public MailConfiguration MailConfiguration => 
            ((ConfigurationReader)_serviceProvider.GetService(typeof(ConfigurationReader)))
            .GetMailConfiguration();
    }
}