using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace LTs.Logging.Configurations;

/// <summary>
///     Extensions for <see cref="LogConfiguration" />.
/// </summary>
public static class LogConfigurationExtensions
{
    /// <summary>
    ///     Loads the Log configuration.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration" /> instance.</param>
    /// <returns>The loaded <see cref="LogConfiguration" />.</returns>
    [ UsedImplicitly ]
    public static LogConfiguration LoadLogConfiguration( this IConfiguration configuration )
        => new()
        {
            Path = Environment.ExpandEnvironmentVariables( configuration.GetValue( "Path", LogConfigurationDefaults.Path ) ??
                                                           LogConfigurationDefaults.Path ),
            MaxFileSizeInMegabytes = configuration.GetValue( "MaxFileSizeInMegabytes", LogConfigurationDefaults.MaxFileSizeInMegabytes ),
            DebugLogRetainedFileCount = configuration.GetValue( "DebugLogRetainedFileCount", LogConfigurationDefaults.DebugLogRetainedFileCount ),
            ErrorLogRetainedFileCount = configuration.GetValue( "ErrorLogRetainedFileCount", LogConfigurationDefaults.ErrorLogRetainedFileCount )
        };
}
