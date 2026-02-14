using System.Collections.Immutable;
using System.Text;
using IdentityModel.Client;
using LTs.Utils.Extensions.Web;
using LTs.Web.Abstractions;
using LTs.Web.Authorization;
using LTs.Web.Mime;
using Microsoft.Extensions.Logging;

namespace LTs.Web;

/// <summary>
///     <see cref="IHttpHandler" /> implementation using a <see cref="HttpClient" /> to send requests.
/// </summary>
[ UsedImplicitly ]
public class HttpHandler : IHttpHandler
{
    /// <summary>
    ///     The <see cref="HttpClient" /> used to send requests.
    /// </summary>
    [ UsedImplicitly ]
    protected HttpClient HttpClient { get; init; }

    /// <summary>
    ///     Cache for access tokens.
    /// </summary>
    [ UsedImplicitly ]
    protected static readonly List<AuthorizationData> AccessTokenCache = [ ];

    private readonly ILogger logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="HttpHandler" /> class.
    /// </summary>
    /// <param name="httpClient">Instance of <see cref="HttpClient" />.</param>
    /// <param name="logger">Logger instance.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public HttpHandler( HttpClient httpClient, ILogger<HttpHandler> logger )
        : this( httpClient, (ILogger)logger ) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="HttpHandler" /> class.
    /// </summary>
    /// <param name="httpClient">Instance of <see cref="HttpClient" />.</param>
    /// <param name="logger">Logger instance.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    protected HttpHandler( HttpClient httpClient, ILogger logger )
    {
        HttpClient = httpClient;
        this.logger = logger;
    }

    #region Get
    /// <inheritdoc />
    public virtual async Task<HttpResponseMessage> GetAsync( string uri )
        => await GetAsync( uri, ImmutableDictionary<string, string>.Empty, ImmutableDictionary<string, string>.Empty, "", MediaType.None );

    /// <inheritdoc />
    public virtual async Task<HttpResponseMessage> GetAsync( string uri,
                                                             IImmutableDictionary<string, string> parameters,
                                                             IImmutableDictionary<string, string> headers )
        => await GetAsync( uri, parameters, headers, "", MediaType.None );

    /// <inheritdoc />
    public virtual async Task<HttpResponseMessage> GetAsync( string uri,
                                                             IImmutableDictionary<string, string> parameters,
                                                             IImmutableDictionary<string, string> headers,
                                                             string bodyContent,
                                                             MediaType mediaType )
        => await SendAsync( uri, HttpMethod.Get, parameters, headers, bodyContent, mediaType );
    #endregion

    #region Post
    /// <inheritdoc />
    public virtual async Task<HttpResponseMessage> PostTextAsync( string uri, string bodyContent, MediaType mediaType )
        => await PostTextAsync(
               uri,
               ImmutableDictionary<string, string>.Empty,
               ImmutableDictionary<string, string>.Empty,
               bodyContent,
               mediaType );

    /// <inheritdoc />
    public virtual async Task<HttpResponseMessage> PostTextAsync( string uri,
                                                                  IImmutableDictionary<string, string> parameters,
                                                                  IImmutableDictionary<string, string> headers,
                                                                  string bodyContent,
                                                                  MediaType mediaType )
        => string.IsNullOrEmpty( bodyContent )
               ? throw new ArgumentNullException( nameof( bodyContent ) )
               : mediaType == MediaType.None
                   ? throw new ArgumentException( "The media type must be defined", nameof( mediaType ) )
                   : await SendAsync( uri, HttpMethod.Post, parameters, headers, bodyContent, mediaType );
    #endregion

    #region Authorization
    /// <inheritdoc />
    public virtual Task<string> GetAccessTokenAsync( string accessTokenUrl,
                                                     GrantType grantType,
                                                     string clientId,
                                                     string secret,
                                                     string scope )
        => GetAccessTokenAsync( accessTokenUrl, grantType, clientId, secret, scope, false );

    /// <inheritdoc />
    public virtual async Task<string> GetAccessTokenAsync( string accessTokenUrl,
                                                           GrantType grantType,
                                                           string clientId,
                                                           string secret,
                                                           string scope,
                                                           bool forceRefresh )
    {
        logger.LogDebug( "Getting Access Token from URL: {AccessTokenUrl}", accessTokenUrl );

        if( grantType != GrantType.ClientCredentials )
        {
            throw new NotSupportedException( "The grant type is not supported" );
        }

        var authorizationData = new AuthorizationData
        {
            AccessTokenUrl = accessTokenUrl,
            GrantType = grantType,
            ClientId = clientId,
            Secret = secret,
            Scope = scope
        };

        // Looks for the token in the cache.
        AuthorizationData? cachedAuthorization = null;

        try
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            cachedAuthorization = AccessTokenCache.FirstOrDefault( a => a != null && a.Equals( authorizationData ) );
        }
        catch( Exception ex )
        {
            logger.LogWarning( ex, "Exception while searching for cached access token. Will continue as if no cached token exists." );
        }

