using System.Collections.Immutable;
using System.Net;
using LTs.TestUtils.Web;
using Xunit.Sdk;

namespace LTs.TestUtils.test.Web;

public class MockHttpClientExtensionsTest
{
    [ Fact ]
    public void ShouldBeEquivalentTo_WithCorrectResponse_Successes()
    {
        // Arrange
        var response = new HttpResponseMessage( HttpStatusCode.OK )
        {
            Content = new StringContent( "Hello, World!" )
        };

        response.Headers.Add( "X-Parameters",
                              new[]
                              {
                                  "param1=value1",
                                  "param2=value2"
                              } );

        response.Headers.Add( "Header1", "Value1" );
        response.Headers.Add( "Header2", "Value2" );

        // Act
        var act = () => response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.OK )
            {
                Content = new StringContent( "Hello, World!" )
            },
            new Dictionary<string, string>
            {
                { "param1", "value1" },
                { "param2", "value2" }
            }.ToImmutableDictionary(),
            new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            }.ToImmutableDictionary() );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ShouldBeEquivalentTo_WithWrongStatusCode_Throws()
    {
        // Arrange
        var response = new HttpResponseMessage( HttpStatusCode.BadRequest )
        {
            Content = new StringContent( "invalid_request" )
        };

        response.Headers.Add( "X-Parameters",
                              new[]
                              {
                                  "param1=value1",
                                  "param2=value2"
                              } );

        response.Headers.Add( "Header1", "Value1" );
        response.Headers.Add( "Header2", "Value2" );

        // Act
        var act = () => response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.OK )
            {
                Content = new StringContent( "Hello, World!" )
            },
            new Dictionary<string, string>
            {
                { "param1", "value1" },
                { "param2", "value2" }
            }.ToImmutableDictionary(),
            new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            }.ToImmutableDictionary() );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage(
               "Expected requestResponse!.StatusCode to be HttpStatusCode.OK {value: 200}, but found HttpStatusCode.BadRequest {value: 400}." );
    }

    [ Fact ]
    public void ShouldBeEquivalentTo_WithWrongContent_Throws()
    {
        // Arrange
        var response = new HttpResponseMessage( HttpStatusCode.BadRequest )
        {
            Content = new StringContent( "invalid_request_x" )
        };

        response.Headers.Add( "X-Parameters",
                              new[]
                              {
                                  "param1=value1",
                                  "param2=value2"
                              } );

        response.Headers.Add( "Header1", "Value1" );
        response.Headers.Add( "Header2", "Value2" );

        // Act
        var act = () => response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.BadRequest )
            {
                Content = new StringContent( "invalid_request" )
            },
            new Dictionary<string, string>
            {
                { "param1", "value1" },
                { "param2", "value2" }
            }.ToImmutableDictionary(),
            new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            }.ToImmutableDictionary() );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage(
               //"""Expected responseContent to be equivalent to "invalid_request" with a length of 15, """ +
               //"""but "invalid_request_x" has a length of 17, differs near "_x" (index 15).""" );
               @"Expected responseContent to be equivalent to ""invalid_request"" with a length of 15, " +
               @"but ""invalid_request_x"" has a length of 17, differs near ""_x"" (index 15)." );
    }

    [ Fact ]
    public void ShouldBeEquivalentTo_WithParameters_Throws()
    {
        // Arrange
        var response = new HttpResponseMessage( HttpStatusCode.OK )
        {
            Content = new StringContent( "invalid_request" )
        };

        response.Headers.Add( "X-Parameters",
                              new[]
                              {
                                  "param1=value123",
                                  "param2=value2"
                              } );

        response.Headers.Add( "Header1", "Value1" );
        response.Headers.Add( "Header2", "Value2" );

        // Act
        var act = () => response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.OK )
            {
                Content = new StringContent( "invalid_request" )
            },
            new Dictionary<string, string>
            {
                { "param1", "value1" },
                { "param2", "value2" }
            }.ToImmutableDictionary(),
            new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            }.ToImmutableDictionary() );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage(
               "Expected property responseParameters[?].Value to be \"value1\" with a length of 6, " +
               "but \"value123\" has a length of 8, differs near \"23\" (index 6).*" );
    }

    [ Fact ]
    public void ShouldBeEquivalentTo_WithWrongHeaders_Throws()
    {
        // Arrange
        var response = new HttpResponseMessage( HttpStatusCode.OK )
        {
            Content = new StringContent( "invalid_request" )
        };

        response.Headers.Add( "X-Parameters",
                              new[]
                              {
                                  "param1=value1",
                                  "param2=value2"
                              } );

        response.Headers.Add( "Header1", "Value123" );
        response.Headers.Add( "Header2", "Value2" );

        // Act
        var act = () => response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.OK )
            {
                Content = new StringContent( "invalid_request" )
            },
            new Dictionary<string, string>
            {
                { "param1", "value1" },
                { "param2", "value2" }
            }.ToImmutableDictionary(),
            new Dictionary<string, string>
            {
                { "Header1", "Value1" },
                { "Header2", "Value2" }
            }.ToImmutableDictionary() );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage(
               "Expected property responseHeaders[?].Value to be \"Value1\" with a length of 6, " +
               "but \"Value123\" has a length of 8, differs near \"23\" (index 6).*" );
    }

    //if(requestResponse.StatusCode != HttpStatusCode.NotFound &&
    //requestResponse.StatusCode != HttpStatusCode.BadRequest )
    [ Fact ]
    public void ShouldBeEquivalentTo_WithNotFound_Success()
    {
        // Arrange
        var response = new HttpResponseMessage( HttpStatusCode.NotFound );

        // Act
        var act = () => response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.NotFound ),
            ImmutableDictionary<string, string>.Empty,
            ImmutableDictionary<string, string>.Empty );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ShouldBeEquivalentTo_WithBadRequest_Success()
    {
        // Arrange
        var response = new HttpResponseMessage( HttpStatusCode.BadRequest )
        {
            Content = new StringContent( "invalid_request" )
        };

        // Act
        var act = () => response.ShouldBeEquivalentTo(
            new HttpResponseMessage( HttpStatusCode.BadRequest )
            {
                Content = new StringContent( "invalid_request" )
            },
            ImmutableDictionary<string, string>.Empty,
            ImmutableDictionary<string, string>.Empty );

        // Assert
        act.Should().NotThrow();
    }
}