using LTs.TestUtils.Loggers;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0290 // Primary constructor should be used

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Assertions for <see cref="InMemoryLogger" />.
/// </summary>
public class InMemoryLoggerAssertions
{
    /// <summary>
    ///     Creates a new instance of <see cref="InMemoryLoggerAssertions" />.
    /// </summary>
    /// <param name="subject">The in-memory logger to assert on.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public InMemoryLoggerAssertions( InMemoryLogger subject )
        => Subject = subject;

    /// <summary>
    ///     Gets the in-memory logger subject.
    /// </summary>
    [ UsedImplicitly ]
    public InMemoryLogger Subject { get; }

    /// <summary>
    ///     Returns assertions for messages that should be present in the logger.
    /// </summary>
    [ UsedImplicitly ]
    public InMemoryLoggerMessageAssertions HaveMessage()
        => new( this, false, null );

    /// <summary>
    ///     Returns assertions for messages at the specified log level that should be present in the logger.
    /// </summary>
    /// <param name="logLevel">The expected log level.</param>
    [ UsedImplicitly ]
    public InMemoryLoggerMessageAssertions HaveMessage( LogLevel logLevel )
        => new( this, false, logLevel );

    /// <summary>
    ///     Returns assertions for messages that should not be present in the logger.
    /// </summary>
    [ UsedImplicitly ]
    public InMemoryLoggerMessageAssertions NotHaveMessage()
        => new( this, true, null );

    /// <summary>
    ///     Returns assertions for messages at the specified log level that should not be present in the logger.
    /// </summary>
    /// <param name="logLevel">The unexpected log level.</param>
    [ UsedImplicitly ]
    public InMemoryLoggerMessageAssertions NotHaveMessage( LogLevel logLevel )
        => new( this, true, logLevel );
}
