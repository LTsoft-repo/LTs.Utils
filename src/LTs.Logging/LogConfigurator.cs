using JetBrains.Annotations;
using LTs.Logging.Configurations;
using LTs.Logging.Wrappers;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace LTs.Logging;

/// <summary>
///     Configures Serilog with the provided configuration.
/// </summary>
public static class LogConfigurator
{
    /// <summary>
    ///     Template format for the logs.
    /// </summary>
    [ UsedImplicitly ]
    public const string LogTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

    /// <summary>
    ///     Configures Serilog to store logs in file.
    /// </summary>
    /// <param name="sinkConfiguration">The sink configuration.</param>
    /// <param name="path">Path to the log file(s).</param>
    /// <param name="maxFileSizeInMegabytes">The maximum file size in megabytes.</param>
    /// <param name="retainedFileCount">How many files to keep.</param>
    /// <param name="logEventLevel">The log event level.</param>
    /// <returns></returns>
    [ UsedImplicitly ]
    public static LoggerConfiguration FileWithConfiguration(
        this LoggerSinkConfiguration sinkConfiguration,
        string path,
        int maxFileSizeInMegabytes,
        int retainedFileCount,
        LogEventLevel logEventLevel = LevelAlias.Minimum )
        => sinkConfiguration.File( path,
                                   fileSizeLimitBytes: 1024 * 1024 * maxFileSizeInMegabytes,
                                   rollingInterval: RollingInterval.Hour,
                                   rollOnFileSizeLimit: true,
                                   retainedFileCountLimit: retainedFileCount,
                                   buffered: true,
                                   flushToDiskInterval: TimeSpan.FromSeconds( 5 ),
                                   outputTemplate: LogTemplate,
                                   restrictedToMinimumLevel: logEventLevel );

    /// <summary>
    ///     Configures Serilog to store the debug and error logs separately.
    /// </summary>
    /// <param name="configuration">The log configuration.</param>
    public static void Configure( LogConfiguration configuration )
        => Configure( configuration, null );

    /// <summary>
    ///     Configures Serilog to store the debug and error logs separately.
    /// </summary>
    /// <param name="configuration">The log configuration.</param>
    /// <param name="additionalConfiguration">
    ///     Additional Logger configuration. If no additional configuration is needed, pass
    ///     <c>null</c>.
    /// </param>
    public static void Configure( LogConfiguration configuration, Action<LoggerConfiguration>? additionalConfiguration )
    {
        var loggerConfiguration = new LoggerConfiguration()
                                  .MinimumLevel.Debug()
                                  .MinimumLevel.Override( "Microsoft", LogEventLevel.Information )
                                  .Enrich.FromLogContext()
                                  .WriteTo.Console( outputTemplate: LogTemplate )
                                  .WriteTo.Debug( outputTemplate: LogTemplate )
                                  .WriteTo.FileWithConfiguration(
                                      Path.Combine( configuration.Path, "debug-.log" ),
                                      configuration.MaxFileSizeInMegabytes,
                                      configuration.DebugLogRetainedFileCount )
                                  .WriteTo.FileWithConfiguration(
                                      Path.Combine( configuration.Path, "error-.log" ),
                                      configuration.MaxFileSizeInMegabytes,
                                      configuration.ErrorLogRetainedFileCount,
                                      LogEventLevel.Error );

        additionalConfiguration?.Invoke( loggerConfiguration );

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    /// <summary>
    ///     Configures Serilog to store the debug and error logs separately, and using transformations.
    /// </summary>
    /// <param name="configuration">The log configuration.</param>
    /// <param name="additionalConfiguration">
    ///     Additional Logger configuration. If no additional configuration is needed, pass
    ///     <c>null</c>.
    /// </param>
    /// <param name="transformations">IEnumerable of LogTransformations to apply to the logs.</param>
    public static void ConfigureWithTransform(
        LogConfiguration configuration,
        IEnumerable<ILogTransformation> transformations,
        Action<LoggerConfiguration>? additionalConfiguration )
    {
        var arrayTransformation = transformations.ToArray();

        var loggerConfiguration = new LoggerConfiguration()
                                  .MinimumLevel.Debug()
                                  .MinimumLevel.Override( "Microsoft", LogEventLevel.Information )
                                  .Enrich.FromLogContext()
                                  .WriteTo.TransformLog(
                                      arrayTransformation,
                                      writeTo => writeTo.Console( outputTemplate: LogTemplate ) )
                                  .WriteTo.TransformLog(
                                      arrayTransformation,
                                      writeTo => writeTo.Debug( outputTemplate: LogTemplate ) )
                                  .WriteTo.TransformLog(
                                      arrayTransformation,
                                      writeTo => writeTo.FileWithConfiguration(
                                          Path.Combine( configuration.Path, "debug-.log" ),
                                          configuration.MaxFileSizeInMegabytes,
                                          configuration.DebugLogRetainedFileCount ) )
                                  .WriteTo.TransformLog(
                                      arrayTransformation,
                                      writeTo => writeTo.FileWithConfiguration(
                                          Path.Combine( configuration.Path, "error-.log" ),
                                          configuration.MaxFileSizeInMegabytes,
                                          configuration.ErrorLogRetainedFileCount,
                                          LogEventLevel.Error ) );

        additionalConfiguration?.Invoke( loggerConfiguration );

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    /// <summary>
    ///     Adds Client IP and Agent to Http request logging.
    ///     <para>For this to work, the Web app has to register <c>services.AddHttpContextAccessor();</c></para>
    /// </summary>
    /// <param name="configuration">The log configuration.</param>
    [ UsedImplicitly ]
    public static void ConfigureWithIp( LogConfiguration configuration )
        => Configure( configuration,
                      loggerConfiguration =>
                          {
                              loggerConfiguration
                                  .Enrich.WithClientIp()
                                  .Enrich.WithRequestHeader( "User-Agent" );
                          } );
}
