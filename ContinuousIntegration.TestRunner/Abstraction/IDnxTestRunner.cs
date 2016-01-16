namespace ContinuousIntegration.TestRunner.Abstraction{
    using System.Collections.Generic;

    public interface IDnxTestRunner
    {
        string RunTests(IEnumerable<string> testProjects);
    }
}