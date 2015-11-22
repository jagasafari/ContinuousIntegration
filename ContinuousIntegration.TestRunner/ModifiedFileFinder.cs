namespace ContinuousIntegration.TestRunner
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    public class FileFinder
    {
        private readonly ILogger _logger;

        public FileFinder(ProviderServices provider)
        {
            _logger = provider.Logger("FileFinder");
        }

        internal bool Search(DateTime lastRunTime, string solutionPath)
        {
            foreach (var pattern in new[] { "*.cs", "*.cshtml" })
            {
                var files =
                    new DirectoryInfo(solutionPath).GetFiles(pattern,
                        SearchOption.AllDirectories);
                _logger.LogInformation(
                    $"{files.Length} {pattern} under the solution");
                var modifiedFileFound =
                    files.Any(f => f.LastWriteTimeUtc > lastRunTime);
                _logger.LogInformation(
                    $"{modifiedFileFound} that at least one of them was modified since {lastRunTime} minutes");
                if (modifiedFileFound)
                    return true;
            }
            return false;
        }
    }
}