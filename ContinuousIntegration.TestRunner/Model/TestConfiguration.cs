namespace ContinuousIntegration.TestRunner.Model
{
    using System;
    using Common.Core;
    using Microsoft.Extensions.OptionsModel;

    public class TestConfiguration
    {
        private TimeSpan _minutesToWait;
        private string[] _testProjects;
        public TestConfiguration(IOptions<TestOptions> options){
            MinutesToWait = new TimeSpan(0, options.Value.MinutesToWait, 0);
            TestProjects = options.Value.TestProjects.Split(new[] { ',' });
        }
        public TimeSpan MinutesToWait { get { return _minutesToWait; } set { _minutesToWait = Check.NotNull(value); } }
        public string[] TestProjects { get { return _testProjects; } set { _testProjects = Check.NotNull(value); } }
    }
}