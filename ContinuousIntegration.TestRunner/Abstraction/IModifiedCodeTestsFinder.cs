namespace ContinuousIntegration.TestRunner.Abstraction{
    using System;
    using System.Collections.Generic;

    public interface IModifiedCodeTestsFinder
    {
        List<string> FilterTestProjects(string[] testProjects, DateTime lastRunTime);   
    }
}