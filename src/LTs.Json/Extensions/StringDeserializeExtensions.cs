using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LTs.Json.Extensions;

/// <summary>
///     Extensions for deserializing JSON strings.
/// </summary>
public static class StringDeserializeExtensions
{
    /// <summary>
    ///     Parses a JSON string into a <see cref="JToken" />.
    /// </summary>
    /// <param name="json">The JSON string to parse.</param>
    /// <param name="options">The JSON parse options.</param>
    /// <returns>The parsed JSON token.</returns>
    public static JToken ParseAsJToken( this string json, JsonParseOptions? options = null )
    {
        using var reader = new JsonTextReader( new StringReader( json ) );

        reader.DateParseHandling = ( options ?? JsonParseOptions.Default ).DateParseType switch
        {
            JsonDateParseType.DateTime => DateParseHandling.DateTime,
            JsonDateParseType.DateTimeOffset => DateParseHandling.DateTimeOffset,
            _ => DateParseHandling.None
        };

        return JToken.ReadFrom( reader );
    }
}