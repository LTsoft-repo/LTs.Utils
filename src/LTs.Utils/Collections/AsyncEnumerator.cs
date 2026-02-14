namespace LTs.Utils.Collections;

/// <summary>
///     Represents an asynchronous enumerator over a sequence of values.
/// </summary>
/// <typeparam name="T">Type of the elements in the collection.</typeparam>
public class AsyncEnumerator<T> : IAsyncEnumerator<T>
{
    /// <inheritdoc />
    public T Current => inner.Current;

    private readonly IEnumerator<T> inner;
    private bool disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncEnumerator{T}" /> class.
    /// </summary>
    /// <param name="inner">Inner enumerator.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public AsyncEnumerator( IEnumerator<T> inner )
        => this.inner = inner;

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        if( inner is IDisposable d )
        {
            d.Dispose();
        }

        disposed = true;

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask<bool> MoveNextAsync()
    {
        if( disposed )
        {
            return new ValueTask<bool>( false );
        }

        return new ValueTask<bool>( inner.MoveNext() );
    }
}