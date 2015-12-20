namespace ContinuousIntegration.TestRunner
{
    using Common.Mailer.Model;
    using Model;
    using Common.Core;

    public class ModelProvider
    {
        private readonly ConfigurationReader _configurationReader;

        public ModelProvider(ConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }

        public TestConfiguration TestConfiguration => 
            Check.NotNull<TestConfiguration>(_configurationReader
            .GetTestConfiguration());

        public MailConfiguration MailConfiguration => 
            Check.NotNull<MailConfiguration>(_configurationReader
                 .GetMailConfiguration());
    }
}