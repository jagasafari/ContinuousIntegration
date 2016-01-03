namespace ContinuousIntegration.TestRunner.Abstraction{
    using Common.Mailer.Model;
    using ContinuousIntegration.TestRunner.Model;

    public interface IConfigurationReader
    {
        TestConfiguration TestConfiguration { get; }
         MailConfiguration MailConfiguration {get;}
    } 
}