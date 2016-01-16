namespace ContinuousIntegration.TestRunner.Model
{
    using Common.Core;

    public class TestOptions
    {
        private int _minutesToWait;
        private string _testProjects;
        private string _dnx;
        public int MinutesToWait { get { return _minutesToWait; } set { _minutesToWait = Check.NotNegative(value, nameof(value)); } }
        public string TestProjects { get { return _testProjects; } set { _testProjects = Check.NotNullOrWhiteSpace(value, nameof(value)); } }
        public string Dnx { get { return _dnx; } set { _dnx = Check.NotNullOrWhiteSpace(value, nameof(value)); } }
    }
}