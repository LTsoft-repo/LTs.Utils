using System.Collections.Immutable;
using System.Net;
using System.Text;
using LTs.Web;
using LTs.Web.Authorization;
using LTs.Web.Configurations;
using LTs.Web.Mime;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace LTs.TestUtils.Web;

/// <summary>
///     Factory to create <see cref="HttpHandler" /> mocks.
/// </summary>
public static class MockHttpHandlerFactory
{
    /// <summary>
    ///     Creates a mock of <see cref="HttpHandler" /> to echo the GET and POST requests.
    /// </summary>
    /// <param name="configuration">Authorization configuration.</param>
    /// <param name="token">Value to return as Token.</param>
    /// <returns>A mocked <see cref="HttpHandler" /> with <c>GetAsync</c> and <c>PostJsonAsync</c> configured.</returns>
    public static Mock<HttpHandler> CreateToEchoRequest( AuthorizationConfiguration configuration, string token )
    {
        // Mock the IHttpHandler
        var mockHttpHandler = new Mock<HttpHandler>( It.IsAny<HttpClient>(), It.IsAny<ILogger<HttpHandler>>() );

        // Get.
        mockHttpHandler
            .Setup( x => x.GetAsync( It.IsAny<string>() ) )
            .ReturnsAsync( ( string uri )
                               => GenerateResponse( uri,
                                                    "GET",
                                                    ImmutableDictionary<string, string>.Empty,
                                                    ImmutableDictionary<string, string>.Empty,
                                                    "",
                                                    MediaType.None ) );

        mockHttpHandler
            .Setup( x => x.GetAsync(
                        It.IsAny<string>(),
                        It.IsAny<IImmutableDictionary<string, string>>(),
                        It.IsAny<IImmutableDictionary<string, string>>() ) )
            .ReturnsAsync( ( string uri,
                             IImmutableDictionary<string, string> parameters,
                             IImmutableDictionary<string, string> headers )
                               => GenerateResponse( uri, "GET", parameters, headers, "", MediaType.None ) );

        mockHttpHandler
            .Setup( x => x.GetAsync(
                        It.IsAny<string>(),
                        It.IsAny<IImmutableDictionary<string, string>>(),
                        It.IsAny<IImmutableDictionary<string, string>>(),
                        It.IsAny<string>(),
                        It.IsAny<MediaType>() ) )
            .ReturnsAsync( ( string uri,
                             IImmutableDictionary<string, string> parameters,
                             IImmutableDictionary<string, string> headers,
                             string bodyContent,
                             MediaType mediaType )
                               => GenerateResponse( uri, "GET", parameters, headers, bodyContent, mediaType ) );

        // Post.
        mockHttpHandler
            .Setup( x => x.PostTextAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<MediaType>() ) )
            .ReturnsAsync( ( string uri,
                             string bodyContent,
                             MediaType mediaType )
                               => GenerateResponse( uri,
                                                    "POST",
                                                    ImmutableDictionary<string, string>.Empty,
                                                    ImmutableDictionary<string, string>.Empty,
                                                    bodyContent,
                                                    mediaType ) );

        mockHttpHandler
            .Setup( x => x.PostTextAsync(
                        It.IsAny<string>(),
                        It.IsAny<IImmutableDictionary<string, string>>(),
                        It.IsAny<IImmutableDictionary<string, string>>(),
                        It.IsAny<string>(),
                        It.IsAny<MediaType>() ) )
            .ReturnsAsync( ( string uri,
                             IImmutableDictionary<string, string> parameters,
                             IImmutableDictionary<string, string> headers,
                             string bodyContent,
                             MediaType mediaType )
                               => GenerateResponse( uri, "POST", parameters, headers, bodyContent, mediaType ) );

        mockHttpHandler
            .Setup( x => x.GetAccessTokenAsync( configuration.AccessTokenUrl,
                                                GrantType.ClientCredentials,
                                                configuration.ClientId,
                                                configuration.ClientSecret,
                                                configuration.Scope ) )
            .ReturnsAsync( token );

        return mockHttpHandler;
    }

    private static HttpResponseMessage GenerateResponse( string uri,
                                                         string httpMethod,
                                                         IImmutableDictionary<string, string> parameters,
                                                         IImmutableDictionary<string, string> headers,
                                                         string bodyContent,
                                                         MediaType mediaType )
    {
        var response = new MockHttpHandlerFactoryEchoResponseContent
        {
            Uri = uri,
            Method = httpMethod,
            Parameters = parameters,
            Headers = headers,
            Body = bodyContent,
            MediaType = mediaType
        };

        return new HttpResponseMessage( HttpStatusCode.OK )
        {
            Content = new StringContent( JsonConvert.SerializeObject( response ),
                                         Encoding.UTF8,
                                         MediaType.ApplicationJson.ToMediaTypeString() )
        };
    }
}