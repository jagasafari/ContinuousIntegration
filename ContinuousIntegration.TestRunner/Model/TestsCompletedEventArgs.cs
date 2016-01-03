namespace ContinuousIntegration.TestRunner.Model
{
    using System;
    public class TestsCompletedEventArgs : EventArgs {
        public string TestsResult {get;set;}
        public string Title {get; set;} 
    }
}