namespace LTs.Json.Extensions;

/// <summary>
///     Options for converting JSON tokens to strings.
/// </summary>
public sealed record JsonStringOptions
{
    /// <summary>
    ///     Default JSON string options.
    /// </summary>
    public static JsonStringOptions Default { get; } = new();

    /// <summary>
    ///     Indicates whether the JSON string should be indented.
    /// </summary>
    public bool UseIndent { get; init; }

    /// <summary>
    ///     Indicates whether the JSON string should be minified.
    /// </summary>
    public bool Minify { get; init; }
}