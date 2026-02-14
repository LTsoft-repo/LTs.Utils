using System.Linq.Expressions;

namespace LTs.Utils.Collections;

/// <summary>
///     Represents a collection of objects that can be asynchronously enumerated.
/// </summary>
/// <typeparam name="T">Type of the elements in the collection.</typeparam>
public class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    IQueryProvider IQueryable.Provider => new AsyncQueryProvider<T>( this );

    /// <inheritdoc />
    public AsyncEnumerable( IEnumerable<T> enumerable ) : base( enumerable ) { }

    /// <inheritdoc />
    public AsyncEnumerable( Expression expression ) : base( expression ) { }

    /// <inheritdoc />
    public IAsyncEnumerator<T> GetAsyncEnumerator( CancellationToken cancellationToken = default )
        => new AsyncEnumerator<T>( this.AsEnumerable().GetEnumerator() );
}