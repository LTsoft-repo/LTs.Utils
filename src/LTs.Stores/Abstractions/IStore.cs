namespace LTs.Stores.Abstractions;

/// <summary>
///     Persists entities of type <typeparamref name="TEntity" /> by storage key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IStore<TEntity> where TEntity : class
{
    /// <summary>
    ///     Reads the entity stored under <paramref name="key" />.
    /// </summary>
    /// <param name="key">The storage key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The entity, or <see langword="null" /> when nothing is stored for <paramref name="key" />.</returns>
    Task<TEntity?> ReadAsync( string key, CancellationToken cancellationToken );

    /// <summary>
    ///     Writes an entity under <paramref name="key" /> (creates or replaces).
    /// </summary>
    /// <param name="key">The storage key.</param>
    /// <param name="entity">The entity to persist.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task WriteAsync( string key, TEntity entity, CancellationToken cancellationToken );

    /// <summary>
    ///     Lists all entities in this store instance.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Entities in enumeration order.</returns>
    Task<IReadOnlyList<TEntity>> ListAsync( CancellationToken cancellationToken );

    /// <summary>
    ///     Deletes the entity stored under <paramref name="key" />.
    /// </summary>
    /// <param name="key">The storage key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteAsync( string key, CancellationToken cancellationToken );
}