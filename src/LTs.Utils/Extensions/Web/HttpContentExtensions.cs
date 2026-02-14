using System.Collections.Immutable;
using System.Net;
using Newtonsoft.Json;

namespace LTs.Utils.Extensions.Web;

/// <summary>
///     Extensions for <see cref="HttpContent" />.
/// </summary>
public static class HttpContentExtensions
{
    /// <summary>
    ///     Reads the content of the <paramref name="content" /> as a JSON and converts it to <see cref="object" />.
    /// </summary>
    /// <param name="content">The content to read.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The JSON object.</returns>
    [ UsedImplicitly ]
    public static async Task<object?> ReadAsJsonAsync( this HttpContent content, CancellationToken cancellationToken )
    {
        var jsonContent = await content.ReadAsStringAsync( cancellationToken );
        var obj = JsonConvert.DeserializeObject<object>( jsonContent );

        return obj;
    }

    /// <summary>
    ///     Reads the content of the <paramref name="content" /> as a JSON and converts it to the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content">The content to read.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The object.</returns>
    [ UsedImplicitly ]
    public static async Task<T?> ReadAsJsonAsync<T>( this HttpContent content, CancellationToken cancellationToken )
    {
        var jsonContent = await content.ReadAsStringAsync( cancellationToken );
        var obj = JsonConvert.DeserializeObject<T>( jsonContent );

        return obj;
    }

    /// <summary>
    ///     Reads the content of the <see cref="HttpContent" /> as a form.
    /// </summary>
    /// <param name="content"> The HttpContent to read from.</param>
    /// <returns>A dictionary containing the form fields.</returns>
    public static async Task<IImmutableDictionary<string, string>> ReadFormAsync( this HttpContent? content )
    {
        ArgumentNullException.ThrowIfNull( content, nameof( content ) );

        var formFields = new Dictionary<string, string>();

        // Read the content as a string
        var encodedString = await content.ReadAsStringAsync();

        if( string.IsNullOrWhiteSpace( encodedString ) )
        {
            return ImmutableDictionary<string, string>.Empty;
        }

        // Parse the string as form data
        var decodedString = WebUtility.UrlDecode( encodedString );
        var keyValuePairs = decodedString.Split( '&' );

        foreach( var pair in keyValuePairs )
        {
            var keyValue = pair.Split( '=' );

            if( keyValue.Length == 2 )
            {
                var key = Uri.UnescapeDataString( keyValue[ 0 ] );
                var value = Uri.UnescapeDataString( keyValue[ 1 ] );
                formFields[ key ] = value;
            }
        }

        return formFields.ToImmutableDictionary();
    }
}