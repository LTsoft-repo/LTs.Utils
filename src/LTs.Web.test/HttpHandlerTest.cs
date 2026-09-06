using System.Collections.Immutable;
using System.Net;
using System.Text;
using LTs.TestUtils.FluentAssertions;
using LTs.TestUtils.Loggers;
using LTs.TestUtils.Web;
using LTs.Web.Abstractions;
using LTs.Web.Authorization;
using LTs.Web.Mime;
using LTs.Web.test.Infrastructure;

namespace LTs.Web.test;

public class HttpHandlerTest : BaseTest
{
    public HttpHandlerTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region GetAsync( uri )
    [ Fact ]
    public async Task GetAsync_ValidParameters_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";
        //var content = """{"key": "value"}""";
        var content = @"{""key"": ""value""}";

        var httpClient = MockHttpClientFactory.CreateForGet( "https://example.com/api", content );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.GetAsync( hostUrl );

        // Assert
        requestResponse.Should().NotBeNull();
        requestResponse.StatusCode.Should().Be( HttpStatusCode.OK );
        await requestResponse.Should().HaveContentAsJsonAsync( content );
    }
    #endregion

    #region GetAsync( uri, parameters, headers, bodyContent, mediaType )
    [ Fact ]
    public async Task GetAsync_FullParameters_ValidParameters_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";

        // HttpClient
        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Get, "https://example.com/api", true );

        // HttpHandler
        var parameters = new Dictionary<string, string>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        }.ToImmutableDictionary();

        var headers = new Dictionary<string, string>
        {
            { "header1", "value1" },
            { "header2", "value2" }
        }.ToImmutableDictionary();

        //var bodyContent = """{"key": "value"}""";
        var bodyContent = @"{""key"": ""value""}";
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.GetAsync( hostUrl, parameters, headers, bodyContent, MediaType.ApplicationJson );

        // Assert
        requestResponse.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.OK )
                                              {
                                                  //Content = new StringContent( """{"key": "value"}""",
                                                  Content = new StringContent( @"{""key"": ""value""}",
                                                                               Encoding.UTF8,
                                                                               MediaType.ApplicationJson.ToMediaTypeString() )
                                              },
                                              parameters,
                                              headers );
    }

    [ Fact ]
    public async Task GetAsync_FullParameters_NoUrlParameters_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";

        // HttpClient
        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Get, "https://example.com/api", true );

        // HttpHandler
        var headers = new Dictionary<string, string>
        {
            { "header1", "value1" },
            { "header2", "value2" }
        }.ToImmutableDictionary();

        //var bodyContent = """{"key": "value"}""";
        var bodyContent = @"{""key"": ""value""}";
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse =
            await client.GetAsync( hostUrl, ImmutableDictionary<string, string>.Empty, headers, bodyContent, MediaType.ApplicationJson );

        // Assert
        requestResponse.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.OK )
                                              {
                                                  //Content = new StringContent( """{"key": "value"}""",
                                                  Content = new StringContent( @"{""key"": ""value""}",
                                                                               Encoding.UTF8,
                                                                               MediaType.ApplicationJson.ToMediaTypeString() )
                                              },
                                              ImmutableDictionary<string, string>.Empty,
                                              headers );
    }

    [ Fact ]
    public async Task GetAsync_FullParameters_NoHeaders_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";

        // HttpClient
        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Get, "https://example.com/api", true );

        // HttpHandler
        var parameters = new Dictionary<string, string>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        }.ToImmutableDictionary();

        //var bodyContent = """{"key": "value"}""";
        var bodyContent = @"{""key"": ""value""}";
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.GetAsync( hostUrl,
                                                     parameters,
                                                     ImmutableDictionary<string, string>.Empty,
                                                     bodyContent,
                                                     MediaType.ApplicationJson );

        // Assert
        requestResponse.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.OK )
                                              {
                                                  //Content = new StringContent( """{"key": "value"}""",
                                                  Content = new StringContent( @"{""key"": ""value""}",
                                                                               Encoding.UTF8,
                                                                               MediaType.ApplicationJson.ToMediaTypeString() )
                                              },
                                              parameters,
                                              ImmutableDictionary<string, string>.Empty );
    }

    [ Fact ]
    public async Task GetAsync_FullParameters_IncorrectUrl_GetsNotFound()
    {
        // Arrange
        var hostUrl = "https://example.com/api2";
        //var content = """{"key": "value"}""";
        var content = @"{""key"": ""value""}";

        var httpClient = MockHttpClientFactory.CreateForGet( "https://example.com/api", content );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.GetAsync( hostUrl,
                                                     ImmutableDictionary<string, string>.Empty,
                                                     ImmutableDictionary<string, string>.Empty,
                                                     "",
                                                     MediaType.None );

        // Assert
        requestResponse.Should().NotBeNull();
        requestResponse.StatusCode.Should().Be( HttpStatusCode.NotFound );
    }

    [ Fact ]
    public async Task GetAsync_FullParameters_NullUrl_ThrowsInvalidOperationException()
    {
        // Arrange
        //var content = """{"key": "value"}""";
        var content = @"{""key"": ""value""}";

        var httpClient = MockHttpClientFactory.CreateForGet( "https://example.com/api", content );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var act = () => client.GetAsync( null!,
                                         ImmutableDictionary<string, string>.Empty,
                                         ImmutableDictionary<string, string>.Empty,
                                         "",
                                         MediaType.None );

        // Assert
        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage( "An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set." );
    }
    #endregion

    #region PostAsync( uri, bodyContent, mediaType )
    [ Fact ]
    public async Task PostAsync_ValidParameters_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";
        var content = @"{""key"": ""value""}";

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Post, "https://example.com/api", true );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.PostTextAsync( hostUrl, content, MediaType.ApplicationJson );

        // Assert
        requestResponse.Should().NotBeNull();
        requestResponse.StatusCode.Should().Be( HttpStatusCode.OK );
        await requestResponse.Should().HaveContentAsJsonAsync( content );
    }
    #endregion

    #region PostAsync( uri, parameters, headers, bodyContent, mediaType )
    [ Fact ]
    public async Task PostAsync_FullParameters_ValidParameters_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";
        var content = @"{""key"": ""value""}";

        var parameters = new Dictionary<string, string>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        }.ToImmutableDictionary();

        var headers = new Dictionary<string, string>
        {
            { "header1", "value1" },
            { "header2", "value2" }
        }.ToImmutableDictionary();

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Post, "https://example.com/api", true );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.PostTextAsync( hostUrl, parameters, headers, content, MediaType.ApplicationJson );

        // Assert
        requestResponse.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.OK )
                                              {
                                                  Content = new StringContent( @"{""key"": ""value""}",
                                                                               Encoding.UTF8,
                                                                               MediaType.ApplicationJson.ToMediaTypeString() )
                                              },
                                              parameters,
                                              headers );
    }

    [ Fact ]
    public async Task PostAsync_FullParameters_NoUrlParameters_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";
        var content = @"{""key"": ""value""}";

        var headers = new Dictionary<string, string>
        {
            { "header1", "value1" },
            { "header2", "value2" }
        }.ToImmutableDictionary();

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Post, "https://example.com/api", true );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.PostTextAsync(
                                  hostUrl,
                                  ImmutableDictionary<string, string>.Empty,
                                  headers,
                                  content,
                                  MediaType.ApplicationJson );

        // Assert
        requestResponse.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.OK )
                                              {
                                                  Content = new StringContent( @"{""key"": ""value""}",
                                                                               Encoding.UTF8,
                                                                               MediaType.ApplicationJson.ToMediaTypeString() )
                                              },
                                              ImmutableDictionary<string, string>.Empty,
                                              headers );
    }

    [ Fact ]
    public async Task PostAsync_FullParameters_NoHeaders_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";
        var content = @"{""key"": ""value""}";

        var parameters = new Dictionary<string, string>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        }.ToImmutableDictionary();

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Post, "https://example.com/api", true );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.PostTextAsync(
                                  hostUrl,
                                  parameters,
                                  ImmutableDictionary<string, string>.Empty,
                                  content,
                                  MediaType.ApplicationJson );

        // Assert
        requestResponse.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.OK )
                                              {
                                                  Content = new StringContent( @"{""key"": ""value""}",
                                                                               Encoding.UTF8,
                                                                               MediaType.ApplicationJson.ToMediaTypeString() )
                                              },
                                              parameters,
                                              ImmutableDictionary<string, string>.Empty );
    }

    [ Fact ]
    public async Task PostAsync_FullParameters_IncorrectUrl_GetsNotFound()
    {
        // Arrange
        var hostUrl = "https://example.com/api2";
        var content = @"{""key"": ""value""}";

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Post, "https://example.com/api", false );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.PostTextAsync(
                                  hostUrl,
                                  ImmutableDictionary<string, string>.Empty,
                                  ImmutableDictionary<string, string>.Empty,
                                  content,
                                  MediaType.ApplicationJson );

        // Assert
        requestResponse.Should().NotBeNull();
        requestResponse.StatusCode.Should().Be( HttpStatusCode.NotFound );
    }

    [ Fact ]
    public async Task PostAsync_FullParameters_NullUrl_ThrowsInvalidOperationException()
    {
        // Arrange
        var content = @"{""key"": ""value""}";

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Post, "https://example.com/api", false );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var act = () => client.PostTextAsync(
            null!,
            ImmutableDictionary<string, string>.Empty,
            ImmutableDictionary<string, string>.Empty,
            content,
            MediaType.ApplicationJson );

        // Assert
        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage( "An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set." );
    }

    [ Fact ]
    public async Task PostAsync_NullData_ThrowsArgumentNullException()
    {
        // Arrange
        var hostUrl = "https://example.com/api";

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Post, "https://example.com/api", false );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var act = () => client.PostTextAsync(
            hostUrl,
            ImmutableDictionary<string, string>.Empty,
            ImmutableDictionary<string, string>.Empty,
            null!,
            MediaType.ApplicationJson );

        // Assert
        await act.Should()
                 .ThrowAsync<ArgumentNullException>()
                 .WithMessage( "Value cannot be null. (Parameter 'bodyContent')" );
    }

    [ Fact ]
    public async Task PostAsync_FullParameters_NoMediaType_ThrowsArgumentNullException()
    {
        // Arrange
        var hostUrl = "https://example.com/api";
        var content = @"{""key"": ""value""}";

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Post, "https://example.com/api", false );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var act = () => client.PostTextAsync(
            hostUrl,
            ImmutableDictionary<string, string>.Empty,
            ImmutableDictionary<string, string>.Empty,
            content,
            MediaType.None );

        // Assert
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage( "The media type must be defined (Parameter 'mediaType')" );
    }
    #endregion

    #region SendAsync( uri, httpMethod )
    [ Fact ]
    public async Task SendAsync_ValidParameters_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Delete, "https://example.com/api", false );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.SendAsync( hostUrl, HttpMethod.Delete );

        // Assert
        var statusCode = requestResponse.StatusCode;
        statusCode.Should().Be( HttpStatusCode.OK );
    }
    #endregion

    #region SendAsync( uri, httpMethod, parameters, headers, bodyContent, mediaType )
    [ Fact ]
    public async Task SendAsync_FullParameters_ValidParameters_Successes()
    {
        // Arrange
        var hostUrl = "https://example.com/api";
        var content = @"{""key"": ""value""}";

        var parameters = new Dictionary<string, string>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        }.ToImmutableDictionary();

        var headers = new Dictionary<string, string>
        {
            { "header1", "value1" },
            { "header2", "value2" }
        }.ToImmutableDictionary();

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Put, "https://example.com/api", true );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.SendAsync( hostUrl, HttpMethod.Put, parameters, headers, content, MediaType.ApplicationJson );

        // Assert
        requestResponse.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.OK )
                                              {
                                                  Content = new StringContent( @"{""key"": ""value""}",
                                                                               Encoding.UTF8,
                                                                               MediaType.ApplicationJson.ToMediaTypeString() )
                                              },
                                              parameters,
                                              headers );
    }

    [ Fact ]
    public async Task SendAsync_FullParameters_NoMediaType_ThrowsArgumentException()
    {
        // Arrange
        var hostUrl = "https://example.com/api";
        var content = @"{""key"": ""value""}";

        var httpClient = MockHttpClientFactory.CreateToEchoRequest( HttpMethod.Put, "https://example.com/api", false );
        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var act = () => client.SendAsync(
            hostUrl,
            HttpMethod.Put,
            ImmutableDictionary<string, string>.Empty,
            ImmutableDictionary<string, string>.Empty,
            content,
            MediaType.None );

        // Assert
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage( "The media type must be defined (Parameter 'mediaType')" );
    }
    #endregion

    #region Authorization
    [ Fact ]
    public async Task GetAccessTokenAsync_ValidParameters_Successes()
    {
        //Arrange
        var accessTokenUrl = "https://example.com/connect/token";
        var clientId = "someClientId";
        var clientSecret = "someClientSecret";
        var scopes = new[] { "someScope" };

        var httpClient = MockHttpClientFactory.CreateForIdentity(
            "https://example.com",
            clientId,
            clientSecret,
            scopes,
            "MyAccessToken",
            "MyIdToken" );

        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var requestResponse = await client.GetAccessTokenAsync( accessTokenUrl, GrantType.ClientCredentials, clientId, clientSecret, scopes.First() );

        // Assert
        requestResponse.Should().NotBeNull();
        requestResponse.Should().Be( "MyAccessToken" );
    }

    [ Fact ]
    public async Task GetAccessTokenAsync_CachedTokenValid_ReturnsCached()
    {
        //Arrange
        var accessTokenUrl = "https://example.com/connect/token";
        var clientId = "someClientId";
        var clientSecret = "someClientSecret";
        var scopes = new[] { "someScope" };

        var httpClient = MockHttpClientFactory.CreateForIdentity(
            "https://example.com",
            clientId,
            clientSecret,
            scopes,
            "MyAccessToken",
            "MyIdToken" );

        var cachedTokens = new AuthorizationData[]
        {
            new()
            {
                AccessTokenUrl = accessTokenUrl,
                GrantType = GrantType.ClientCredentials,
                ClientId = clientId,
                Secret = clientSecret,
                Scope = scopes.First(),
                AccessToken = "CachedAccessToken",
                ExpireAtUtc = DateTime.UtcNow.AddHours( 1 )
            }
        };

        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        var clientInstance = new TestHttpHandler( httpClient, cachedTokens, logger );
        IHttpHandler client = clientInstance;

        // Act
        var requestResponse = await client.GetAccessTokenAsync( accessTokenUrl, GrantType.ClientCredentials, clientId, clientSecret, scopes.First() );

        // Assert
        requestResponse.Should().NotBeNull();
        requestResponse.Should().Be( "CachedAccessToken" );
    }

    [ Fact ]
    public async Task GetAccessTokenAsync_CachedTokenExpired_ReturnsNewToken()
    {
        //Arrange
        var accessTokenUrl = "https://example.com/connect/token";
        var clientId = "someClientId";
        var clientSecret = "someClientSecret";
        var scopes = new[] { "someScope" };

        var httpClient = MockHttpClientFactory.CreateForIdentity(
            "https://example.com",
            clientId,
            clientSecret,
            scopes,
            "MyNewAccessToken",
            "MyIdToken" );

        var cachedTokens = new AuthorizationData[]
        {
            new()
            {
                AccessTokenUrl = accessTokenUrl,
                GrantType = GrantType.ClientCredentials,
                ClientId = clientId,
                Secret = clientSecret,
                Scope = scopes.First(),
                AccessToken = "CachedAccessToken",
                ExpireAtUtc = DateTime.UtcNow.AddMinutes( -10 )
            }
        };

        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        var clientInstance = new TestHttpHandler( httpClient, cachedTokens, logger );
        IHttpHandler client = clientInstance;

        // Act
        var requestResponse = await client.GetAccessTokenAsync( accessTokenUrl, GrantType.ClientCredentials, clientId, clientSecret, scopes.First() );

        // Assert
        requestResponse.Should().NotBeNull();
        requestResponse.Should().Be( "MyNewAccessToken" );
    }

    [ Fact ]
    public async Task GetAccessTokenAsync_CachedTokenNull_ReturnsNewToken()
    {
        //Arrange
        var accessTokenUrl = "https://example.com/connect/token";
        var clientId = "someClientId";
        var clientSecret = "someClientSecret";
        var scopes = new[] { "someScope" };

        var httpClient = MockHttpClientFactory.CreateForIdentity(
            "https://example.com",
            clientId,
            clientSecret,
            scopes,
            "MyNewAccessToken",
            "MyIdToken" );

        var cachedTokens = new AuthorizationData[]
        {
            new()
            {
                AccessTokenUrl = accessTokenUrl,
                GrantType = GrantType.ClientCredentials,
                ClientId = clientId,
                Secret = clientSecret,
                Scope = scopes.First(),
                AccessToken = null!,
                ExpireAtUtc = DateTime.UtcNow.AddHours( 1 )
            }
        };

        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        var clientInstance = new TestHttpHandler( httpClient, cachedTokens, logger );
        IHttpHandler client = clientInstance;

        // Act
        var requestResponse = await client.GetAccessTokenAsync( accessTokenUrl, GrantType.ClientCredentials, clientId, clientSecret, scopes.First() );

        // Assert
        requestResponse.Should().NotBeNull();
        requestResponse.Should().Be( "MyNewAccessToken" );
    }

    [ Theory ]
    [ InlineData( null,
                  "someClientId",
                  "someClientSecret",
                  "someScope",
                  "MyNewAccessToken",
                  "Error getting Access Token: An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set." ) ]
    [ InlineData( "https://example.com/connect/token",
                  null,
                  "someClientSecret",
                  "someScope",
                  "MyNewAccessToken",
                  "Error getting Access Token: 401 Unauthorized" ) ]
    [ InlineData( "https://example.com/connect/token",
                  "someClientId",
                  null,
                  "someScope",
                  "MyNewAccessToken",
                  "Error getting Access Token: 401 Unauthorized" ) ]
    [ InlineData( "https://example.com/connect/token",
                  "someClientId",
                  "someClientSecret",
                  null,
                  "MyNewAccessToken",
                  "Error getting Access Token: 401 Unauthorized" ) ]
    [ InlineData( "https://example.com/connect/token",
                  "someClientId",
                  "someClientSecret",
                  "someScope",
                  null,
                  "Error getting Access Token: The token is empty" ) ]
    public async Task GetAccessTokenAsync_InvalidParameter_ThrowsException( string? accessTokenUrl,
                                                                            string? clientId,
                                                                            string? clientSecret,
                                                                            string? scope,
                                                                            string? accessToken,
                                                                            string expectedExceptionMessage )
    {
        var scopes = new[] { scope };

        var httpClient = MockHttpClientFactory.CreateForIdentity(
            "https://example.com",
            clientId!,
            clientSecret!,
            scopes!,
            accessToken!,
            "MyIdToken" );

        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        var clientInstance = new TestHttpHandler( httpClient, logger );
        IHttpHandler client = clientInstance;

        // Act
        var act = async () => await client.GetAccessTokenAsync( accessTokenUrl!,
                                                                GrantType.ClientCredentials,
                                                                clientId!,
                                                                clientSecret!,
                                                                scopes.First()! );

        // Assert
        await act.Should().ThrowAsync<Exception>()
                 .WithMessage( expectedExceptionMessage );
    }

    [ Theory ]
    [ InlineData( "https://example.com/id",
                  "someClientId",
                  "someClientSecret",
                  "someScope",
                  "Error getting Access Token: 404 Not Found" ) ]
    [ InlineData( "https://example2.com/",
                  "someClientId",
                  "someClientSecret",
                  "someScope",
                  "Error getting Access Token: 404 Not Found" ) ]
    [ InlineData( "https://example.com/connect/token",
                  "someClientId",
                  "someClientSecret",
                  "anotherScope",
                  "Error getting Access Token: 401 Unauthorized" ) ]
    [ InlineData( "https://example.com/connect/token",
                  "someClientId",
                  "wrongSecret",
                  "someScope",
                  "Error getting Access Token: 401 Unauthorized" ) ]
    public async Task GetAccessTokenAsync_WrongParameters_ThrowsException(
        string accessTokenUrl,
        string clientId,
        string clientSecret,
        string scope,
        string expectedExceptionMessage )
    {
        //Arrange
        var scopes = new[] { "someScope" };

        var httpClient = MockHttpClientFactory.CreateForIdentity(
            "https://example.com",
            "someClientId",
            "someClientSecret",
            scopes,
            "MyAccessToken",
            "MyIdToken" );

        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var act = () => client.GetAccessTokenAsync( accessTokenUrl, GrantType.ClientCredentials, clientId, clientSecret, scope );

        // Assert
        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage( expectedExceptionMessage );
    }

    [ Fact ]
    public async Task GetAccessTokenAsync_WrongGrantType_ThrowsException()
    {
        //Arrange
        var accessTokenUrl = "https://example.com/connect/token";
        var clientId = "someClientId";
        var clientSecret = "someClientSecret";
        var scopes = new[] { "someScope" };

        var httpClient = MockHttpClientFactory.CreateForIdentity(
            "https://example.com",
            clientId,
            clientSecret,
            scopes,
            "MyAccessToken",
            "MyIdToken" );

        var logger = new TestLogger<TestHttpHandler>( TestOutput );
        IHttpHandler client = new TestHttpHandler( httpClient, logger );

        // Act
        var act = () => client.GetAccessTokenAsync( accessTokenUrl, GrantType.Password, clientId, clientSecret, scopes.First() );

        // Assert
        await act.Should()
                 .ThrowAsync<NotSupportedException>()
                 .WithMessage( "The grant type is not supported" );
    }
    #endregion
}