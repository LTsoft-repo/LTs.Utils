using FluentAssertions;
using JetBrains.Annotations;
using LTs.TestUtils.Loggers;
using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Extensions for <see cref="ILogger" /> assertions.
/// </summary>
public static class LoggerFluentAssertionExtensions
{
    /// <summary>
    ///     Returns a <see cref="LoggerAssertions" /> object that can be used to assert the current
    ///     <see cref="ILogger" /> when it is backed by <see cref="InMemoryLogger" />.
    /// </summary>
    /// <param name="logger">The logger to assert on.</param>
    [ Pure ]
    public static LoggerAssertions Should( this ILogger logger )
        => new( InMemoryLoggerResolver.Resolve( logger ) );

    /// <summary>
    ///     Returns a <see cref="LoggerAssertions" /> object that can be used to assert the current
    ///     <see cref="ILogger{T}" /> when it is backed by <see cref="InMemoryLogger" />.
    /// </summary>
    /// <typeparam name="T">The logger category type.</typeparam>
    /// <param name="logger">The logger to assert on.</param>
    [ Pure ]
    public static LoggerAssertions Should<T>( this ILogger<T> logger )
        => new( InMemoryLoggerResolver.Resolve( logger ) );
}
