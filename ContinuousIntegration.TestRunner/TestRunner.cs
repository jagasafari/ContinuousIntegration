namespace ContinuousIntegration.TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Common.Core;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution;

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
                var testConfiguration = _providerServices.ModelProvider.TestConfiguration;
                
                var testsToRun = FilterModifiedProjectFiles(testConfiguration.TestProjects).ToList();
                logger.LogInformation($"{testsToRun.Count} projects to test:");
                testsToRun.ForEach(logger.LogInformation);
                
                if (testsToRun.Any())
                {
                    var testResults = _providerServices.DnxTestRunner.RunTests(testsToRun);
                                            
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

        private IEnumerable<string> FilterModifiedProjectFiles(List<string> testProjects)
        {
            List<string> modifiedProjects=new List<string>();
            int solutionLevel = 2;
            foreach (var testProject in testProjects)
            {
                if (_providerServices.ModifiedFileFinder
                    .Search(_lastRunTime,  GetParentDirectory(testProject, solutionLevel)))
                {
                    modifiedProjects.Add(testProject);
                }
            }
            return modifiedProjects;
        }
        
       private string GetParentDirectory(string fullPath, int level){
           while(level-- > 0)
               fullPath = Directory.GetParent(fullPath).FullName;
           return fullPath;
       }
    }
}