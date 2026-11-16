namespace LTs.TestUtils.test.Infrastructure;

internal class CapturingTestOutputHelper : ITestOutputHelper
{
    internal List<string> Lines { get; } = [ ];

    public void WriteLine( string message )
        => Lines.Add( message );

    public void WriteLine( string format, params object[] args )
        => WriteLine( string.Format( format, args ) );
}