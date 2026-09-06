namespace LTs.Json.Extensions;

/// <summary>
///     Options for parsing JSON strings.
/// </summary>
public record JsonParseOptions
{
    /// <summary>
    ///     Default JSON parse options.
    /// </summary>
    public static JsonParseOptions Default { get; } = new();

    /// <summary>
    ///     Date parse type to use when parsing JSON strings.
    /// </summary>
    public JsonDateParseType DateParseType { get; init; } = JsonDateParseType.String;
}