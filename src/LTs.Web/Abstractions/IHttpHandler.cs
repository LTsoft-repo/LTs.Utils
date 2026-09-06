using System.Collections.Immutable;
using LTs.Web.Authorization;
using LTs.Web.Mime;

namespace LTs.Web.Abstractions;

/// <summary>
///     Represents an HTTP handler (client).
/// </summary>
public interface IHttpHandler
{
    #region Get
    /// <summary>
    ///     Sends a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <returns>The response to the GET request as an asynchronous operation.</returns>
    Task<HttpResponseMessage> GetAsync( string uri );

    /// <summary>
    ///     Sends a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <param name="parameters">The parameters to send with the request (query string keys-values).</param>
    /// <param name="headers">The headers to send with the request.</param>
    /// <returns>The response to the GET request as an asynchronous operation.</returns>
    Task<HttpResponseMessage> GetAsync( string uri, IImmutableDictionary<string, string> parameters, IImmutableDictionary<string, string> headers );

    /// <summary>
    ///     Sends a GET request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <param name="parameters">The parameters to send with the request (query string keys-values).</param>
    /// <param name="headers">The headers to send with the request.</param>
    /// <param name="bodyContent">The body content to send with the request.</param>
    /// <param name="mediaType">The media type of the body content.</param>
    /// <returns>The response to the GET request as an asynchronous operation.</returns>
    Task<HttpResponseMessage> GetAsync(
        string uri,
        IImmutableDictionary<string, string> parameters,
        IImmutableDictionary<string, string> headers,
        string bodyContent,
        MediaType mediaType );
    #endregion

    #region Post
    /// <summary>
    ///     Sends a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <param name="bodyContent">The text data to send with the request.</param>
    /// <param name="mediaType">The media type of the text data.</param>
    /// <returns>The response to the POST request as an asynchronous operation.</returns>
    Task<HttpResponseMessage> PostTextAsync( string uri, string bodyContent, MediaType mediaType );

    /// <summary>
    ///     Sends a POST request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <param name="parameters">The parameters to send with the request (query string keys-values).</param>
    /// <param name="headers">The headers to send with the request.</param>
    /// <param name="bodyContent">The body content to send with the request.</param>
    /// <param name="mediaType">The media type of the body content.</param>
    /// <returns>The response to the POST request as an asynchronous operation.</returns>
    Task<HttpResponseMessage> PostTextAsync(
        string uri,
        IImmutableDictionary<string, string> parameters,
        IImmutableDictionary<string, string> headers,
        string bodyContent,
        MediaType mediaType );
    #endregion

    #region Send
    /// <summary>
    ///     Sends an HTTP request with the specified <paramref name="httpMethod" /> to the specified Uri as an asynchronous
    ///     operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <param name="httpMethod">The HTTP method.</param>
    /// <returns>The response to the request as an asynchronous operation.</returns>
    Task<HttpResponseMessage> SendAsync( string uri, HttpMethod httpMethod );

    /// <summary>
    ///     Sends an HTTP request with the specified <paramref name="httpMethod" /> to the specified Uri as an asynchronous
    ///     operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <param name="httpMethod">The HTTP method.</param>
    /// <param name="parameters">The parameters to send with the request (query string keys-values).</param>
    /// <param name="headers">The headers to send with the request.</param>
    /// <returns>The response to the request as an asynchronous operation.</returns>
    Task<HttpResponseMessage> SendAsync( string uri,
                                         HttpMethod httpMethod,
                                         IImmutableDictionary<string, string> parameters,
                                         IImmutableDictionary<string, string> headers );

    /// <summary>
    ///     Sends an HTTP request with the specified <paramref name="httpMethod" /> to the specified Uri as an asynchronous
    ///     operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <param name="httpMethod">The HTTP method.</param>
    /// <param name="parameters">The parameters to send with the request (query string keys-values).</param>
    /// <param name="headers">The headers to send with the request.</param>
    /// <param name="bodyContent">The body content to send with the request.</param>
    /// <param name="mediaType">The media type of the body content.</param>
    /// <returns>The response to the request as an asynchronous operation.</returns>
    Task<HttpResponseMessage> SendAsync( string uri,
                                         HttpMethod httpMethod,
                                         IImmutableDictionary<string, string> parameters,
                                         IImmutableDictionary<string, string> headers,
                                         string bodyContent,
                                         MediaType mediaType );
    #endregion

    #region Authorization
    /// <summary>
    ///     Gets the access token asynchronously from <paramref name="accessTokenUrl" /> using the specified
    ///     <paramref name="grantType" />,<paramref name="clientId" />, <paramref name="secret" /> and
    ///     <paramref name="scope" />.
    /// </summary>
    /// <param name="accessTokenUrl">The URL to get the access token.</param>
    /// <param name="grantType">The grant type to use.</param>
    /// <param name="clientId">The Client ID.</param>
    /// <param name="secret">The Client Secret.</param>
    /// <param name="scope">The scope the access token will be issued with.</param>
    /// <returns>The access token as an asynchronous operation.</returns>
    Task<string> GetAccessTokenAsync( string accessTokenUrl, GrantType grantType, string clientId, string secret, string scope );

    /// <summary>
    ///     Gets the access token asynchronously from <paramref name="accessTokenUrl" /> using the specified
    ///     <paramref name="grantType" />,<paramref name="clientId" />, <paramref name="secret" /> and
    ///     <paramref name="scope" />.
    /// </summary>
    /// <param name="accessTokenUrl">The URL to get the access token.</param>
    /// <param name="grantType">The grant type to use.</param>
    /// <param name="clientId">The Client ID.</param>
    /// <param name="secret">The Client Secret.</param>
    /// <param name="scope">The scope the access token will be issued with.</param>
    /// <param name="forceRefresh">If <c>true</c> forces the refresh of the access token.</param>
    /// <returns>The access token as an asynchronous operation.</returns>
    [ UsedImplicitly ]
    Task<string> GetAccessTokenAsync( string accessTokenUrl, GrantType grantType, string clientId, string secret, string scope, bool forceRefresh );
    #endregion
}