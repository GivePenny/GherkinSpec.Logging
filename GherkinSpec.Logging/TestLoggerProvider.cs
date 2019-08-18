using GherkinSpec.TestModel;
using Microsoft.Extensions.Logging;

namespace GherkinSpec.Logging
{
    public class TestLoggerProvider : ILoggerProvider
    {
        private readonly ITestLogAccessor testLogAccessor;

        public TestLoggerProvider(ITestLogAccessor testLogAccessor)
        {
            this.testLogAccessor = testLogAccessor;
        }

        public ILogger CreateLogger(string categoryName)
            => new TestLogger(testLogAccessor);

        public void Dispose()
        {
        }
    }
}
