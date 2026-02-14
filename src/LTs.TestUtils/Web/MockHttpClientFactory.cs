using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using RichardSzalay.MockHttp;
using LTs.Utils.Extensions.Web;

namespace LTs.TestUtils.Web;

/// <summary>
///     Factory to create HttpClient mocks.
/// </summary>
public static class MockHttpClientFactory
{
    /// <summary>
    ///     Creates a HttpClient that returns <paramref name="contentToReturn" /> when a GET request is made to
    ///     <paramref name="expectedUrl" />.
    /// </summary>
    /// <param name="expectedUrl">URL that the HttpClient is expected to receive a GET request.</param>
    /// <param name="contentToReturn">Content to be returned when the expected URL is requested.</param>
    /// <returns>A mocked HttpClient that returns the given content.</returns>
    public static HttpClient CreateForGet( string expectedUrl, string contentToReturn )
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When( "*" )
                .Respond( request =>
                    {
                        if( request.RequestUri != new Uri( expectedUrl ) ||
                            request.Method != HttpMethod.Get )
                        {
                            return new HttpResponseMessage( HttpStatusCode.NotFound );
                        }

                        var encoding = Encoding.UTF8;
                        var mediaType = "application/json";

                        var response = new HttpResponseMessage( HttpStatusCode.OK )
                        {
                            Content = new StringContent( contentToReturn, encoding, mediaType )
                        };

                        foreach( var header in request.Headers )
                        {
                            response.Headers.Add( header.Key, header.Value );
                        }

                        return response;
                    } );

        var httpClient = new HttpClient( mockHttp );

