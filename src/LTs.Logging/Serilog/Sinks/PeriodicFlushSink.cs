using JetBrains.Annotations;
using LTs.Logging.Serilog.Abstractions;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

#pragma warning disable IDE0290

namespace LTs.Logging.Serilog.Sinks;

/// <summary>
///     A sink wrapper that periodically flushes the wrapped sink to disk.
/// </summary>
public class PeriodicFlushSink<TSink> : ILogEventSink, IDisposable
    where TSink : ILogEventSink, IFlushableSink
{
    private readonly TSink sink;
    private readonly Timer timer;
    private int flushRequired;

    [ UsedImplicitly ]
    private TimeSpan flushInterval; // For tests

    /// <summary>
    ///     Gets a value indicating whether the sink has been disposed.
    /// </summary>
    [ UsedImplicitly ]
    protected bool IsDisposed { get; private set; }

    /// <summary>
    ///     Construct a <see cref="PeriodicFlushSink{TSink}" /> that wraps
    ///     <paramref name="sink" /> and flushes it at the specified <paramref name="flushInterval" />.
    /// </summary>
    /// <param name="sink">The sink to wrap.</param>
    /// <param name="flushInterval">The interval at which to flush the underlying sink.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="sink" /> is <code>null</code></exception>
    public PeriodicFlushSink( TSink sink, TimeSpan flushInterval )
    {
        this.sink = sink ?? throw new ArgumentNullException( nameof( sink ) );
        this.flushInterval = flushInterval;

        if( flushInterval <= TimeSpan.Zero )
        {
            throw new ArgumentOutOfRangeException( nameof( flushInterval ), "The flush interval must be greater than zero." );
        }

        timer = new( _ => Flush(), null, flushInterval, flushInterval );
    }

    /// <inheritdoc />
    public void Emit( LogEvent logEvent )
    {
        if( IsDisposed )
        {
            return;
        }

        if( logEvent == null )
        {
            throw new ArgumentNullException( nameof( logEvent ) );
        }

        sink.Emit( logEvent );
        Interlocked.Exchange( ref flushRequired, 1 );
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if( IsDisposed )
        {
            return;
        }

        timer.Dispose();

        try
        {
            sink.Flush();
        }
        catch( Exception )
        {
            // Do nothing.
        }

        // ReSharper disable once SuspiciousTypeConversion.Global
        ( sink as IDisposable )?.Dispose();

        IsDisposed = true;
    }

    /// <summary>
    ///     Flushes the sink.
    /// </summary>
    [ UsedImplicitly ]
    protected virtual void Flush()
    {
        if( IsDisposed )
        {
            return;
        }

        try
        {
            if( Interlocked.CompareExchange( ref flushRequired, 0, 1 ) == 1 )
            {
                sink.Flush();
            }
        }
        catch( Exception ex )
        {
            SelfLog.WriteLine( "{0} could not flush the underlying sink: {1}", typeof( PeriodicFlushSink<TSink> ), ex );
        }
    }
}
