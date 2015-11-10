namespace ContinuousIntegration.TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Logging;
    using Model;
    using ProcessExecution;
    using ProcessExecution.Model;

    public class TestRunner
    {
        private readonly ProviderServices _providerServices;
        private readonly ProcessProviderServices _processProviderServices;
        private readonly ApplicationLogger _logger;
        private TestConfiguration _testConfiguration;
        private DateTime _lastRunTime;

        public TestRunner(ProviderServices providerServices,
            ProcessProviderServices processProviderServices)
        {
            _providerServices = providerServices;
            _processProviderServices = processProviderServices;
            _logger = providerServices.ApplicationLogger;
            _lastRunTime = DateTime.UtcNow.AddYears(-1000);
        }

        public TestRunner(TestConfiguration testConfiguration)
        {
            _testConfiguration = testConfiguration;
        }

        public void Run()
        {
            while(true)
            {
                if(_providerServices.ModifiedFileFinder
                    .Search(_lastRunTime, _testConfiguration.SolutionPath))
                {
                    var testResults = RunTests();
                    _logger.Info("Sending Report email");

                    _providerServices.Mailer
                        .BuildMailMessage("Result", testResults)
                        .Send();
                }
                else
                {
                    _logger.Info("Nothing have changed");
                }

                _lastRunTime = DateTime.UtcNow;
                _logger.Info(
                    $"Going to sleep for {_testConfiguration.MinutesToWait} minutes");
                Thread.Sleep(_testConfiguration.MinutesToWait);
            }
        }

        public string RunTests()
        {
            var stringBuilder = new StringBuilder();
            Parallel.ForEach(GetTestProjects(),
                f => { RunTest(f, stringBuilder); });
            return stringBuilder.ToString();
        }

        private IEnumerable<string> GetTestProjects()
        {
            return _testConfiguration.TestProjects.Select(
                testProject =>
                    Path.Combine(
                        _testConfiguration.SolutionPath,
                        testProject));
        }

        private void RunTest(string testProjectPath,
            StringBuilder stringBuilder)
        {
            Directory.SetCurrentDirectory(testProjectPath);
            _logger.Info($"Testing {testProjectPath} project");

            var instructions = new ProcessInstructions
            {
                Program = DnxInformation.DnxPath,
                Arguments = $@"-p ""{testProjectPath}"" test"
            };

            var processExecutor = _processProviderServices
                .FinishingProcessExecutor(instructions);

            processExecutor.ExecuteAndWait(x => x.Equals("Failed"));

            _logger.Info("Tests Completed!");
            stringBuilder.Append(processExecutor.Output);
        }
    }
}