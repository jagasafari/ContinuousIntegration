namespace ContinuousIntegration.TestRunner
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Common;
    using ContinuousIntegration.TestRunner.Model;
    using Microsoft.Extensions.Logging;
    using ProcessExecution;
    using ProcessExecution.Model;

    public class DnxTestRunner
    {
        private ILogger _logger;
        private readonly ProcessProviderServices _processProviderServices;
        private TestConfiguration _testConfiguration;

        public DnxTestRunner(ProviderServices providerServices,
        ProcessProviderServices processProviderServices)
        {
            var providerServicesTmp = Check.NotNull<ProviderServices>(providerServices);
            _logger = providerServicesTmp.Logger(nameof(DnxTestRunner));
            _testConfiguration = providerServices.ProviderModels.TestConfiguration;
            
            _processProviderServices = Check.NotNull<ProcessProviderServices>(processProviderServices);
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
            _logger.LogInformation($"Testing {testProjectPath} project");

            var instructions = new ProcessInstructions
            {
                Program = DnxInformation.DnxPath,
                Arguments = $@"-p ""{testProjectPath}"" test"
            };

            var processExecutor = _processProviderServices
                .FinishingProcessExecutor(instructions, _logger);

            processExecutor.ExecuteAndWait(x => x.Equals("Failed"));

            _logger.LogInformation("Tests Completed!");
            stringBuilder.Append(processExecutor.Output);
        }
    }
}