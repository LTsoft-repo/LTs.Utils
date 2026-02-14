namespace LTs.TestUtils.Tests;

/// <summary>
///     Base class for tests that need to dispose of resources. It also adds a TestOutput property for writing to the test
///     output.
/// </summary>
public abstract class DisposableTest : IDisposable
{
    /// <summary>
    ///     Test output helper.
    /// </summary>
    protected readonly ITestOutputHelper TestOutput;

    /// <summary>
    ///     List of disposables to clean up at the end of each test.
    /// </summary>
    protected readonly List<IDisposable> Disposables = [ ];

    /// <summary>
    ///     Creates a new instance of <see cref="DisposableTest" />.
    /// </summary>
    /// <param name="testOutput"></param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public DisposableTest( ITestOutputHelper testOutput )
        => TestOutput = testOutput;

    /// <inheritdoc />
    public virtual void Dispose()
    {
        TestOutput.WriteLine( "Cleaning up..." );

        foreach( var disposable in Disposables )
        {
            try
            {
                disposable.Dispose();
            }
            catch( Exception ex )
            {
                if( ex is ObjectDisposedException )
                {
                    continue;
                }

                TestOutput.WriteLine( $"Error disposing {disposable.GetType().Name}: {ex}" );
            }
        }

        Disposables.Clear();
    }
}