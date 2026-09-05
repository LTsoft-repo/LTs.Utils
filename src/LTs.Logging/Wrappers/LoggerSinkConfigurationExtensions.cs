using Serilog;
using Serilog.Configuration;

namespace LTs.Logging.Wrappers;

/// <summary>
///     Extensions for the logger sink configuration.
/// </summary>
public static class LoggerSinkConfigurationExtensions
{
    /// <summary>
    ///     Wraps the logger sink configuration with a transformation.
    /// </summary>
    /// <param name="loggerSinkConfiguration">Logger sink configuration.</param>
    /// <param name="transformations">Transformations to apply.</param>
    /// <param name="writeTo">Action to write to.</param>
    /// <returns>The Logger configuration.</returns>
    public static LoggerConfiguration TransformLog(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        IEnumerable<ILogTransformation> transformations,
        Action<LoggerSinkConfiguration> writeTo )
    {
        var wrapper = LoggerSinkConfiguration.Wrap( wrapped => new TransformLogSinkWrapper( wrapped, transformations ), writeTo );

        return loggerSinkConfiguration.Sink( wrapper );
    }
}
