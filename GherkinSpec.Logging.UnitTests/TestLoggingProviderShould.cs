using GherkinSpec.TestModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GherkinSpec.Logging.UnitTests
{
    [TestClass]
    public class TestLoggingProviderShould
    {
        class MockTestSubject
        {
            public MockTestSubject(ILogger<MockTestSubject> logger)
            {
                Logger = logger;
            }

            public ILogger<MockTestSubject> Logger { get; }
        }

        class MockAccessor : ITestLogAccessor
        {
            public void LogStepInformation(string message)
            {
                LoggedMessage = message;
            }

            public string LoggedMessage { get; private set; }
        }

        [TestMethod]
        public void LogInformationMessagesToTestLogAccessor()
        {
            var testLogAccessor = new MockAccessor();

            var services = new ServiceCollection();

            var serviceProvider = services
                .AddLogging(
                    builder => builder.AddTestLogging(testLogAccessor))
                .AddSingleton<MockTestSubject>()
                .AddSingleton(testLogAccessor)
                .AddSingleton<ITestLogAccessor>(testLogAccessor)
                .BuildServiceProvider();

            var mock = serviceProvider.GetRequiredService<MockTestSubject>();
            mock.Logger.LogInformation("Hello world!");

            Assert.AreEqual("Hello world!", testLogAccessor.LoggedMessage);
        }
    }
}
