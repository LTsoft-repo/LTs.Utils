using JetBrains.Annotations;
using LTs.Logging.Serilog.Abstractions;
using Serilog.Core;
using Serilog.Events;

namespace LTs.Logging.test.Infrastructure;

[ UsedImplicitly ]
public class TestFlushableSink : ILogEventSink, IFlushableSink
{
    public virtual void Emit( LogEvent logEvent ) { }

    public virtual void Flush() { }
}
