using JetBrains.Annotations;
using LTs.TestUtils.Tests;

namespace LTs.TestUtils.test.Infrastructure;

[ UsedImplicitly ]
internal class TestClass : BaseTest
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public TestClass( ITestOutputHelper testOutput ) : base( testOutput ) { }

    public void AddDisposable( IDisposable disposable )
        => Disposables.Add( disposable );

    public ITestOutputHelper GetTestOutput()
        => TestOutput;

    public IEnumerable<IDisposable> GetDisposables()
        => Disposables;
}