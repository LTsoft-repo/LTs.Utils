namespace LTs.TestUtils.test.Infrastructure;

internal class ObjectDisposedDisposableClass : IDisposable
{
    public void Dispose()
        => throw new ObjectDisposedException( nameof( ObjectDisposedDisposableClass ) );
}