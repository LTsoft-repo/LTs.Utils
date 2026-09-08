using FluentAssertions;
using JetBrains.Annotations;
using LTs.TestUtils.Loggers;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Extensions for <see cref="InMemoryLogger" /> assertions.
/// </summary>
public static class InMemoryLoggerFluentAssertionExtensions
{
    /// <summary>
    ///     Returns an <see cref="InMemoryLoggerAssertions" /> object that can be used to assert the current
    ///     <see cref="InMemoryLogger" />.
    /// </summary>
    /// <param name="logger">The in-memory logger to assert on.</param>
    [ Pure ]
    public static InMemoryLoggerAssertions Should( this InMemoryLogger logger )
        => new( logger );
}
