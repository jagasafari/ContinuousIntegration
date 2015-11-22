namespace ContinuousIntegration.TestRunner
{
    using System;
    using System.Threading;
    using Common;
    using Microsoft.Extensions.Logging;
    using ProcessExecution;

    public class TestRunner
    {
        private readonly ProviderServices _providerServices;
        private DateTime _lastRunTime;

        public TestRunner(ProcessProviderServices processProviderServices,
            ProviderServices providerServices)
        {
            _providerServices = Check.NotNull<ProviderServices>(providerServices);
            _lastRunTime = DateTime.UtcNow.AddYears(-1000);
        }

        public void Run()
        {
            while (true)
            {
                var logger = _providerServices.Logger(nameof(TestRunner));
                var testConfiguration = _providerServices.ProviderModels.TestConfiguration;
                
                if (_providerServices.ModifiedFileFinder
                    .Search(_lastRunTime, testConfiguration.SolutionPath))
                {
                    var testResults = _providerServices.DnxTestRunner.RunTests();
                                            
                    logger.LogInformation("Sending Report email");

                    _providerServices.Mailer
                        .BuildMailMessage("Result", testResults)
                        .Send();
                }
                else
                {
                    logger.LogInformation("Nothing have changed");
                }

                _lastRunTime = DateTime.UtcNow;
                logger.LogInformation(
                    $"Going to sleep for {testConfiguration.MinutesToWait} minutes");
                Thread.Sleep(testConfiguration.MinutesToWait);
            }
        }
    }
}