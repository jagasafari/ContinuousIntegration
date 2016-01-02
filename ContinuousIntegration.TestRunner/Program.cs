namespace ContinuousIntegration.TestRunner
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;

    public class Program
    {
        private readonly ProviderServices _providerServices;

        public Program(IApplicationEnvironment env)
        {
             _providerServices = new ProviderServices(env);
        }

        public void Main(string[] args)
        {
            _providerServices.ProgramLogger.LogInformation("Test Runner Initialized!");
            try
            {
                new TestRunner(_providerServices).Run();
            }
            catch (Exception e)
            {
                 _providerServices.ProgramLogger
                    .LogError($"Excepion cought : {Environment.NewLine} {e}");

                _providerServices.Mailer
                    .BuildMailMessage("Error",
                    e.StackTrace).Send();
            }
        }
    }
}