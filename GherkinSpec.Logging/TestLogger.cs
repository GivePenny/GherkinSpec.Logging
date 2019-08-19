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
            if (testLogAccessor is null)
            {
                throw new ArgumentNullException(nameof(testLogAccessor));
            }

            this.testLogAccessor = testLogAccessor;
        }

        public IDisposable BeginScope<TState>(TState state)
            => new Scope();

        public bool IsEnabled(LogLevel logLevel)
        {
            // TODO honour loglevel and other filters
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            if (!testLogAccessor.IsInRunningTest)
            {
                // Can't log to a test case outside of a running test, leave it to another logger to deal with
                return;
            }

            var message = formatter(state, exception);

            if (exception != null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = exception.ToString();
                }
                else
                {
                    message += Environment.NewLine + exception.ToString();
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                testLogAccessor.LogStepInformation(message);
            }
        }
    }
}
