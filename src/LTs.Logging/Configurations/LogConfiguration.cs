namespace LTs.Logging.Configurations;

/// <summary>
///     Configuration for the logs.
/// </summary>
public record LogConfiguration
{
    /// <summary>
    ///     Path to where the log file(s) will be stored.
    /// </summary>
    public string Path { get; init; } = LogConfigurationDefaults.Path;

    /// <summary>
    ///     The maximum size for each log file in Mega Bytes.
    /// </summary>
    public int MaxFileSizeInMegabytes { get; init; } = LogConfigurationDefaults.MaxFileSizeInMegabytes;

    /// <summary>
    ///     How many debug log files to keep.
    /// </summary>
    public int DebugLogRetainedFileCount { get; init; } = LogConfigurationDefaults.DebugLogRetainedFileCount;

    /// <summary>
    ///     How many error log files to keep.
    /// </summary>
    public int ErrorLogRetainedFileCount { get; init; } = LogConfigurationDefaults.ErrorLogRetainedFileCount;
}
