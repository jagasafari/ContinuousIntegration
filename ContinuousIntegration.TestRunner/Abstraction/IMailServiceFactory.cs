namespace ContinuousIntegration.TestRunner.Abstraction{
    using Common.Mailer;

    public interface IMailServiceFactory
    {
        IMailService Create();
    } 
}