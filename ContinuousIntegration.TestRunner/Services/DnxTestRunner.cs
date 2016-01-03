namespace ContinuousIntegration.TestRunner.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution;
    using Common.ProcessExecution.Abstraction;

    public class DnxTestRunner
    {
        private ILogger _logger;
        private readonly IFinishingExecutorFactory _processExecutorFactory;

        public DnxTestRunner(IFinishingExecutorFactory executorFactory,
            ILogger<DnxTestRunner> logger)
        {
            _logger = logger;
            _processExecutorFactory = executorFactory;
        }

        public string RunTests(IEnumerable<string> testProjects)
        {
            var stringBuilder = new StringBuilder();
            Parallel.ForEach(testProjects, testProject => { RunTest(testProject, stringBuilder); });
            return stringBuilder.ToString();
        }

        private void RunTest(string testProjectPath,
             StringBuilder stringBuilder)
        {
            Directory.SetCurrentDirectory(testProjectPath);
            var processExecutor = _processExecutorFactory.Create(DnxInformation.DnxPath, $@"-p ""{testProjectPath}"" test", x => x.Equals("Failed"));
            processExecutor.Execute();
            stringBuilder.Append(processExecutor.Output);
        }
    }
}