using System.Text.RegularExpressions;
using Serilog.Events;
using Serilog.Parsing;

namespace LTs.Logging.Wrappers;

/// <summary>
///     Transformation to obfuscate the access token in the log.
/// </summary>
public class ReplaceAccessTokenLogTransformation : LogTransformation
{
    private const string FindExpression = "access_token=([a-zA-Z]?\\d?)+";
    private const string ReplaceWith = "access_token=<NotShown>";

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReplaceAccessTokenLogTransformation" /> class.
    /// </summary>
    public ReplaceAccessTokenLogTransformation()
    {
        ShouldTransform = ShouldTransformFunc;
        Transform = TransformFunc;
    }

    private bool ShouldTransformFunc( LogEvent logEvent )
    {
        if( logEvent.Level != LogEventLevel.Information )
        {
            return false;
        }

        if( !logEvent.MessageTemplate.Text.StartsWith( "Request starting" ) &&
            !logEvent.MessageTemplate.Text.StartsWith( "Request finished" ) )
        {
            return false;
        }

        var regex = new Regex( FindExpression, RegexOptions.IgnoreCase );

        if( !logEvent.Properties.ContainsKey( "QueryString" ) &&
            !logEvent.MessageTemplate.Text.Contains( "access_token=", StringComparison.OrdinalIgnoreCase ) )
        {
            return false;
        }

        var value = logEvent.MessageTemplate.Text;

        if( logEvent.Properties.ContainsKey( "QueryString" ) &&
            logEvent.Properties[ "QueryString" ] is ScalarValue valueStart2 )
        {
            value = valueStart2.ToString();
        }

        var regexMatch2 = regex.Match( value );

        if( regexMatch2.Success )
        {
            return true;
        }

        return false;
    }

    private LogEvent TransformFunc( LogEvent logEvent )
    {
        var tokens = logEvent.MessageTemplate.Tokens.Select( token =>
            {
                if( token is TextToken textToken )
                {
                    var value = Regex.Replace( textToken.Text, FindExpression, ReplaceWith );

                    return new TextToken( value );
                }

                return token;
            } );

        var messageTemplate = new MessageTemplate( tokens );

        return new LogEvent(
            logEvent.Timestamp,
            logEvent.Level,
            logEvent.Exception,
            messageTemplate,
            logEvent.Properties.Select( kvp =>
                {
                    if( kvp.Key is not ("HostingRequestStartingLog" or "HostingRequestFinishedLog" or "QueryString") )
                    {
                        return new LogEventProperty( kvp.Key, kvp.Value );
                    }

                    if( kvp.Value is not ScalarValue scalarValue )
                    {
                        return new LogEventProperty( kvp.Key, kvp.Value );
                    }

                    if( scalarValue.Value == null )
                    {
                        return new LogEventProperty( kvp.Key, kvp.Value );
                    }

                    var newPropertyValue = Regex.Replace( scalarValue.Value!.ToString() ?? "", FindExpression, ReplaceWith );

                    if( newPropertyValue.GetHashCode() != kvp.Value.ToString().GetHashCode() )
                    {
                        return new LogEventProperty( kvp.Key, new ScalarValue( newPropertyValue ) );
                    }

                    // ReSharper disable once ArrangeObjectCreationWhenTypeNotEvident
                    return new LogEventProperty( kvp.Key, kvp.Value );
                } ) );
    }
}
