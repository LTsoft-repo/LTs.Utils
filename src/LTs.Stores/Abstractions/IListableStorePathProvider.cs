namespace LTs.Stores.Abstractions;

/// <summary>
///     <see cref="IStorePathProvider{TEntity}" /> that can enumerate on-disk entity files for listing.
/// </summary>
/// <typeparam name="TEntity">The entity type stored at those paths.</typeparam>
public interface IListableStorePathProvider<TEntity> : IStorePathProvider<TEntity> where TEntity : class
{
    /// <summary>
    ///     Enumerates absolute paths to entity files managed by this provider.
    /// </summary>
    /// <returns>Entity file paths.</returns>
    IEnumerable<string> EnumerateEntityFilePaths();
}