using System.Collections.Concurrent;

namespace LTs.Stores.Infrastructure;

// ToDo: Move to LTs.Utils.

/// <summary>
///     Serializes access to one file within the process.
/// </summary>
internal class FileGate : IAsyncDisposable
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> Gates =
        new( StringComparer.OrdinalIgnoreCase );

    private readonly SemaphoreSlim semaphore;
    private bool entered;

    private FileGate( SemaphoreSlim semaphore )
        => this.semaphore = semaphore;

    /// <summary>
    ///     Waits for exclusive access to the file at <paramref name="absolutePath" />.
    /// </summary>
    /// <param name="absolutePath">The absolute file path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A gate that releases the lock when disposed.</returns>
    public static async Task<FileGate> AcquireAsync( string absolutePath,
                                                     CancellationToken cancellationToken )
    {
        var semaphore = Gates.GetOrAdd( absolutePath, _ => new SemaphoreSlim( 1, 1 ) );
        await semaphore.WaitAsync( cancellationToken );

        return new FileGate( semaphore )
        {
            entered = true
        };
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        if( !entered )
        {
            return ValueTask.CompletedTask;
        }

        semaphore.Release();
        entered = false;

        return ValueTask.CompletedTask;
    }
}