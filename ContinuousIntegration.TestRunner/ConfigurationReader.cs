namespace ContinuousIntegration.TestRunner
{
    using System;
    using Common.Mailer.Model;
    using Microsoft.Extensions.Configuration;
    using Model;

    public class ConfigurationReader
    {
        private readonly IConfiguration _configuration;

        public ConfigurationReader(ApplicationConfiguration configuration)
        {
            _configuration = configuration.Configuration;
        }

        public string ApplicationBasePath { get; set; }

        public TestConfiguration GetTestConfiguration()
        {
            var ciTestConfiguration = new TestConfiguration
            {
                MinutesToWait = new TimeSpan(0,
                    int.Parse(_configuration["Timeing:MinutesToWait"]), 0)
            };
            var count = 0;
            var next = GetTestProject(count++);
            while(next != null)
            {
                ciTestConfiguration.TestProjects.Add(next);
                next = GetTestProject(count++);
            }
            return ciTestConfiguration;
        }

        public MailConfiguration GetMailConfiguration() =>
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

        private string GetTestProject(int count) => 
            _configuration[$"Paths:TestProjects:{count}"];
    }
}