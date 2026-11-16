namespace LTs.TestUtils.test.Infrastructure;

internal class PlainTestOutputHelper : ITestOutputHelper
{
    public void WriteLine( string message ) { }

    public void WriteLine( string format, params object[] args )
        => WriteLine( string.Format( format, args ) );
}