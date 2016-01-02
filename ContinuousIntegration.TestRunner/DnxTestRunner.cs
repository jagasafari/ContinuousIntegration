namespace ContinuousIntegration.TestRunner
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution;

    public class DnxTestRunner
    {
        private ILogger _logger;
        private readonly ProcessProviderServices _processProviderServices;

        public DnxTestRunner(ProcessProviderServices processProviderServices,
            ILogger<DnxTestRunner> logger)
        {
            _logger = logger;
            _processProviderServices = processProviderServices;
        }

        public string RunTests(IEnumerable<string> testProjects)
        {
            var stringBuilder = new StringBuilder();
            Parallel.ForEach(testProjects,
                f => { RunTest(f, stringBuilder); });
            return stringBuilder.ToString();
        }

        private void RunTest(string testProjectPath,
             StringBuilder stringBuilder)
        {
            Directory.SetCurrentDirectory(testProjectPath);

            var processExecutor = _processProviderServices
                .FinishingExecutor(DnxInformation.DnxPath, $@"-p ""{testProjectPath}"" test", x => x.Equals("Failed"));

            processExecutor.Execute();

            stringBuilder.Append(processExecutor.Output);
        }
    }
}