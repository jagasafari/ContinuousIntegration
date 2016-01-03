namespace ContinuousIntegration.TestRunner.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Common.Core;
    using Microsoft.Extensions.Logging;

    public class ModifiedCodeTestsFinder
    {
        private readonly ILogger _logger;

        public ModifiedCodeTestsFinder(ILogger<ModifiedCodeTestsFinder> logger)
        {
            _logger = logger;
        }

        public List<string> FilterTestProjects(List<string> testProjects, DateTime lastRunTime)
        {
            var modifiedProjects = new List<string>();
            int solutionLevel = 2;
            foreach (var testProject in testProjects)
            {
                if (Search(lastRunTime, testProject.GetParentDirectory(solutionLevel)))
                {
                    modifiedProjects.Add(testProject);
                }
            }
            return modifiedProjects;
        }

        private bool Search(DateTime lastRunTime, string solutionPath)
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