namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Options for JSON equivalencies.
/// </summary>
public sealed record JsonAssertionOptions
{
    /// <summary>
    ///     Default JSON assertion options.
    /// </summary>
    public static JsonAssertionOptions Default { get; } = new();

    /// <summary>
    ///     JSON paths to exclude from the comparison.
    /// </summary>
    public IEnumerable<string> ExcludedJsonPaths { get; init; } = [ ];

    /// <summary>
    ///     Indicates whether extra fields in the subject JSON should be ignored.
    /// </summary>
    public bool IgnoreExtraFields { get; init; }

    /// <summary>
    ///     Excludes a JSON path from the comparison.
    /// </summary>
    /// <param name="jsonPath">JSON path to exclude.</param>
    /// <returns>The JSON assertion options.</returns>
    public JsonAssertionOptions Exclude( string jsonPath )
        => this with
        {
            ExcludedJsonPaths = ExcludedJsonPaths.Append( jsonPath ).ToArray()
        };

    /// <summary>
    ///     Ignores extra fields in the subject JSON.
    /// </summary>
    /// <returns>The JSON assertion options.</returns>
    public JsonAssertionOptions IgnoringExtraFields()
        => this with
        {
            IgnoreExtraFields = true
        };
}