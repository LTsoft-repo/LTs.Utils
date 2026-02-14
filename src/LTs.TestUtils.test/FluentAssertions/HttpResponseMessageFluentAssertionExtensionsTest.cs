using System.Net;
using System.Net.Http.Headers;
using LTs.TestUtils.FluentAssertions;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace LTs.TestUtils.test.FluentAssertions;

public class HttpResponseMessageFluentAssertionExtensionsTest
{
    #region HaveContentAsJsonAsync
    [ Fact ]
    public async Task HaveContentAsJsonAsync_WithJson_Successes()
    {
        // Arrange
        var content = new { Test = "Test Content" };

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = CreateJsonContent( content )
        };

        // Act
        var act = async () => await httpResponseMessage.Should()
                                                       .HaveContentAsJsonAsync( @"{""Test"":""Test Content""}" );

        // Assert
        await act.Should().NotThrowAsync();
    }

    [ Fact ]
    public async Task HaveContentAsJsonAsync_WithTextContent_Throws()
    {
        // Arrange
        var content = new { Test = "Test Content" };

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent( JsonConvert.SerializeObject( content ) )
        };

        // Act
        var act = async () => await httpResponseMessage.Should()
                                                       .HaveContentAsJsonAsync( @"{""Test"":""Test Content""}" );

        // Assert
        await act.Should().ThrowAsync<XunitException>()
                 .WithMessage( "Expected content type \"application/json\" but found \"text/plain\"" );
    }

    [ Fact ]
    public async Task HaveContentAsJsonAsync_WithWrongContent_Throws()
    {
        // Arrange
        var content = new { Test = "Test Content 1" };

        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = CreateJsonContent( content )
        };

        // Act
        var act = async () => await httpResponseMessage.Should()
                                                       .HaveContentAsJsonAsync( @"{""Test"":""Test Content""}" );

        // Assert
        await act.Should().ThrowAsync<XunitException>()
                 .WithMessage( "Expected JSON \"{\"Test\":\"Test Content\"}\" but found \"{\"Test\":\"Test Content 1\"}\"" );
    }
    #endregion

    private static StringContent CreateJsonContent( object content )
    {
        var messageContent = new StringContent(
            JsonConvert.SerializeObject( content ),
            new MediaTypeHeaderValue( "application/json", "utf-8" ) );

        return messageContent;
    }
}