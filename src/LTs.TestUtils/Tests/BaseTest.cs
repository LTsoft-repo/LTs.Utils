namespace LTs.TestUtils.Tests;

/// <summary>
///     Represents a base class for test implementations, providing common functionality
///     such as managing disposable resources and test output logging.
/// </summary>
/// <remarks>
///     This class is designed to simplify test development by offering built-in support
///     for resource cleanup and structured logging during test execution.
/// </remarks>
public abstract class BaseTest : IDisposable
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
    ///     Creates a new instance of <see cref="BaseTest" />.
    /// </summary>
    /// <param name="testOutput"></param>
    // ReSharper disable once ConvertToPrimaryConstructor
    protected BaseTest( ITestOutputHelper testOutput )
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