        // If the token is in the cache, and it is not expired, return it.
        if( cachedAuthorization != null &&
            !string.IsNullOrWhiteSpace( cachedAuthorization.AccessToken ) &&
            cachedAuthorization.ExpireAtUtc > DateTime.UtcNow.AddMilliseconds( -500 ) )
        {
            logger.LogDebug( "Using cached Access Token from URL: {AccessTokenUrl}", accessTokenUrl );

            return cachedAuthorization.AccessToken;
        }

        // If the token is in the cache, and it is expired, remove it from the cache.
        if( cachedAuthorization != null )
        {
            AccessTokenCache.Remove( cachedAuthorization );
        }

        logger.LogDebug( "Requesting new Access Token from URL: {AccessTokenUrl}", accessTokenUrl );

        var tokenResponse = await HttpClient.RequestClientCredentialsTokenAsync(
                                new ClientCredentialsTokenRequest
                                {
                                    Address = accessTokenUrl,
                                    GrantType = grantType.ToIdentityString(),
                                    ClientId = clientId,
                                    ClientSecret = secret,
                                    Scope = scope
                                } );

        if( tokenResponse.IsError )
        {
            var errorDescription = tokenResponse.ErrorDescription;

            if( string.IsNullOrWhiteSpace( errorDescription ) )
            {
                var statusCode = (int?)tokenResponse.HttpResponse?.StatusCode;
                errorDescription = $"{statusCode} {tokenResponse.HttpErrorReason}";

                if( string.IsNullOrWhiteSpace( errorDescription ) )
                {
                    errorDescription = tokenResponse.Error;
                }
            }

            logger.LogError( "Error getting Access Token from URL: {AccessTokenUrl}. Error: {ErrorDescription}",
                             accessTokenUrl,
                             errorDescription );

            throw new InvalidOperationException( $"Error getting Access Token: {errorDescription}" );
        }

        if( string.IsNullOrEmpty( tokenResponse.AccessToken ) )
        {
            logger.LogError( "Error getting Access Token from URL: {AccessTokenUrl}. Error: The token is empty",
                             accessTokenUrl );

            throw new InvalidOperationException( "Error getting Access Token: The token is empty" );
        }

        AccessTokenCache.Add(
            new AuthorizationData
            {
                AccessTokenUrl = accessTokenUrl,
                ClientId = clientId,
                Secret = secret,
                Scope = scope,
                AccessToken = tokenResponse.AccessToken,
                ExpireAtUtc = DateTime.UtcNow.AddMilliseconds( tokenResponse.ExpiresIn )
            } );

        return tokenResponse.AccessToken;
    }
    #endregion

    /// <summary>
    ///     Sends a <paramref name="httpMethod" /> request to the specified Uri <paramref name="uri" /> as an asynchronous
    ///     operation.
    /// </summary>
    /// <param name="uri">The Uri the request is sent to.</param>
    /// <param name="httpMethod">The HTTP method.</param>
    /// <param name="parameters">The parameters to send with the request (query string keys-values).</param>
    /// <param name="headers">The headers to send with the request.</param>
    /// <param name="bodyContent">The body content to send with the request.</param>
    /// <param name="mediaType">The media type of the body content.</param>
    /// <returns>The response to the request as an asynchronous operation.</returns>
    [ UsedImplicitly ]
    protected virtual async Task<HttpResponseMessage> SendAsync( string uri,
                                                                 HttpMethod httpMethod,
                                                                 IImmutableDictionary<string, string> parameters,
                                                                 IImmutableDictionary<string, string> headers,
                                                                 string bodyContent,
                                                                 MediaType mediaType )
    {
        logger.LogDebug( "Sending request with MediaType {MediaType}: {httpMethod} {Uri}", mediaType, httpMethod, uri );

        var hasBodyContent = !string.IsNullOrEmpty( bodyContent );

        // Parameters (QueryString)
        foreach( var parameter in parameters )
        {
            uri = uri.AddQueryStringIfValueNotNull( parameter.Key, parameter.Value );
        }

        // Request
        var requestMessage = !hasBodyContent
                                 ? new HttpRequestMessage( httpMethod, uri )
                                 : new HttpRequestMessage( httpMethod, uri )
                                 {
                                     Content = new StringContent( bodyContent, Encoding.UTF8, mediaType.ToMediaTypeString() )
                                 };

        // Headers
        foreach( var header in headers )
        {
            requestMessage.Headers.Add( header.Key, header.Value );
        }

        return await HttpClient.SendAsync( requestMessage );
    }
}