using LTs.Logging.Serilog.Sinks;

namespace LTs.Logging.test.Infrastructure;

internal class TestPeriodicFlushSink : PeriodicFlushSink<TestFlushableSink>
{
    public TestPeriodicFlushSink( TestFlushableSink sink, TimeSpan flushInterval ) : base( sink, flushInterval ) { }

    public void FlushSink() => Flush();
}
