using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Maps <see cref="LogLevel" /> values to the tags written by <see cref="Loggers.TestLogger" />.
/// </summary>
internal static class LogLevelOutputTags
{
    internal static string GetTag( LogLevel logLevel )
        => logLevel switch
        {
            LogLevel.Trace => "TRC",
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "CRT",
            _ => "UNK"
        };

    internal static bool LineMatchesLevel( string line, LogLevel logLevel )
        => line.Contains( $"[{GetTag( logLevel )}]", StringComparison.Ordinal );

    internal static string GetLevelName( LogLevel logLevel )
        => Enum.GetName( logLevel ) ?? logLevel.ToString();
}
