using LTs.Json.Converters;
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
    public static string ToJson( this object? value, JsonStringOptions options = JsonStringOptions.Minify )
        => JsonConvert.SerializeObject( value, options.ToSerializerSettings() );

    /// <summary>
    ///     Converts an object to a JSON string.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="formatting">The JSON formatting.</param>
    /// <returns>The JSON string.</returns>
    public static string ToJson( this object? value, Formatting formatting )
        => JsonConvert.SerializeObject( value, formatting );

    /// <summary>
    ///     Converts an object to a JSON string.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="settings">The JSON serializer settings.</param>
    /// <returns>The JSON string.</returns>
    public static string ToJson( this object? value, JsonSerializerSettings settings )
        => JsonConvert.SerializeObject( value, settings );

    /// <summary>
    ///     Converts a <see cref="JToken" /> to a JSON string.
    /// </summary>
    /// <param name="token">The JSON token to convert.</param>
    /// <param name="options">The JSON string options.</param>
    /// <returns>The JSON string.</returns>
    public static string ToJson( this JToken token, JsonStringOptions options = JsonStringOptions.Minify )
        => JsonConvert.SerializeObject( token, options.ToSerializerSettings() );

    /// <summary>
    ///     Creates <see cref="JsonSerializerSettings" /> from <see cref="JsonStringOptions" />.
    /// </summary>
    /// <param name="options">The JSON string options.</param>
    /// <returns>The serializer settings.</returns>
    public static JsonSerializerSettings ToSerializerSettings( this JsonStringOptions options )
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = GetFormatting( options ),
            NullValueHandling = options.HasFlag( JsonStringOptions.IgnoreNulls )
                                    ? NullValueHandling.Ignore
                                    : NullValueHandling.Include
        };

        // Reuse returned settings when deserializing so enum flags apply on read as well as ToJson.
        AddEnumConverter( settings, options );

        return settings;
    }

    private static Formatting GetFormatting( JsonStringOptions options )
    {
        if( options.HasFlag( JsonStringOptions.UseIndent ) && !options.HasFlag( JsonStringOptions.Minify ) )
        {
            return Formatting.Indented;
        }

        return Formatting.None;
    }

    private static void AddEnumConverter( JsonSerializerSettings settings, JsonStringOptions options )
    {
        settings.Converters.Add( new EnumJsonConverter( options ) );
    }
}
