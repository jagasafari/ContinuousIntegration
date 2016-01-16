namespace ContinuousIntegration.TestRunner.Services
{
    using System;
    using Common.Mailer;
    using Common.Mailer.Model;
    using ContinuousIntegration.TestRunner.Abstraction;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.OptionsModel;

    public class MailServiceFactory : IMailServiceFactory{
        private MailConfiguration _configurations;
        private ILogger<MailService> _logger;
        public MailServiceFactory(IOptions<MailConfiguration> options, ILogger<MailService> logger)
        {
            _configurations = options.Value;
            _logger=logger;
        }
        
        public IMailService Create(){
            return new MailService(_configurations, eventSource => $"CI {eventSource} {DateTime.UtcNow}", _logger);
        }
    }
}