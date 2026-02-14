using System.Text.RegularExpressions;

namespace LTs.Utils.Extensions.Exceptions;

/// <summary>
///     Extensions for <see cref="ArgumentException" />.
/// </summary>
public static class ArgumentExceptionExtensions
{
    /// <summary>
    ///     Gets the information about the <see cref="ArgumentException" />.
    /// </summary>
    /// <param name="ex">Exception to get information from.</param>
    /// <returns>A <see cref="ArgumentExceptionInformation" /> object containing the parameter name and error type.</returns>
    /// "/>
    public static ArgumentExceptionInformation GetInformation( this ArgumentException ex )
    {
        var parameter = GetParameterName( ex );

        var errorType = GetTypeFromMessage( ex );

        return new ArgumentExceptionInformation
        {
            Parameter = parameter,
            ErrorType = errorType
        };
    }

    private static string GetParameterName( ArgumentException ex )
    {
        var parameter = ex.ParamName ?? string.Empty;

        if( !string.IsNullOrWhiteSpace( parameter ) )
        {
            return parameter;
        }

        var match = Regex.Match(
            ex.Message,
            @"\(Parameter\s+'(?<paramName>[^']+)'\)"
        );

        if( !match.Success )
        {
            return string.Empty;
        }

        var paramName = match.Groups[ "paramName" ].Value;

        return paramName;
    }

    private static ArgumentExceptionErrorType GetTypeFromMessage( ArgumentException ex )
    {
        if( ex is ArgumentNullException ||
            ex.Message.Contains( "cannot be null" ) )
        {
            return ArgumentExceptionErrorType.Null;
        }

        if( ex.Message.Contains( "cannot be an empty string" ) ||
            ex.Message.Contains( "cannot be empty" ) )
        {
            return ArgumentExceptionErrorType.Empty;
        }

        if( ex.Message.Contains( "invalid" ) )
        {
            return ArgumentExceptionErrorType.Invalid;
        }

        return ArgumentExceptionErrorType.Other;
    }
}