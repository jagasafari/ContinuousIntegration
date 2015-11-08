namespace ContinuousIntegration.TestRunner
{
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.Configuration;

    public class ApplicationConfiguration
    {
        public ApplicationConfiguration(IApplicationEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddUserSecrets()
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }
    }
}