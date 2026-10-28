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
    IgnoreNulls = 4
}
