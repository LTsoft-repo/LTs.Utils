using LTs.TestUtils.Loggers;
using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Resolves <see cref="InMemoryLogger" /> instances from <see cref="ILogger" /> implementations used in tests.
/// </summary>
internal static class InMemoryLoggerResolver
{
    internal static InMemoryLogger Resolve( ILogger logger )
    {
        if( logger is InMemoryLogger inMemoryLogger )
        {
            return inMemoryLogger;
        }

        throw new InvalidOperationException(
            $"Cannot assert log messages on {logger.GetType().FullName}. " +
            "Use an ILogger implementation that derives from InMemoryLogger, such as InMemoryLogger<T> or TestLogger<T>." );
    }
}
