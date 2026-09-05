namespace LTs.Logging.Configurations;

/// <summary>
///     Default values for the log configuration.
/// </summary>
public static class LogConfigurationDefaults
{
    /// <summary>
    ///     Path to where the log file(s) will be stored.
    /// </summary>
    public const string Path = ".\\Logs";

    /// <summary>
    ///     The maximum size for each log file in Mega Bytes.
    /// </summary>
    public const int MaxFileSizeInMegabytes = 10;

    /// <summary>
    ///     How many debug log files to keep.
    /// </summary>
    public const int DebugLogRetainedFileCount = 5;

    /// <summary>
    ///     How many error log files to keep.
    /// </summary>
    public const int ErrorLogRetainedFileCount = 95;
}
