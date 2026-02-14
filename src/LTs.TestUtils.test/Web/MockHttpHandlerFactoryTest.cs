using System.Collections.Immutable;
using System.Net;
using LTs.TestUtils.Web;
using LTs.Web.Authorization;
using LTs.Web.Configurations;
using LTs.Web.Mime;
using Newtonsoft.Json;

namespace LTs.TestUtils.test.Web;

public class MockHttpHandlerFactoryTest
{
    #region GetAsync
    [ Fact ]
    public async Task CreateToEchoRequest_GetAsync_WithUrlOnly_ReturnsMockedResponse()
    {
        // Arrange
        var token = "dummyToken";

        var authorizationConfig = new AuthorizationConfiguration
        {
            AccessTokenUrl = "https://example.com/token",
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            Scope = "scope"
        };

        var mockHttpHandler = MockHttpHandlerFactory.CreateToEchoRequest( authorizationConfig, token );
        var httpHandler = mockHttpHandler.Object;

        // Act
        var response = await httpHandler.GetAsync( "https://example.com/api" );

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseData = await response.Content.ReadAsStringAsync();
        responseData.Should().NotBeNull();
        var responseObject = JsonConvert.DeserializeObject<MockHttpHandlerFactoryEchoResponseContent>( responseData );
        responseObject.Should().NotBeNull();

        responseObject.Should().BeEquivalentTo( new MockHttpHandlerFactoryEchoResponseContent
        {
            Method = "GET",
            Uri = "https://example.com/api",
            Parameters = ImmutableDictionary<string, string>.Empty,
            Headers = ImmutableDictionary<string, string>.Empty
        } );
    }

    [ Fact ]
    public async Task CreateToEchoRequest_GetAsync_WithParametersAndHeaders_ReturnsMockedResponse()
    {
        // Arrange
        var token = "dummyToken";

        var authorizationConfig = new AuthorizationConfiguration
        {
            AccessTokenUrl = "https://example.com/token",
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            Scope = "scope"
        };

        var mockHttpHandler = MockHttpHandlerFactory.CreateToEchoRequest( authorizationConfig, token );
        var httpHandler = mockHttpHandler.Object;

        // Act
        var response = await httpHandler.GetAsync(
                           "https://example.com/api",
                           new Dictionary<string, string>
                           {
                               [ "param1" ] = "value1",
                               [ "param2" ] = "value2"
                           }.ToImmutableDictionary(),
                           new Dictionary<string, string>
                           {
                               [ "header1" ] = "headerValue1",
                               [ "header2" ] = "headerValue2"
                           }.ToImmutableDictionary() );

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseData = await response.Content.ReadAsStringAsync();
        responseData.Should().NotBeNull();
        var responseObject = JsonConvert.DeserializeObject<MockHttpHandlerFactoryEchoResponseContent>( responseData );
        responseObject.Should().NotBeNull();

        responseObject.Should().BeEquivalentTo( new MockHttpHandlerFactoryEchoResponseContent
        {
            Method = "GET",
            Uri = "https://example.com/api",
            Parameters = new Dictionary<string, string>
            {
                [ "param1" ] = "value1",
                [ "param2" ] = "value2"
            }.ToImmutableDictionary(),
            Headers = new Dictionary<string, string>
            {
                [ "header1" ] = "headerValue1",
                [ "header2" ] = "headerValue2"
            }.ToImmutableDictionary()
        } );
    }

    [ Fact ]
    public async Task CreateToEchoRequest_GetAsync_WithBody_ReturnsMockedResponse()
    {
        // Arrange
        var token = "dummyToken";

        var authorizationConfig = new AuthorizationConfiguration
        {
            AccessTokenUrl = "https://example.com/token",
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            Scope = "scope"
        };

        var mockHttpHandler = MockHttpHandlerFactory.CreateToEchoRequest( authorizationConfig, token );
        var httpHandler = mockHttpHandler.Object;

        // Act
        var response = await httpHandler.GetAsync(
                           "https://example.com/api",
                           new Dictionary<string, string>
                           {
                               [ "param1" ] = "valueA",
                               [ "param2" ] = "valueB"
                           }.ToImmutableDictionary(),
                           new Dictionary<string, string>
                           {
                               [ "header1" ] = "headerValueA",
                               [ "header2" ] = "headerValueB"
                           }.ToImmutableDictionary(),
                           "Some Body",
                           MediaType.TextPlain );

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseData = await response.Content.ReadAsStringAsync();
        responseData.Should().NotBeNull();
        var responseObject = JsonConvert.DeserializeObject<MockHttpHandlerFactoryEchoResponseContent>( responseData );
        responseObject.Should().NotBeNull();

        responseObject.Should().BeEquivalentTo( new MockHttpHandlerFactoryEchoResponseContent
        {
            Method = "GET",
            Uri = "https://example.com/api",
            Parameters = new Dictionary<string, string>
            {
                [ "param1" ] = "valueA",
                [ "param2" ] = "valueB"
            }.ToImmutableDictionary(),
            Headers = new Dictionary<string, string>
            {
                [ "header1" ] = "headerValueA",
                [ "header2" ] = "headerValueB"
            }.ToImmutableDictionary(),
            Body = "Some Body",
            MediaType = MediaType.TextPlain
        } );
    }
    #endregion

