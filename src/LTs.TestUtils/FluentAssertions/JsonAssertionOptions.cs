namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     JSON path matcher for JSON equivalencies.
/// </summary>
/// <param name="JsonPath">JSON path to match.</param>
/// <param name="Match">Matcher to execute for the selected JSON value.</param>
public record JsonAssertionMatcher( string JsonPath, Action<string?> Match );

/// <summary>
///     Options for JSON equivalencies.
/// </summary>
public record JsonAssertionOptions
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
    ///     JSON path matchers to validate from the subject JSON.
    /// </summary>
    public IEnumerable<JsonAssertionMatcher> Matchers { get; init; } = [ ];

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

    /// <summary>
    ///     Adds a matcher for a JSON path.
    /// </summary>
    /// <param name="jsonPath">JSON path to match.</param>
    /// <param name="match">Matcher to execute for the selected JSON value.</param>
    /// <returns>The JSON assertion options.</returns>
    public JsonAssertionOptions WithMatcher( string jsonPath, Action<string?> match )
        => this with
        {
            Matchers = Matchers.Append( new JsonAssertionMatcher( jsonPath, match ) ).ToArray()
        };
}