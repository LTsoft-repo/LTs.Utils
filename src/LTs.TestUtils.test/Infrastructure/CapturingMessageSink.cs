using Xunit.Abstractions;
using Xunit.Sdk;

namespace LTs.TestUtils.test.Infrastructure;

internal class CapturingMessageSink : LongLivedMarshalByRefObject, IMessageSink
{
    internal List<IMessageSinkMessage> Messages { get; } = [ ];

    public bool OnMessage( IMessageSinkMessage message )
    {
        Messages.Add( message );
        return true;
    }
}