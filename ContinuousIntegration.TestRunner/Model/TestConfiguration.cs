namespace ContinuousIntegration.TestRunner.Model
{
    using System;
    using System.Collections.Generic;

    public class TestConfiguration
    {
        public TestConfiguration()
        {
            TestProjects=new List<string>();
        }
        public string SolutionPath { get; set; }
        public TimeSpan MinutesToWait { get; set; }
        public List<string> TestProjects { get; set; }

    }
}