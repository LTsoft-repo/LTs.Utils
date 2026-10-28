using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LTs.Json.Extensions;

/// <summary>
///     Extensions for serializing JSON tokens and objects.
/// </summary>
public static class JTokenSerializeExtensions
{
    /// <summary>
    ///     Converts an object to a JSON string.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">The JSON string options.</param>
    /// <returns>The JSON string.</returns>
    public static string ToJson( this object? value, JsonStringOptions? options = null )
    {
        var selectedOptions = options ?? JsonStringOptions.Default;

        return JsonConvert.SerializeObject( value, GetFormatting( selectedOptions ) );
    }

    /// <summary>
    ///     Converts a <see cref="JToken" /> to a JSON string.
    /// </summary>
    /// <param name="token">The JSON token to convert.</param>
    /// <param name="options">The JSON string options.</param>
    /// <returns>The JSON string.</returns>
    public static string ToJson( this JToken token, JsonStringOptions? options = null )
    {
        var selectedOptions = options ?? JsonStringOptions.Default;

        return token.ToString( GetFormatting( selectedOptions ) );
    }

    private static Formatting GetFormatting( JsonStringOptions options )
    {
        if( options.Minify )
        {
            return Formatting.None;
        }

        return options.UseIndent ? Formatting.Indented : Formatting.None;
    }
}