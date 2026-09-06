using Serilog.Events;
using Serilog.Parsing;

#pragma warning disable IDE0290

namespace LTs.Logging.Wrappers;

/// <summary>
///     Log transformation that replaces text in the log message.
/// </summary>
public class ReplaceTextLogTransformation : LogTransformation
{
    private readonly LogEventLevel? logEventLevel;
    private readonly string replaceWith;
    private readonly string textToRemove;

    /// <summary>
    ///     Creates a new instance of <see cref="ReplaceTextLogTransformation" />.
    /// </summary>
    /// <param name="textToRemove">Text to replace from the log message.</param>
    /// <param name="replaceWith">Text to replace with.</param>
    /// <param name="logEventLevel">Log event level to apply the transformation.</param>
    public ReplaceTextLogTransformation( string textToRemove, string replaceWith, LogEventLevel? logEventLevel = null )
    {
        this.textToRemove = textToRemove;
        this.replaceWith = replaceWith;
        this.logEventLevel = logEventLevel;

        ShouldTransform = ShouldTransformFunc;
        Transform = TransformFunc;
    }

    private bool ShouldTransformFunc( LogEvent logEvent ) =>
        ( logEventLevel == null || logEventLevel == logEvent.Level ) &&
        logEvent.MessageTemplate.Text.Contains( textToRemove );

    private LogEvent TransformFunc( LogEvent logEvent )
    {
        var newTokens = logEvent.MessageTemplate.Tokens.Select( t =>
                {
                    if( t is TextToken token && token.Text.Contains( textToRemove ) )
                    {
                        return new TextToken( token.Text.Replace( textToRemove, replaceWith ) );
                    }

                    return t;
                } )
            .ToList();

        return new( logEvent.Timestamp,
            logEvent.Level,
            logEvent.Exception,
            new( newTokens ),
            logEvent.Properties.Select( kvp => new LogEventProperty( kvp.Key, kvp.Value ) ) );
    }
}
