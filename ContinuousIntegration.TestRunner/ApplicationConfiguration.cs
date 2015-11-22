namespace ContinuousIntegration.TestRunner
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.PlatformAbstractions;

    public class ApplicationConfiguration
    {
        //To-Do remove once AddUserSecrets works
        const string UserSecretPath = @"C:\Users\mika\AppData\Roaming\Microsoft\UserSecrets\ContinuousIntegration.TestRunner-4ca7ba98-1d59-433f-8494-ef169053cccc\secrets.json";
        public ApplicationConfiguration(IApplicationEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile(UserSecretPath)
                .AddEnvironmentVariables()
                //.AddUserSecrets()
                
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }
    }
}