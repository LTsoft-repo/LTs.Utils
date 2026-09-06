using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LTs.Json.Extensions;

/// <summary>
///     Extensions for serializing JSON tokens.
/// </summary>
public static class JTokenSerializeExtensions
{
    /// <summary>
    ///     Converts a <see cref="JToken" /> to a JSON string.
    /// </summary>
    /// <param name="token">The JSON token to convert.</param>
    /// <param name="options">The JSON string options.</param>
    /// <returns>The JSON string.</returns>
    public static string ToJson( this JToken token, JsonStringOptions? options = null )
    {
        var selectedOptions = options ?? JsonStringOptions.Default;

        if( selectedOptions.Minify )
        {
            return token.ToString( Formatting.None );
        }

        return token.ToString( selectedOptions.UseIndent
                                   ? Formatting.Indented
                                   : Formatting.None );
    }
}