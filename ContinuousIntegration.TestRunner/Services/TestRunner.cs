namespace ContinuousIntegration.TestRunner.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using ContinuousIntegration.TestRunner.Abstraction;
    using ContinuousIntegration.TestRunner.Model;
    using Microsoft.Extensions.Logging;

    public class TestRunner : ITestRunner
    {
        private DateTime _lastRunTime;
        private ILogger<TestRunner> _logger;
        private IModifiedCodeTestsFinder _testsFinder;
        private IDnxTestRunner _dnxTestRunner;
        private TestConfiguration _testConfiguration;

        public TestRunner(ILogger<TestRunner> logger, TestConfiguration testOptions,
            IModifiedCodeTestsFinder testsFinder, IDnxTestRunner dnxTestRunner)
        {
            _logger = logger;
            _testsFinder = testsFinder;
            _dnxTestRunner = dnxTestRunner;
            
            _testConfiguration = testOptions;
            _lastRunTime = DateTime.UtcNow.AddYears(-1000);
        }
        
        public event EventHandler<TestsCompletedEventArgs> TestsCompleted;
        
        public void Run()
        {
            var testsToRun = _testsFinder.FilterTestProjects(_testConfiguration.TestProjects, _lastRunTime);

            _logger.LogInformation($"{testsToRun.Count} projects to test:");
            testsToRun.ForEach(_logger.LogInformation);

            if (testsToRun.Any())
            {
                var testResults = _dnxTestRunner.RunTests(testsToRun);
                
                OnTestsCompleted(new TestsCompletedEventArgs(){TestsResult=testResults, Title="Tests completed, Result"});
            }
            else
            {
                _logger.LogInformation("Nothing have changed");
            }

            _lastRunTime = DateTime.UtcNow;

            _logger.LogInformation($"Going to sleep for {_testConfiguration.MinutesToWait} minutes");

            Thread.Sleep(_testConfiguration.MinutesToWait);
        }
        
        protected virtual void OnTestsCompleted(TestsCompletedEventArgs e){
            var handler = TestsCompleted;
            if(handler!=null)
            {
                handler(this, e);
            }
        }
        

    }
}