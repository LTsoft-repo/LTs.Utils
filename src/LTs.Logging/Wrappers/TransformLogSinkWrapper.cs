using Serilog.Core;
using Serilog.Events;

#pragma warning disable IDE0290

namespace LTs.Logging.Wrappers;

/// <summary>
///     Wraps a log sink and applies transformations to the log events.
/// </summary>
public class TransformLogSinkWrapper : ILogEventSink, IDisposable
{
    private readonly ILogEventSink outputSink;
    private readonly ILogTransformation[] transformations;

    /// <summary>
    ///     Creates a new instance of <see cref="TransformLogSinkWrapper" />.
    /// </summary>
    /// <param name="outputSink">The output sink.</param>
    /// <param name="transformations">The transformations to apply.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public TransformLogSinkWrapper( ILogEventSink outputSink, IEnumerable<ILogTransformation> transformations )
    {
        _ = transformations ?? throw new ArgumentNullException( nameof( transformations ) );

        this.outputSink = outputSink;
        this.transformations = transformations.ToArray();
    }

    /// <inheritdoc />
    public void Dispose()
        => ( outputSink as IDisposable )?.Dispose();

    /// <inheritdoc />
    public void Emit( LogEvent logEvent )
    {
        var newLogEvent = logEvent;

        foreach( var transformation in transformations )
        {
            if( newLogEvent == null )
            {
                continue;
            }

            var shouldTransform = transformation.ShouldTransform( newLogEvent );

            if( shouldTransform )
            {
                var transformedLogEvent = transformation.Transform( newLogEvent );
                newLogEvent = transformedLogEvent;
            }
        }

        if( newLogEvent != null )
        {
            outputSink.Emit( newLogEvent );
        }
    }
}
