using System;
using GherkinSpec.TestModel;
using Microsoft.Extensions.Logging;

namespace GherkinSpec.Logging
{
    public class TestLogger : ILogger
    {
        private readonly ITestLogAccessor testLogAccessor;

        public TestLogger(ITestLogAccessor testLogAccessor)
        {
            this.testLogAccessor = testLogAccessor;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // TODO honour loglevel and other filters
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // TODO honour loglevel and other filters
            testLogAccessor.LogStepInformation(formatter(state, exception));
        }
    }
}
