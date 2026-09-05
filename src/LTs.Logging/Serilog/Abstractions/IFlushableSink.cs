namespace LTs.Logging.Serilog.Abstractions;

/// <summary>
///     Supported by sinks that can be explicitly flushed.
/// </summary>
public interface IFlushableSink
{
    /// <summary>
    ///     Flushes the sink.
    /// </summary>
    void Flush();
}
