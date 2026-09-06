using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Extensions for <see cref="HttpResponseMessage" /> assertions.
/// </summary>
public static class HttpResponseMessageFluentAssertionExtensions
{
    /// <summary>
    ///     Asserts the content is a JSON with the specific content.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectedJson">The expected JSON content.</param>
    /// <param name="because">Because message.</param>
    /// <param name="becauseArgs">Because arguments.</param>
    /// <returns></returns>
    [ UsedImplicitly ]
    public static async Task<AndConstraint<HttpResponseMessageAssertions>> HaveContentAsJsonAsync(
        this HttpResponseMessageAssertions assertions,
        string expectedJson,
        string because = "",
        params object[] becauseArgs )
    {
        assertions.HaveContentWithMediaType( "application/json", "utf-8", because, becauseArgs );

        var actualJson = await assertions.Subject.Content.ReadAsStringAsync();

        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( actualJson == expectedJson )
               .FailWith( "Expected JSON {0} but found {1}", expectedJson, actualJson );

        return new AndConstraint<HttpResponseMessageAssertions>( assertions );
    }

    /// <summary>
    ///     Asserts the content is of the specified media type and charset.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="mediaType">The expected media type.</param>
    /// <param name="charset">The expected charset.</param>
    /// <param name="because">Because message.</param>
    /// <param name="becauseArgs">Because arguments.</param>
    /// <returns></returns>
    [ UsedImplicitly ]
    public static AndConstraint<HttpResponseMessageAssertions> HaveContentWithMediaType(
        this HttpResponseMessageAssertions assertions,
        string mediaType,
        string charset,
        string because = "",
        params object[] becauseArgs )
    {
        var contentType = assertions.Subject.Content.Headers.ContentType;

        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( contentType is not null )
               .FailWith( "Expected content type {0} but found none", mediaType );

        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( contentType?.MediaType == mediaType )
               .FailWith( "Expected content type {0} but found {1}", mediaType, contentType?.MediaType );

        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( contentType?.CharSet == charset )
               .FailWith( "Expected charset {0} but found {1}", charset, contentType?.CharSet );

        return new AndConstraint<HttpResponseMessageAssertions>( assertions );
    }

    /// <summary>
    ///     Asserts the response has the specified header.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="headerName">Name of the header.</param>
    /// <param name="headerValues">The header values.</param>
    /// <param name="because">Because message.</param>
    /// <param name="becauseArgs">Because arguments.</param>
    /// <returns></returns>
    [ UsedImplicitly ]
    public static AndConstraint<HttpResponseMessageAssertions> ContainsHeaderWithValues(
        this HttpResponseMessageAssertions assertions,
        string headerName,
        string[] headerValues,
        string because = "",
        params object[] becauseArgs )
    {
        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( assertions.Subject.Headers.Contains( headerName ) )
               .FailWith( "Expected not to header {0} but found {1}", headerName, assertions.Subject.Headers );

        var values = assertions.Subject.Headers.GetValues( headerName ).ToHashSet();

        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( values.Any() )
               .FailWith( "Expected header {0} but found none", headerName );

        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( headerValues.All( values.Contains ) )
               .FailWith( "Expected header {0} to contain {1}, but found {2}", headerName, headerValues, values );

        return new AndConstraint<HttpResponseMessageAssertions>( assertions );
    }

    /// <summary>
    ///     Asserts the response does not contain the specified header.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="headerName">Name of the header.</param>
    /// <param name="because">Because message.</param>
    /// <param name="becauseArgs">Because arguments.</param>
    /// <returns></returns>
    [ UsedImplicitly ]
    public static AndConstraint<HttpResponseMessageAssertions> NotContainsHeader(
        this HttpResponseMessageAssertions assertions,
        string headerName,
        string because = "",
        params object[] becauseArgs )
    {
        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( !assertions.Subject.Headers.Contains( headerName ) )
               .FailWith( "Expected not to contain header {0} but found {1}",
                          headerName,
                          assertions.Subject.Headers.Select( h => $"{h.Key}, [{string.Join( ", ", h.Value )}]" ) );

        return new AndConstraint<HttpResponseMessageAssertions>( assertions );
    }
}