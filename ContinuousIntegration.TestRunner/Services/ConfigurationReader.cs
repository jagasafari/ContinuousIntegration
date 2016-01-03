namespace ContinuousIntegration.TestRunner.Services
{
    using System;
    using Common.Core;
    using Common.Mailer.Model;
    using ContinuousIntegration.TestRunner.Abstraction;
    using ContinuousIntegration.TestRunner.Model;
    using Microsoft.Extensions.Configuration;

    public class ConfigurationReader : IConfigurationReader
    {
        private readonly IConfiguration _configuration;

        public ConfigurationReader(ApplicationConfiguration configuration)
        {
            _configuration = configuration.Configuration;
        }

        public TestConfiguration TestConfiguration => 
            Check.NotNull<TestConfiguration>(GetTestConfiguration());

        public MailConfiguration MailConfiguration => 
            Check.NotNull<MailConfiguration>(GetMailConfiguration());
            
        private TestConfiguration GetTestConfiguration()
        {
            var testConfiguration = new TestConfiguration
            {
                MinutesToWait = new TimeSpan(0, int.Parse(_configuration["Timeing:MinutesToWait"]), 0)
            };
            var count = 0;
            var next = GetTestProject(count++);
            while(next != null)
            {
                testConfiguration.TestProjects.Add(next);
                next = GetTestProject(count++);
            }
            return testConfiguration;
        }

        private string GetTestProject(int count) => 
            _configuration[$"Paths:TestProjects:{count}"];
            
        private MailConfiguration GetMailConfiguration() =>
            new MailConfiguration
            {
                Sender =
                    _configuration[
                        GetKey(nameof(MailConfiguration.Sender))],
                Password =
                    _configuration[
                        GetKey(nameof(MailConfiguration.Password))],
                SmtpPort =
                    int.Parse(
                        _configuration[
                            GetKey(nameof(MailConfiguration.SmtpPort))]),
                SmtpHost =
                    _configuration[
                        GetKey(nameof(MailConfiguration.SmtpHost))],
                Receiver =
                    _configuration[
                        GetKey(nameof(MailConfiguration.Receiver))]
            };

        private static string GetKey(string property) => $"EMail:{property}";
    }
}