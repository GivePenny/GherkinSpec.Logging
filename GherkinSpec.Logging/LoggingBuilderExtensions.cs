using GherkinSpec.TestModel;
using Microsoft.Extensions.Logging;

namespace GherkinSpec.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddTestLogging(this ILoggingBuilder builder, ITestLogAccessor testLogAccessor)
            => builder.AddProvider(new TestLoggerProvider(testLogAccessor));
    }
}
