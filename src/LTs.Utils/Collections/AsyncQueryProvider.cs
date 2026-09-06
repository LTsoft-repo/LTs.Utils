using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace LTs.Utils.Collections;

/// <summary>
///     Defines method to execute queries asynchronously that are described by an IQueryable object.
/// </summary>
/// <typeparam name="TEntity">Type of the elements in the collection.</typeparam>
[ UsedImplicitly ]
public class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider inner;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncQueryProvider{TEntity}" /> class.
    /// </summary>
    /// <param name="inner">The inner query provider.</param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public AsyncQueryProvider( IQueryProvider inner )
        => this.inner = inner;

    /// <inheritdoc />
    public virtual IQueryable CreateQuery( Expression expression )
        => new AsyncEnumerable<TEntity>( expression );

    /// <inheritdoc />
    public virtual IQueryable<TElement> CreateQuery<TElement>( Expression expression )
        => new AsyncEnumerable<TElement>( expression );

    /// <inheritdoc />
    public virtual object Execute( Expression expression )
        => inner.Execute( expression )!;

    /// <inheritdoc />
    public virtual TResult Execute<TResult>( Expression expression )
        => inner.Execute<TResult>( expression );

    /// <inheritdoc />
    public TResult ExecuteAsync<TResult>( Expression expression, CancellationToken cancellationToken )
    {
        var result = inner.Execute<TResult>( expression );

        return result;
    }
}