        return httpClient;
    }

    /// <summary>
    ///     Creates a HttpClient that returns the received request when a request is made to
    ///     <paramref name="expectedUrl" />.
    ///     The HTTP method the client will accept is defined by <paramref name="expectedHttpMethod" />.
    ///     <para>If <paramref name="echoBody" /> is <c>true</c>, it will echo the Body too and validate it has content type.</para>
    /// </summary>
    /// <param name="expectedHttpMethod">HTTP method that the HttpClient is expected to receive.</param>
    /// <param name="expectedUrl">URL that the HttpClient is expected to receive a request.</param>
    /// <param name="echoBody">Send the body content back in the response?</param>
    /// <returns>A mocked HttpClient that returns the given content.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static HttpClient CreateToEchoRequest( HttpMethod expectedHttpMethod, string expectedUrl, bool echoBody )
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When( "*" )
                .Respond( async request =>
                    {
                        // Request URL validation.
                        var requestUrl = request.RequestUri?.AbsoluteUri ?? "";

                        if( !string.IsNullOrEmpty( request.RequestUri?.Query ) )
                        {
                            requestUrl = request.RequestUri?.AbsoluteUri.Replace( request.RequestUri.Query, "" ) ?? "";
                        }

                        // Is expected URL?
                        if( string.Compare( requestUrl, expectedUrl, StringComparison.OrdinalIgnoreCase ) != 0 )
                        {
                            return new HttpResponseMessage( HttpStatusCode.NotFound );
                        }

                        // Is expected HTTP method?
                        if( request.Method != expectedHttpMethod )
                        {
                            return new HttpResponseMessage( HttpStatusCode.NotFound );
                        }

                        var response = new HttpResponseMessage( HttpStatusCode.OK );

                        if( echoBody )
                        {
                            // Has Content?
                            if( request.Content == null )
                            {
                                return new HttpResponseMessage( HttpStatusCode.BadRequest );
                            }

                            // Content.
                            var data = await request.Content!.ReadAsStringAsync();

                            var encoding = Encoding.Default;
                            var contentType = request.Content.Headers.ContentType;

                            encoding = contentType switch
                            {
                                null => throw new InvalidOperationException( "Content type is null" ),

                                { CharSet: not null } => Encoding.GetEncoding( contentType.CharSet ),

                                _ => encoding
                            };

                            response = new HttpResponseMessage( HttpStatusCode.OK )
                            {
                                Content = new StringContent( data,
                                                             encoding,
                                                             contentType.MediaType ?? "" )
                            };
                        }

                        // Parameters.
                        var parameters = request.RequestUri?.Query
                                                .TrimStart( '?' )
                                                .Split( '&' ) ?? Array.Empty<string>();

                        response.Headers.Add( "X-Parameters", parameters );

                        // Headers.
                        foreach( var header in request.Headers )
                        {
                            response.Headers.Add( header.Key, header.Value );
                        }

                        return response;
                    } );

        var httpClient = new HttpClient( mockHttp );

        return httpClient;
    }

    /// <summary>
    ///     Creates a mock of <see cref="HttpClient" /> that simulates an Identity Server to get an access token.
    /// </summary>
    /// <param name="baseUrl">Base URL where requests are expected.</param>
    /// <param name="clientId">Expected Client ID.</param>
    /// <param name="clientSecret">Expected Client Secret.</param>
    /// <param name="scopes">Expected scopes.</param>
    /// <param name="accessToken">Access Token to return.</param>
    /// <param name="idToken">ID Token to return.</param>
    /// <returns>The mocked <see cref="HttpClient" />.</returns>
    public static HttpClient CreateForIdentity(
        string baseUrl,
        string clientId,
        string clientSecret,
        IEnumerable<string> scopes,
        string accessToken,
        string idToken )
    {
        var allScopes = new[]
        {
            "openid",
            "profile",
            "email",
            "address",
            "roles",
            "offline_access"
        }.Concat( scopes ).ToHashSet();

        string GetDiscoveryDocumentUrl( string url )
        {
            var scopeList = string.Join( ", ",
                                         allScopes
                                             .Select( s => $"\"{s}\"" ) );

            // ReSharper disable StringLiteralTypo
            var disco = $@"
                {{
                    ""issuer"": ""{url}"",
                    ""jwks_uri"": ""{url}/.well-known/openid-configuration/jwks"",
                    ""authorization_endpoint"": ""{url}/connect/authorize"",
                    ""token_endpoint"": ""{url}/connect/token"",
                    ""userinfo_endpoint"": ""{url}/connect/userinfo"",
                    ""end_session_endpoint"": ""{url}/connect/endsession"",
                    ""check_session_iframe"": ""{url}/connect/checksession"",
                    ""revocation_endpoint"": ""{url}/connect/revocation"",
                    ""introspection_endpoint"": ""{url}/connect/introspect"",
                    ""frontchannel_logout_supported"": true,
                    ""frontchannel_logout_session_supported"": true,
                    ""backchannel_logout_supported"": true,
                    ""backchannel_logout_session_supported"": true,
                    ""scopes_supported"": [{scopeList}],
                    ""claims_supported"": [
                        ""sub"",
                        ""name"",
                        ""family_name"",
                        ""given_name"",
                        ""nickname"",
                        ""preferred_username"",
                        ""email"",
                        ""email_verified"",
                        ""role""
                    ],
                    ""grant_types_supported"": [
                        ""authorization_code"",
                        ""client_credentials"",
                        ""refresh_token"",
                        ""implicit"",
                        ""password""
                    ],
                    ""response_types_supported"": [
                        ""code"",
                        ""token"",
                        ""id_token"",
                        ""id_token token"",
                        ""code id_token"",
                        ""code token"",
                        ""code id_token token""
                    ],
                    ""response_modes_supported"": [
                        ""form_post"",
                        ""query"",
                        ""fragment""
                    ],
                    ""token_endpoint_auth_methods_supported"": [
                        ""client_secret_basic"",
                        ""client_secret_post""
                    ],
                    ""id_token_signing_alg_values_supported"": [
                        ""RS256""
                    ],
                    ""subject_types_supported"": [
                        ""public""
                    ],
                    ""code_challenge_methods_supported"": [
                        ""plain"",
                        ""S256""
                    ],
                    ""request_parameter_supported"": true
                }}";
            // ReSharper restore StringLiteralTypo

            return disco;
        }

        string GetJwksResponse()
        {
            // ReSharper disable StringLiteralTypo
            return @"
            {
                ""keys"": [
                    {
                        ""kty"": ""RSA"",
                        ""use"": ""sig"",
                        ""kid"": ""7BB3D507D1022F0FB1F60E73B5A88C1E"",
                        ""e"": ""AQAB"",
                        ""n"": ""91u6LnCneK4kgQldXRheJ8rzje2PdBAMOCQvsjf4702zohqc7UTfiaPzVhUTTE3vS8xG3fkUkHAJ6cGsBzXcQtWEawWxkxUR3mUzlNWQnAyyhqvbdnUL9OduEYoEXWvn2Y3n1MJjatZvj7NpjjsgXltoPuV8qEdV56cQ-Z-gbwlipTxL163r2MpvbTu-9nVRnZO1ijxC4BBZ-AnPv_kVTWc6QizAqElCTHxrLR1J01hwW-M2D1XhwO5kIx8c0QwSX1takwYLyKY4pbr-cy2SFZT1qtr_gkRGsXIHiVovciAl8qUm888gwWcwfUAc_kTEgiFBDuDEG4fXlZj7YJnU_Q"",
                        ""alg"": ""RS256""
                    }
                ]
            }";
            // ReSharper restore StringLiteralTypo
        }

        string GetTokenResponse()
        {
            // ReSharper disable StringLiteralTypo
            return $@"
            {{
                ""access_token"": ""{accessToken}"",
                ""token_type"": ""Bearer"",
                ""expires_in"": 3600,
                ""refresh_token"": ""eJzrW9cuI..."",
                ""id_token"": ""{idToken}""
            }}";
            // ReSharper restore StringLiteralTypo
        }

        var encoding = Encoding.UTF8;
        var mediaType = "application/json";

        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When( "*" )
                .Respond( async request =>
                    {
                        var requestUrl = request.RequestUri?.AbsoluteUri ?? string.Empty;

                        if( !requestUrl.StartsWith( baseUrl ) )
                        {
                            return new HttpResponseMessage( HttpStatusCode.NotFound );
                        }

                        switch( requestUrl )
                        {
                            // OpenID Connect Discovery Document
                            case var url when url == $"{baseUrl}/.well-known/openid-configuration":
                                var discoveryDocumentUrl = GetDiscoveryDocumentUrl( baseUrl );

                                return new HttpResponseMessage( HttpStatusCode.OK )
                                {
                                    Content = new StringContent( discoveryDocumentUrl, encoding, mediaType )
                                };

                            // JSON Web Key Set
                            case var url when url == $"{baseUrl}/.well-known/openid-configuration/jwks":

                                return new HttpResponseMessage( HttpStatusCode.OK )
                                {
                                    Content = new StringContent( GetJwksResponse(), encoding, mediaType )
                                };

                            // Token endpoint
                            case var url when url == $"{baseUrl}/connect/token":

                                // Checks the Http Method.
                                if( request.Method != HttpMethod.Post &&
                                    request.Method != HttpMethod.Get )
                                {
                                    return GetNotFoundResponse();
                                }

                                // Checks the Content.
                                if( request.Content == null )
                                {
                                    return GetUnauthorizedResponse();
                                }

                                var authHeaders = request.Headers.Authorization;
                                var formContent = await request.Content.ReadFormAsync();
                                var credentials = GetClientIdAndSecret( authHeaders, formContent );

                                // Checks the Authorization Header.
                                if( credentials.ClientId is null || credentials.ClientSecret is null )
                                {
                                    return GetUnauthorizedResponse();
                                }

                                var receivedClientId = credentials.ClientId;
                                var receivedClientSecret = credentials.ClientSecret;

                                // Checks the Client ID and Client Secret.

                                if( receivedClientId != clientId ||
                                    receivedClientSecret != clientSecret )
                                {
                                    return GetUnauthorizedResponse();
                                }

                                // Gets the scope.
                                _ = formContent.TryGetValue( "scope", out var formScopes );

                                if( formScopes == null )
                                {
                                    return GetUnauthorizedResponse();
                                }

                                var receivedScopes = formScopes.Split( ' ' ).ToHashSet();

                                if( receivedScopes.Any( scope => !allScopes.Contains( scope ) ) )
                                {
                                    return GetUnauthorizedResponse();
                                }

                                var tokenResponseMessage = new HttpResponseMessage( HttpStatusCode.OK )
                                {
                                    Content = new StringContent( GetTokenResponse(), encoding, mediaType )
                                };

                                return tokenResponseMessage;

                            default:
                                return GetNotFoundResponse();
                        }
                    } );

        var httpClient = new HttpClient( mockHttp );

        return httpClient;
    }

    /// <summary>
    ///     Gets the Client ID and Client Secret from the Authorization Header or Form Content.
    /// </summary>
    /// <param name="authorizationHeader">Authorization Header.</param>
    /// <param name="formContent">Form Content.</param>
    /// <returns>A tuple with the Client ID and Client Secret.</returns>
    private static (string? ClientId, string? ClientSecret) GetClientIdAndSecret(
        AuthenticationHeaderValue? authorizationHeader,
        IImmutableDictionary<string, string>? formContent )
    {
        // Tries to get the Authorization Header.
        var authHeader = authorizationHeader?.Parameter;

        string? clientId;
        string? clientSecret;

        if( authHeader != null )
        {
            var credentials = Encoding.UTF8.GetString( Convert.FromBase64String( authHeader ) )
                                      .Split( ':' );

            if( credentials.Length > 1 )
            {
                clientId = credentials[ 0 ];
                clientSecret = credentials[ 1 ];

                // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if( clientId is not null && clientSecret is not null )
                {
                    return ( clientId, clientSecret );
                }
                // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            }
        }

        // Tries to get them from the request body.
        if( formContent == null )
        {
            return ( null, null );
        }

        _ = formContent.TryGetValue( "client_id", out clientId );
        _ = formContent.TryGetValue( "client_secret", out clientSecret );

        return ( clientId, clientSecret );
    }

    /// <summary>
    ///     Generates an unauthorized response.
    /// </summary>
    /// <returns></returns>
    private static HttpResponseMessage GetUnauthorizedResponse()
    {
        var encoding = Encoding.UTF8;
        var mediaType = "application/json";

        return new HttpResponseMessage( HttpStatusCode.Unauthorized )
        {
            Content = new StringContent( @"{ ""error"": ""invalid_request"" }", encoding, mediaType )
        };
    }

    /// <summary>
    ///     Generates a not found response.
    /// </summary>
    /// <returns></returns>
    private static HttpResponseMessage GetNotFoundResponse()
        => new( HttpStatusCode.NotFound );
}