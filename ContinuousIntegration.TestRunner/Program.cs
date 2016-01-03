namespace ContinuousIntegration.TestRunner
{
    using System;
    using ContinuousIntegration.TestRunner.Model;
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
                while (true)
                {
                    var runner=_providerServices.TestRunner;
                    runner.TestsCompleted += SendEmail;
                    runner.Run();
                }
            }
            catch (Exception e)
            {
                _providerServices.ProgramLogger.LogError($"Excepion cought : {Environment.NewLine} {e}");
                
                SendEmail(this, new TestsCompletedEventArgs(){TestsResult = e.StackTrace, Title = "Error"});
            }
        }
        
        private void SendEmail(object sender, TestsCompletedEventArgs args){
            _providerServices.ProgramLogger.LogInformation("Sending Report email");
            _providerServices
                    .MailService
                    .BuildMailMessage(args.Title, args.TestsResult)
                    .Send();
        }
    }
}