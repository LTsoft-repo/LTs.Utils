using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0290 // Primary constructor should be used

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Message assertions for <see cref="TestOutputAssertions" />.
/// </summary>
public class TestOutputMessageAssertions
{
    /// <summary>
    ///     Creates a new instance of <see cref="TestOutputMessageAssertions" />.
    /// </summary>
    /// <param name="parent">The parent test output assertions.</param>
    /// <param name="negated">Whether the assertion is negated.</param>
    /// <param name="logLevel">The optional log level filter.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public TestOutputMessageAssertions( TestOutputAssertions parent, bool negated, LogLevel? logLevel )
    {
        Parent = parent;
        Negated = negated;
        LogLevel = logLevel;
    }

    [ UsedImplicitly ]
    private TestOutputAssertions Parent { get; }

    [ UsedImplicitly ]
    private bool Negated { get; }

    [ UsedImplicitly ]
    private LogLevel? LogLevel { get; }

    /// <summary>
    ///     Asserts on the message text contained in the test output.
    /// </summary>
    /// <param name="expectedMessage">The expected message fragment.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    [ UsedImplicitly ]
    public AndConstraint<TestOutputAssertions> Containing( string expectedMessage,
                                                           string because = "",
                                                           params object[] becauseArgs )
    {
        var output = TestOutputOutputReader.GetOutput( Parent.Subject );
        var hasMatch = LogLevel.HasValue
                           ? OutputMatchesAtLevel( output, LogLevel.Value, expectedMessage )
                           : output.Contains( expectedMessage, StringComparison.Ordinal );

        if( Negated )
        {
            Execute.Assertion
                   .BecauseOf( because, becauseArgs )
                   .ForCondition( !hasMatch )
                   .FailWith( GetFailureMessage( expectedMessage ), GetFailureArguments( expectedMessage, output ) );
        }
        else
        {
            Execute.Assertion
                   .BecauseOf( because, becauseArgs )
                   .ForCondition( hasMatch )
                   .FailWith( GetFailureMessage( expectedMessage ), GetFailureArguments( expectedMessage, output ) );
        }

        return new AndConstraint<TestOutputAssertions>( Parent );
    }

    private bool OutputMatchesAtLevel( string output, LogLevel logLevel, string expectedMessage )
    {
        var matchingLines = output.Split( Environment.NewLine, StringSplitOptions.RemoveEmptyEntries )
                                  .Where( line => LogLevelOutputTags.LineMatchesLevel( line, logLevel ) );

        if( string.IsNullOrWhiteSpace( expectedMessage ) )
        {
            return matchingLines.Any();
        }

        return matchingLines.Any( line => line.Contains( expectedMessage, StringComparison.Ordinal ) );
    }

    private string GetFailureMessage( string expectedMessage )
    {
        if( Negated )
        {
            if( LogLevel.HasValue && string.IsNullOrWhiteSpace( expectedMessage ) )
            {
                return "Expected test output not to have a message at level {0}{reason}, but found:{1}";
            }

            if( LogLevel.HasValue )
            {
                return "Expected test output not to have a message at level {0} containing {1}{reason}, but found:{2}";
            }

            return "Expected test output not to have a message containing {0}{reason}, but found:{1}";
        }

        if( LogLevel.HasValue )
        {
            return "Expected test output to have a message at level {0} containing {1}{reason}, but found:{2}";
        }

        return "Expected test output to have a message containing {0}{reason}, but found:{1}";
    }

    private object[] GetFailureArguments( string expectedMessage, string output )
    {
        if( LogLevel.HasValue && Negated && string.IsNullOrWhiteSpace( expectedMessage ) )
        {
            return [ LogLevelOutputTags.GetLevelName( LogLevel.Value ), Environment.NewLine + output ];
        }

        if( LogLevel.HasValue )
        {
            return
            [
                LogLevelOutputTags.GetLevelName( LogLevel.Value ),
                expectedMessage,
                Environment.NewLine + output
            ];
        }

        return [ expectedMessage, Environment.NewLine + output ];
    }
}
