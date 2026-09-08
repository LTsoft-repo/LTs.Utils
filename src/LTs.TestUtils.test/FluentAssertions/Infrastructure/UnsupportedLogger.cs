using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.test.FluentAssertions.Infrastructure;

internal sealed class UnsupportedLogger : ILogger
{
    public IDisposable BeginScope<TState>( TState state ) where TState : notnull
        => throw new NotSupportedException();

    public bool IsEnabled( LogLevel logLevel ) => true;

    public void Log<TState>( LogLevel logLevel,
                             EventId eventId,
                             TState state,
                             Exception? exception,
                             Func<TState, Exception?, string> formatter ) { }
}
