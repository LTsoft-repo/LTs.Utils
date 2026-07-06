namespace LTs.Json.Extensions;

/// <summary>
///     Date parse types for JSON strings.
/// </summary>
public enum JsonDateParseType
{
    /// <summary>
    ///     Parse date values as strings.
    /// </summary>
    String,

    /// <summary>
    ///     Parse date values as <see cref="DateTime" />.
    /// </summary>
    DateTime,

    /// <summary>
    ///     Parse date values as <see cref="DateTimeOffset" />.
    /// </summary>
    DateTimeOffset
}
