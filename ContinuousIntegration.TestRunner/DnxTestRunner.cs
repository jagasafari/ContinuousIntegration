namespace ContinuousIntegration.TestRunner
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Core;
    using Microsoft.Extensions.Logging;
    using Common.ProcessExecution;
    using Common.ProcessExecution.Model;

    public class DnxTestRunner
    {
        private ILogger _logger;
        private readonly ProcessProviderServices _processProviderServices;

        public DnxTestRunner(ProviderServices providerServices,
        ProcessProviderServices processProviderServices)
        {
            var providerServicesTmp = Check.NotNull<ProviderServices>(providerServices);
            _logger = providerServicesTmp.Logger(nameof(DnxTestRunner));
            
            _processProviderServices = Check.NotNull<ProcessProviderServices>(processProviderServices);
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

            var instructions = new ProcessInstructions
            {
                Program = DnxInformation.DnxPath,
                Arguments = $@"-p ""{testProjectPath}"" test"
            };

            var processExecutor = _processProviderServices
                .FinishingProcessExecutor(instructions, _logger, x => x.Equals("Failed"));

            processExecutor.Execute();

            stringBuilder.Append(processExecutor.Output);
        }
    }
}