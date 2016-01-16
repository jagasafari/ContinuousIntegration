namespace ContinuousIntegration.TestRunner.Abstraction{
    using System;
    using ContinuousIntegration.TestRunner.Model;

    public interface ITestRunner
    {
        event EventHandler<TestsCompletedEventArgs> TestsCompleted;   
        void Run();
    }
}