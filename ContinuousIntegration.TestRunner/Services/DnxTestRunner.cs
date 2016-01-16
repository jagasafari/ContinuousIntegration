namespace ContinuousIntegration.TestRunner.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution.Abstraction;
    using ContinuousIntegration.TestRunner.Abstraction;
    using Microsoft.Extensions.OptionsModel;
    
    using ContinuousIntegration.TestRunner.Model;

    public class DnxTestRunner : IDnxTestRunner
    {
        private ILogger _logger;
        private readonly IFinishingExecutorFactory _processExecutorFactory;
        private readonly string _dnx;
        

        public DnxTestRunner(IFinishingExecutorFactory executorFactory,
            ILogger<DnxTestRunner> logger, IOptions<TestOptions> options)
        {
            _logger = logger;
            _processExecutorFactory = executorFactory;
            _dnx = options.Value.Dnx;
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
            _logger.LogInformation($"DnxPath {_dnx}");
            var processExecutor = _processExecutorFactory.Create(_dnx, $@"-p ""{testProjectPath}"" test", x => x.Equals("Failed"));
            processExecutor.Execute();
            stringBuilder.Append(processExecutor.Output);
        }
    }
}