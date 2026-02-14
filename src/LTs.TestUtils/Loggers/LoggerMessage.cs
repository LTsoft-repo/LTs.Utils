using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.Loggers;

/// <summary>
///     Logger message.
/// </summary>
public record LoggerMessage
{
    /// <summary>
    ///     Log level of the message
    /// </summary>
    public LogLevel LogLevel { get; init; }

    /// <summary>
    ///     Elapsed milliseconds since the logger was created.
    /// </summary>
    public long ElapsedMilliseconds { get; init; }

    /// <summary>
    ///     Thread ID of the logger.
    /// </summary>
    public int ThreadId { get; init; }

    /// <summary>
    ///     Text of the message.
    /// </summary>
    [ UsedImplicitly ]
    public required string Text { get; init; }

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Enum.GetName( LogLevel )} " +
                                         $"[{ElapsedMilliseconds,4} ms]" +
                                         $"[{ThreadId}] --> {Text}";
}