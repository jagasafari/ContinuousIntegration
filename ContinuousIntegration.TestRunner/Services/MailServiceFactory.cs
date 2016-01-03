namespace ContinuousIntegration.TestRunner.Services
{
    using System;
    using Common.Mailer;
    using ContinuousIntegration.TestRunner.Abstraction;

    public class MailServiceFactory : IMailServiceFactory{
        private readonly IConfigurationReader _configurationReader;
        public MailServiceFactory(IConfigurationReader configurationReader)
        {
            _configurationReader= configurationReader;
        }
        
        public IMailService Create(){
            return new MailService(_configurationReader.MailConfiguration, eventSource => $"CI {eventSource} {DateTime.UtcNow}");
        }
    }
}