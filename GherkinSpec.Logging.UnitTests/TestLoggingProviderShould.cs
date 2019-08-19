using GherkinSpec.TestModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

            public bool IsInRunningTest { get; set; } = true;
        }

        private ILogger logger;
        private MockAccessor testLog;

        [TestInitialize]
        public void Setup()
        {
            testLog = new MockAccessor();

            var services = new ServiceCollection();
            var serviceProvider = services
                .AddLogging(
                    builder => builder.AddTestLogging(testLog))
                .AddSingleton<MockTestSubject>()
                .AddSingleton(testLog)
                .AddSingleton<ITestLogAccessor>(testLog)
                .BuildServiceProvider();

            logger = serviceProvider
                .GetRequiredService<MockTestSubject>()
                .Logger;
        }

        [TestMethod]
        public void NotLogOutsideOfARunningTest()
        {
            testLog.IsInRunningTest = false;
            logger.LogInformation("Hello world!");
            Assert.IsNull(testLog.LoggedMessage);
        }

        [TestMethod]
        public void LogInformationMessagesToTestLogAccessor()
        {
            logger.LogInformation("Hello world!");
            Assert.AreEqual("Hello world!", testLog.LoggedMessage);
        }

        [TestMethod]
        public void LogExceptionsToTestLogAccessor()
        {
            var exception = new InvalidOperationException("Goodbye world");
            logger.LogError(exception, "Associated message");
            Assert.AreEqual("Associated message" + Environment.NewLine + exception, testLog.LoggedMessage);
        }
    }
}
