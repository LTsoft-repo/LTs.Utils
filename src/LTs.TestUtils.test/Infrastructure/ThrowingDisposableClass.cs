namespace LTs.TestUtils.test.Infrastructure;

internal class ThrowingDisposableClass : IDisposable
{
    public void Dispose()
        => throw new InvalidOperationException( "Dispose failed." );
}