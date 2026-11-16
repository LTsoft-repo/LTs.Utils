namespace LTs.Stores.Abstractions;

/// <summary>
///     Normalizes logical storage keys before path hashing and persistence.
/// </summary>
public interface IStorageKeyNormalizer
{
    /// <summary>
    ///     Normalizes <paramref name="storageKey" /> for stable document paths.
    /// </summary>
    /// <param name="storageKey">The logical storage key.</param>
    /// <returns>The normalized storage key.</returns>
    string Normalize( string storageKey );
}