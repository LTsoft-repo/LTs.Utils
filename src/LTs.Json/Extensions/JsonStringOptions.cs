namespace LTs.Json.Extensions;

/// <summary>
///     Options for converting values to JSON strings.
/// </summary>
[ Flags ]
public enum JsonStringOptions
{
    /// <summary>
    ///     No options. Compact JSON without indentation.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Compact JSON without indentation. Takes precedence over <see cref="UseIndent" />.
    /// </summary>
    Minify = 1,

    /// <summary>
    ///     Indented JSON.
    /// </summary>
    UseIndent = 2,

    /// <summary>
    ///     Omit null values when serializing.
    /// </summary>
    IgnoreNulls = 4,

    /// <summary>
    ///     Enum values as JSON strings using the C# member name (Value2).
    /// </summary>
    EnumsAsStrings = 8,

    /// <summary>
    ///     Enum values as JSON strings using LT friendly names (Value 2). Wins over <see cref="EnumsAsStrings" />.
    /// </summary>
    EnumsAsFriendlyNames = 16
}
