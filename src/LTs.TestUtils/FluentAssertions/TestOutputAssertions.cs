using FluentAssertions.Primitives;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0290 // Primary constructor should be used

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Assertions for <see cref="ITestOutputHelper" />.
/// </summary>
public class TestOutputAssertions : ReferenceTypeAssertions<ITestOutputHelper, TestOutputAssertions>
{
    /// <summary>
    ///     Creates a new instance of <see cref="TestOutputAssertions" />.
    /// </summary>
    /// <param name="instance">The test output helper to assert on.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public TestOutputAssertions( ITestOutputHelper instance )
        : base( instance ) { }

    /// <inheritdoc />
    protected override string Identifier => "test output";

    /// <summary>
    ///     Returns assertions for messages that should be present in the test output.
    /// </summary>
    [ UsedImplicitly ]
    public TestOutputMessageAssertions HaveMessage()
        => new( this, false, null );

    /// <summary>
    ///     Returns assertions for messages at the specified log level that should be present in the test output.
    /// </summary>
    /// <param name="logLevel">The expected log level.</param>
    [ UsedImplicitly ]
    public TestOutputMessageAssertions HaveMessage( LogLevel logLevel )
        => new( this, false, logLevel );

    /// <summary>
    ///     Returns assertions for messages that should not be present in the test output.
    /// </summary>
    [ UsedImplicitly ]
    public TestOutputMessageAssertions NotHaveMessage()
        => new( this, true, null );

    /// <summary>
    ///     Returns assertions for messages at the specified log level that should not be present in the test output.
    /// </summary>
    /// <param name="logLevel">The unexpected log level.</param>
    [ UsedImplicitly ]
    public TestOutputMessageAssertions NotHaveMessage( LogLevel logLevel )
        => new( this, true, logLevel );
}
