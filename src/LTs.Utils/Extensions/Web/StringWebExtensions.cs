using Microsoft.AspNetCore.WebUtilities;

namespace LTs.Utils.Extensions.Web;

/// <summary>
///     Extensions for web-related strings.
/// </summary>
public static class StringWebExtensions
{
    /// <summary>
    ///     Adds a query string <paramref name="key" /> and <paramref name="value" /> to the end of the URI.
    /// </summary>
    /// <param name="uri">The URI to add the query string to.</param>
    /// <param name="key">The key of the query string to add.</param>
    /// <param name="value">The value of the query string to add.</param>
    /// <returns>The URI with the query string added.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string AddQueryString( this string uri, string key, string value )
    {
        _ = uri ?? throw new ArgumentNullException( nameof( uri ) );
        _ = key ?? throw new ArgumentNullException( nameof( key ) );
        _ = value ?? throw new ArgumentNullException( nameof( value ) );

        if( string.IsNullOrWhiteSpace( key ) )
        {
            throw new ArgumentException( "Value cannot be empty.", nameof( key ) );
        }

        return QueryHelpers.AddQueryString( uri, key.Trim(), value );
    }

    /// <summary>
    ///     Combines the URI and the path.
    /// </summary>
    /// <param name="uri">The URI to combine.</param>
    /// <param name="path">The path to combine.</param>
    /// <returns>The combined URI and path.</returns>
    public static string CombineUri( this string uri, string path )
        => $"{uri.TrimEnd( '/' )}/{path.TrimStart( '/' )}";

    /// <summary>
    ///     If <paramref name="value" /> is not <c>null</c>, adds the query string <paramref name="key" /> and
    ///     <paramref name="value" /> to the end of the URI.
    /// </summary>
    /// <param name="uri">The URI to add the query string to.</param>
    /// <param name="key">The key of the query string to add.</param>
    /// <param name="value">The value of the query string to add.</param>
    /// <returns>The URI with the query string added.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string AddQueryStringIfValueNotNull( this string uri, string key, string? value ) =>
        value != null
            ? uri.AddQueryString( key, value )
            : uri;
}