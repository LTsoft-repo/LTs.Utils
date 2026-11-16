namespace LTs.Stores.Abstractions;

/// <summary>
///     Resolves absolute file paths for a store from a logical storage key.
/// </summary>
/// <typeparam name="TEntity">The entity type stored at those paths.</typeparam>
public interface IStorePathProvider<TEntity> where TEntity : class
{
    /// <summary>
    ///     Gets the absolute file path for <paramref name="storageKey" />.
    /// </summary>
    /// <param name="storageKey">The logical key (for example a working directory).</param>
    /// <returns>The absolute file path.</returns>
    string GetPath( string storageKey );
}