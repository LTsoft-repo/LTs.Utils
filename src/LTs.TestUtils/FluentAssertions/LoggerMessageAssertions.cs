using FluentAssertions;
using FluentAssertions.Execution;
using LTs.TestUtils.Loggers;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0290 // Primary constructor should be used

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Message assertions for <see cref="LoggerAssertions" />.
/// </summary>
public class LoggerMessageAssertions
{
    /// <summary>
    ///     Creates a new instance of <see cref="LoggerMessageAssertions" />.
    /// </summary>
    /// <param name="parent">The parent logger assertions.</param>
    /// <param name="negated">Whether the assertion is negated.</param>
    /// <param name="logLevel">The optional log level filter.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public LoggerMessageAssertions( LoggerAssertions parent, bool negated, LogLevel? logLevel )
    {
        Parent = parent;
        Negated = negated;
        LogLevel = logLevel;
    }

    [ UsedImplicitly ]
    private LoggerAssertions Parent { get; }

    [ UsedImplicitly ]
    private bool Negated { get; }

    [ UsedImplicitly ]
    private LogLevel? LogLevel { get; }

    /// <summary>
    ///     Asserts on the message text captured by the logger.
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
    public AndConstraint<LoggerAssertions> Containing( string expectedMessage,
                                                       string because = "",
                                                       params object[] becauseArgs )
    {
        var hasMatch = Parent.Subject.Messages.Any( m => MessageMatches( m, expectedMessage ) );

        if( Negated )
        {
            Execute.Assertion
                   .BecauseOf( because, becauseArgs )
                   .ForCondition( !hasMatch )
                   .FailWith( GetFailureMessage( expectedMessage ), GetFailureArguments( expectedMessage ) );
        }
        else
        {
            Execute.Assertion
                   .BecauseOf( because, becauseArgs )
                   .ForCondition( hasMatch )
                   .FailWith( GetFailureMessage( expectedMessage ), GetFailureArguments( expectedMessage ) );
        }

        return new AndConstraint<LoggerAssertions>( Parent );
    }

    private bool MessageMatches( Loggers.LoggerMessage message, string expectedMessage )
    {
        if( LogLevel.HasValue && message.LogLevel != LogLevel.Value )
        {
            return false;
        }

        if( string.IsNullOrWhiteSpace( expectedMessage ) )
        {
            return LogLevel.HasValue;
        }

        var comparison = Negated && LogLevel.HasValue
                             ? StringComparison.OrdinalIgnoreCase
                             : StringComparison.Ordinal;

        return message.Text.Contains( expectedMessage, comparison );
    }

    private string GetFailureMessage( string expectedMessage )
    {
        if( Negated )
        {
            if( LogLevel.HasValue && string.IsNullOrWhiteSpace( expectedMessage ) )
            {
                return "Expected logger not to have a message at level {0}{reason}, but such a message was found.";
            }

            if( LogLevel.HasValue )
            {
                return "Expected logger not to have a message at level {0} containing {1}{reason}, but such a message was found.";
            }

            return "Expected logger not to have a message containing {0}{reason}, but such a message was found.";
        }

        if( LogLevel.HasValue )
        {
            return "Expected logger to have a message at level {0} containing {1}{reason}, but no such message was found.";
        }

        return "Expected logger to have a message containing {0}{reason}, but no such message was found.";
    }

    private object[] GetFailureArguments( string expectedMessage )
    {
        if( LogLevel.HasValue )
        {
            return [ LogLevelOutputTags.GetLevelName( LogLevel.Value ), expectedMessage ];
        }

        return [ expectedMessage ];
    }
}
