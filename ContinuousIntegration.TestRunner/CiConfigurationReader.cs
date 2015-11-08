namespace ContinuousIntegration.TestRunner
{
    using System;
    using Microsoft.Framework.Configuration;

    public class CiConfigurationReader
    {
        private readonly IConfiguration _configuration;

        public CiConfigurationReader(ApplicationConfiguration configuration)
        {
            _configuration = configuration.Configuration;
        }

        public string ApplicationBasePath { get; set; }

        public CiTestConfiguration GetTestConfiguration()
        {
            var ciTestConfiguration = new CiTestConfiguration
            {
                SolutionPath = _configuration["Paths:Solution"],
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

        public CiMailConfiguration GetMailConfiguration()
        {
            var ciMailConfiguration = new CiMailConfiguration
            {
                Sender =
                    _configuration[
                        GetKey(nameof(CiMailConfiguration.Sender))],
                Password =
                    _configuration[
                        GetKey(nameof(CiMailConfiguration.Password))],
                SmtpPort =
                    int.Parse(
                        _configuration[
                            GetKey(nameof(CiMailConfiguration.SmtpPort))]),
                SmtpHost =
                    _configuration[
                        GetKey(nameof(CiMailConfiguration.SmtpHost))],
                Receiver =
                    _configuration[
                        GetKey(nameof(CiMailConfiguration.Receiver))]
            };
            return ciMailConfiguration;
        }

        private static string GetKey(string property)
        {
            return $"EMail:{property}";
        }

        private string GetTestProject(int count)
        {
            return _configuration[$"Paths:TestProjects:{count}"];
        }
    }
}