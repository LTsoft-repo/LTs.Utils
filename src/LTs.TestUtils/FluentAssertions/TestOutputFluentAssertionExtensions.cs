using FluentAssertions;
using JetBrains.Annotations;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Extensions for <see cref="ITestOutputHelper" /> assertions.
/// </summary>
public static class TestOutputFluentAssertionExtensions
{
    /// <summary>
    ///     Returns a <see cref="TestOutputAssertions" /> object that can be used to assert the current
    ///     <see cref="ITestOutputHelper" />.
    /// </summary>
    /// <param name="testOutput">The test output helper to assert on.</param>
    [ Pure ]
    public static TestOutputAssertions Should( this ITestOutputHelper testOutput )
        => new( testOutput );
}