    #region PostAsync
    [ Fact ]
    public async Task CreateToEchoRequest_PostTextAsync_WithBodyOnly_ReturnsMockedResponse()
    {
        // Arrange
        var token = "dummyToken";

        var authorizationConfig = new AuthorizationConfiguration
        {
            AccessTokenUrl = "https://example.com/token",
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            Scope = "scope"
        };

        var mockHttpHandler = MockHttpHandlerFactory.CreateToEchoRequest( authorizationConfig, token );
        var httpClient = mockHttpHandler.Object;

        // Act
        var response = await httpClient.PostTextAsync( "https://example.com/api", "bodyContent", MediaType.ApplicationJson );

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseData = await response.Content.ReadAsStringAsync();
        responseData.Should().NotBeNull();
        var responseObject = JsonConvert.DeserializeObject<MockHttpHandlerFactoryEchoResponseContent>( responseData );
        responseObject.Should().NotBeNull();

        responseObject.Should().BeEquivalentTo( new MockHttpHandlerFactoryEchoResponseContent
        {
            Method = "POST",
            Uri = "https://example.com/api",
            Body = "bodyContent",
            MediaType = MediaType.ApplicationJson
        } );
    }

    [ Fact ]
    public async Task CreateToEchoRequest_PostTextAsync_WithParametersAndHeaders_ReturnsMockedResponse()
    {
        // Arrange
        var token = "dummyToken";

        var authorizationConfig = new AuthorizationConfiguration
        {
            AccessTokenUrl = "https://example.com/token",
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            Scope = "scope"
        };

        var mockHttpHandler = MockHttpHandlerFactory.CreateToEchoRequest( authorizationConfig, token );
        var httpClient = mockHttpHandler.Object;

        // Act
        var response = await httpClient.PostTextAsync( "https://example.com/api",
                                                       new Dictionary<string, string>
                                                       {
                                                           [ "param1" ] = "valueA",
                                                           [ "param2" ] = "valueB"
                                                       }.ToImmutableDictionary(),
                                                       new Dictionary<string, string>
                                                       {
                                                           [ "header1" ] = "headerValueA",
                                                           [ "header2" ] = "headerValueB"
                                                       }.ToImmutableDictionary(),
                                                       "bodyContent",
                                                       MediaType.ApplicationJson );

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be( HttpStatusCode.OK );

        var responseData = await response.Content.ReadAsStringAsync();
        responseData.Should().NotBeNull();
        var responseObject = JsonConvert.DeserializeObject<MockHttpHandlerFactoryEchoResponseContent>( responseData );
        responseObject.Should().NotBeNull();

        responseObject.Should().BeEquivalentTo( new MockHttpHandlerFactoryEchoResponseContent
        {
            Method = "POST",
            Uri = "https://example.com/api",
            Parameters = new Dictionary<string, string>
            {
                [ "param1" ] = "valueA",
                [ "param2" ] = "valueB"
            }.ToImmutableDictionary(),
            Headers = new Dictionary<string, string>
            {
                [ "header1" ] = "headerValueA",
                [ "header2" ] = "headerValueB"
            }.ToImmutableDictionary(),
            Body = "bodyContent",
            MediaType = MediaType.ApplicationJson
        } );
    }
    #endregion

    #region GetAccessTokenAsync
    [ Fact ]
    public async Task CreateToEchoRequest_GetAccessTokenAsync_ReturnsMockedToken()
    {
        // Arrange
        var token = "dummyToken";

        var authorizationConfig = new AuthorizationConfiguration
        {
            AccessTokenUrl = "https://example.com/token",
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            Scope = "scope"
        };

        var mockHttpHandler = MockHttpHandlerFactory.CreateToEchoRequest( authorizationConfig, token );

        // Act
        var actualToken = await mockHttpHandler.Object.GetAccessTokenAsync(
                              "https://example.com/token",
                              GrantType.ClientCredentials,
                              "clientId",
                              "clientSecret",
                              "scope" );

        // Assert
        actualToken.Should().NotBeNullOrEmpty();
        actualToken.Should().Be( token );
    }
    #endregion
}