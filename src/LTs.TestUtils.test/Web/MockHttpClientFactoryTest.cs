using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using LTs.TestUtils.Web;
using LTs.Web.Extensions;

namespace LTs.TestUtils.test.Web;

public class MockHttpClientFactoryTest
{
    #region CreateForGet
    [ Fact ]
    public async Task CreateForGet_WithCorrectUrl_ReturnsOk()
    {
        // Arrange
        var expectedUrl = "https://example.com/api/data";
        //var contentToReturn = """{"message": "Hello, World!"}""";
        var contentToReturn = @"{""message"": ""Hello, World!""}";

        // Act
        var httpClient = MockHttpClientFactory.CreateForGet( expectedUrl, contentToReturn );

        var response = await httpClient.GetAsync( expectedUrl );
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.OK );
        responseBody.Should().Be( contentToReturn );
    }

    [ Fact ]
    public async Task CreateForGet_Should_Return_NotFound_When_UnexpectedUrl()
    {
        // Arrange
        var expectedUrl = "https://example.com/api/data";
        //var contentToReturn = """{"message": "Hello, World!"}""";
        var contentToReturn = @"{""message"": ""Hello, World!""}";

        // Act
        var httpClient = MockHttpClientFactory.CreateForGet( expectedUrl, contentToReturn );

        var response = await httpClient.GetAsync( "https://example.com/api/other" );

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.NotFound );
    }

    [ Fact ]
    public async Task CreateForGet_WithIncorrectHttpMethod_ReturnsNotFound()
    {
        // Arrange
        var expectedUrl = "https://example.com/api/data";
        var contentToReturn = @"{""message"": ""Hello, World!""}";

        // Act
        var httpClient = MockHttpClientFactory.CreateForGet( expectedUrl, contentToReturn );

        var request = new HttpRequestMessage( HttpMethod.Post, expectedUrl );
        var response = await httpClient.SendAsync( request );

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.NotFound );
    }
    #endregion

    #region CreateForIdentity
    [ Fact ]
    public async Task CreateForIdentity_GetWellKnownOpenId_Successes()
    {
        // Arrange
        var baseUrl = "https://example.com";
        var clientId = "client_id";
        var clientSecret = "client_secret";
        var scopes = new List<string> { "profile", "email" };
        var accessToken = "access_token";
        var idToken = "id_token";

        // Act
        var httpClient = MockHttpClientFactory.CreateForIdentity( baseUrl, clientId, clientSecret, scopes, accessToken, idToken );
        var response = await httpClient.GetAsync( $"{baseUrl}/.well-known/openid-configuration" );

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().NotBeNullOrEmpty();
    }

    [ Fact ]
    public async Task CreateForIdentity_GetJwks_Successes()
    {
        // Arrange
        var baseUrl = "https://example.com";
        var clientId = "client_id";
        var clientSecret = "client_secret";
        var scopes = new List<string> { "profile", "email" };
        var accessToken = "access_token";
        var idToken = "id_token";

        // Act
        var httpClient = MockHttpClientFactory.CreateForIdentity( baseUrl, clientId, clientSecret, scopes, accessToken, idToken );

        var response = await httpClient.GetAsync( $"{baseUrl}/.well-known/openid-configuration/jwks" );

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().NotBeNullOrEmpty();
    }

    [ Fact ]
    public async Task CreateForIdentity_GetTokenWithBodyCredentials_Successes()
    {
        // Arrange
        var baseUrl = "https://example.com";
        var clientId = "client_id";
        var clientSecret = "client_secret";
        var scopes = new List<string> { "profile", "email" };
        var accessToken = "access_token";
        var idToken = "id_token";

        // Act
        var httpClient = MockHttpClientFactory.CreateForIdentity( baseUrl, clientId, clientSecret, scopes, accessToken, idToken );

        var tokenRequestContent = new FormUrlEncodedContent( new[]
        {
            new KeyValuePair<string, string>( "grant_type", "client_credentials" ),
            new KeyValuePair<string, string>( "scope", "profile email" ),
            new KeyValuePair<string, string>( "client_id", clientId ),
            new KeyValuePair<string, string>( "client_secret", clientSecret )
        } );

        var response = await httpClient.PostAsync( $"{baseUrl}/connect/token", tokenRequestContent );

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().NotBeNullOrEmpty();
        responseBody.Should().Contain( "access_token" );
        responseBody.Should().Contain( "id_token" );
    }

    [ Fact ]
    public async Task CreateForIdentity_GetTokenWithGet_Successes()
    {
        // Arrange
        var baseUrl = "https://example.com";
        var clientId = "client_id";
        var clientSecret = "client_secret";
        var scopes = new List<string> { "profile", "email" };
        var accessToken = "access_token";
        var idToken = "id_token";

        // Act
        var httpClient = MockHttpClientFactory.CreateForIdentity( baseUrl, clientId, clientSecret, scopes, accessToken, idToken );

        var request = new HttpRequestMessage( HttpMethod.Get, $"{baseUrl}/connect/token" )
        {
            Content = new FormUrlEncodedContent( new[]
            {
                new KeyValuePair<string, string>( "grant_type", "client_credentials" ),
                new KeyValuePair<string, string>( "scope", "profile email" ),
                new KeyValuePair<string, string>( "client_id", clientId ),
                new KeyValuePair<string, string>( "client_secret", clientSecret )
            } )
        };

        var response = await httpClient.SendAsync( request );

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().NotBeNullOrEmpty();
        responseBody.Should().Contain( "access_token" );
        responseBody.Should().Contain( "id_token" );
    }

    [ Fact ]
    public async Task CreateForIdentity_GetTokenWithHeaderCredentials_Successes()
    {
        // Arrange
        var baseUrl = "https://example.com";
        var clientId = "client_id";
        var clientSecret = "client_secret";
        var scopes = new List<string> { "profile", "email" };
        var accessToken = "access_token";
        var idToken = "id_token";

        // Act
        var httpClient = MockHttpClientFactory.CreateForIdentity( baseUrl, clientId, clientSecret, scopes, accessToken, idToken );

        var request = new HttpRequestMessage( HttpMethod.Post, $"{baseUrl}/connect/token" )
        {
            Content = new FormUrlEncodedContent( new[]
            {
                new KeyValuePair<string, string>( "grant_type", "client_credentials" ),
                new KeyValuePair<string, string>( "scope", "profile email" )
            } )
        };

        var authorizationHeader = Convert.ToBase64String( Encoding.ASCII.GetBytes( $"{clientId}:{clientSecret}" ) );
        request.Headers.Authorization = new AuthenticationHeaderValue( "Basic", authorizationHeader );
        var response = await httpClient.SendAsync( request );

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().NotBeNullOrEmpty();
        responseBody.Should().Contain( "access_token" );
        responseBody.Should().Contain( "id_token" );
    }

    [ Fact ]
    public async Task CreateForIdentity_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var baseUrl = "https://example.com";
        var clientId = "client_id";
        var clientSecret = "invalid_secret";
        var scopes = new List<string> { "profile", "email" };
        var accessToken = "access_token";
        var idToken = "id_token";

        // Act
        var httpClient = MockHttpClientFactory.CreateForIdentity( baseUrl, clientId, clientSecret, scopes, accessToken, idToken );

        var tokenRequestContent = new FormUrlEncodedContent( new[]
        {
            new KeyValuePair<string, string>( "grant_type", "client_credentials" ),
            new KeyValuePair<string, string>( "scope", "profile email" )
        } );

        var response = await httpClient.PostAsync( $"{baseUrl}/connect/token", tokenRequestContent );

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.Unauthorized );
    }
    #endregion

    #region CreateToEchoRequest
    [ Fact ]
    public async Task CreateToEchoRequest_WithCorrectParameters_EchoesRequest()
    {
        // Arrange
        var expectedHttpMethod = HttpMethod.Post;
        var expectedUrl = "https://example.com/api/echo";
        var echoBody = true;
        var contentToSend = @"{""message"": ""Hello, World!""}";

        // Act
        var httpClient = MockHttpClientFactory.CreateToEchoRequest( expectedHttpMethod, expectedUrl, echoBody );

        var request = new HttpRequestMessage( HttpMethod.Post, expectedUrl.AddQueryString( "key1", "value1" ) )
        {
            Content = new StringContent( contentToSend )
        };

        request.Headers.Add( "header1", "headerValue1" );

        var response = await httpClient.SendAsync( request );

        // Assert
        response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.OK )
            {
                Content = new StringContent( contentToSend )
            },
            new Dictionary<string, string>
            {
                { "key1", "value1" }
            }.ToImmutableDictionary(),
            new Dictionary<string, string>
            {
                { "header1", "headerValue1" }
            }.ToImmutableDictionary() );
    }

    [ Fact ]
    public async Task CreateToEchoRequest_WithWrongUrl_ReturnsNotFound()
    {
        // Arrange
        var expectedHttpMethod = HttpMethod.Post;
        var expectedUrl = "https://example.com/api/echo";
        var echoBody = true;

        // Act
        var httpClient = MockHttpClientFactory.CreateToEchoRequest( expectedHttpMethod, expectedUrl, echoBody );

        var response = await httpClient.GetAsync( "https://example.com/api/other" );

        // Assert
        response.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.NotFound ),
                                       ImmutableDictionary<string, string>.Empty,
                                       ImmutableDictionary<string, string>.Empty );
    }

    [ Fact ]
    public async Task CreateToEchoRequest_WithNotContent_EchoesRequest()
    {
        // Arrange
        var expectedHttpMethod = HttpMethod.Get;
        var expectedUrl = "https://example.com/api/echo";
        var echoBody = false;

        // Act
        var httpClient = MockHttpClientFactory.CreateToEchoRequest( expectedHttpMethod, expectedUrl, echoBody );

        var request = new HttpRequestMessage( HttpMethod.Get, expectedUrl );
        var response = await httpClient.SendAsync( request );

        // Assert
        response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.OK )
            {
                Content = new StringContent( "" )
            },
            ImmutableDictionary<string, string>.Empty,
            ImmutableDictionary<string, string>.Empty );
    }

    [ Fact ]
    public async Task CreateToEchoRequest_WithNotContentAndEchoBody_ReturnsBadRequest()
    {
        // Arrange
        var expectedHttpMethod = HttpMethod.Post;
        var expectedUrl = "https://example.com/api/echo";
        var echoBody = true;

        // Act
        var httpClient = MockHttpClientFactory.CreateToEchoRequest( expectedHttpMethod, expectedUrl, echoBody );

        var request = new HttpRequestMessage( HttpMethod.Post, expectedUrl );
        var response = await httpClient.SendAsync( request );

        // Assert
        response.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.BadRequest ),
                                       ImmutableDictionary<string, string>.Empty,
                                       ImmutableDictionary<string, string>.Empty );
    }

    [ Fact ]
    public async Task CreateToEchoRequest_WithWrongMethod_ReturnsNotFound()
    {
        // Arrange
        var expectedHttpMethod = HttpMethod.Post;
        var expectedUrl = "https://example.com/api/echo";
        var echoBody = false;

        // Act
        var httpClient = MockHttpClientFactory.CreateToEchoRequest( expectedHttpMethod, expectedUrl, echoBody );

        var request = new HttpRequestMessage( HttpMethod.Get, expectedUrl );
        var response = await httpClient.SendAsync( request );

        // Assert
        response.ShouldBeEquivalentTo( new HttpResponseMessage( HttpStatusCode.NotFound ),
                                       ImmutableDictionary<string, string>.Empty,
                                       ImmutableDictionary<string, string>.Empty );
    }
    #endregion
}