using System.Collections.Immutable;
using LTs.Web.Mime;

namespace LTs.TestUtils.Web;

/// <summary>
///     Represents the content of a response to echo the request.
/// </summary>
public record MockHttpHandlerFactoryEchoResponseContent
{
    /// <summary>
    ///     The URI of the request.
    /// </summary>
    [ UsedImplicitly ]
    public required string Uri { get; set; }

    /// <summary>
    ///     The HTTP method of the request.
    /// </summary>
    [ UsedImplicitly ]
    public required string Method { get; set; }

    /// <summary>
    ///     The parameters of the request.
    /// </summary>
    public IImmutableDictionary<string, string> Parameters { get; set; } = ImmutableDictionary<string, string>.Empty;

    /// <summary>
    ///     The headers of the request.
    /// </summary>
    public IImmutableDictionary<string, string> Headers { get; init; } = ImmutableDictionary<string, string>.Empty;

    /// <summary>
    ///     The body of the request.
    /// </summary>
    public string Body { get; init; } = "";

    /// <summary>
    ///     The media type of the request.
    /// </summary>
    public MediaType MediaType { get; init; } = MediaType.None;
}