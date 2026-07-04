using JetBrains.Annotations;

namespace LTs.TestUtils.test.Infrastructure;

[ UsedImplicitly ]
internal class DisposableClass : IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
        => IsDisposed = true;
}