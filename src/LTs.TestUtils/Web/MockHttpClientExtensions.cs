using System.Collections.Immutable;
using System.Net;
using FluentAssertions;

namespace LTs.TestUtils.Web;

/// <summary>
///     Extensions for MockHttpClient assertions.
/// </summary>
public static class MockHttpClientExtensions
{
    /// <summary>
    ///     Asserts that the <paramref name="requestResponse" /> is equivalent to the <paramref name="expectedResponse" />.
    /// </summary>
    /// <param name="requestResponse">Response to be validated.</param>
    /// <param name="expectedResponse">Expected response.</param>
    /// <param name="expectedParameters">Expected URI parameters.</param>
    /// <param name="expectedHeaders">Expected headers.</param>
    public static void ShouldBeEquivalentTo(
        this HttpResponseMessage? requestResponse,
        HttpResponseMessage expectedResponse,
        IImmutableDictionary<string, string> expectedParameters,
        IImmutableDictionary<string, string> expectedHeaders )
    {
        requestResponse.Should().NotBeNull();

        // Status code.
        requestResponse!.StatusCode.Should().Be( expectedResponse.StatusCode );

        if( requestResponse.StatusCode != HttpStatusCode.NotFound &&
            requestResponse.StatusCode != HttpStatusCode.BadRequest )
        {
            // Validate parameters.
            var parameterHeader = requestResponse.Headers.GetValues( "X-Parameters" );

            var responseParameters = parameterHeader
                .Where( p => p.Contains( '=' ) )
                .Select( p =>
                {
                    var parts = p.Split( '=' );

                    return ( key: parts[ 0 ], value: parts[ 1 ] );
                } )
                .ToImmutableDictionary( t => t.key, t => t.value );

            responseParameters.Should().BeEquivalentTo( expectedParameters );

            // Validate headers.
            var responseHeaders = requestResponse.Headers
                .Where( h => h.Key != "X-Parameters" )
                .ToDictionary( h => h.Key, h => string.Join( ", ", h.Value ) );

            responseHeaders.Should().BeEquivalentTo( expectedHeaders );
        }

        // Content.
        var responseContent = requestResponse.Content.ReadAsStringAsync().Result;
        var expectedContent = expectedResponse.Content.ReadAsStringAsync().Result;

        responseContent.Should().BeEquivalentTo( expectedContent );
    }
}