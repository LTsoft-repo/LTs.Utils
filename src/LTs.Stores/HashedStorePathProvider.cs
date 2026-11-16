using System.Security.Cryptography;
using System.Text;
using LTs.Stores.Abstractions;

#pragma warning disable IDE0290

namespace LTs.Stores;

/// <summary>
///     Maps storage keys to <c>{root}/{hash}/{fileName}</c> paths using a key normalizer and SHA-256 folder name.
/// </summary>
/// <typeparam name="TEntity">The entity type stored at those paths.</typeparam>
public class HashedStorePathProvider<TEntity> : IListableStorePathProvider<TEntity> where TEntity : class
{
    private readonly string storageRootDirectory;
    private readonly string fileName;
    private readonly IStorageKeyNormalizer storageKeyNormalizer;

    /// <summary>
    ///     Initializes a new instance of the <see cref="HashedStorePathProvider{TEntity}" /> class.
    /// </summary>
    /// <param name="storageRootDirectory">The root directory for hashed folders.</param>
    /// <param name="fileName">The file name inside each hash folder.</param>
    /// <param name="storageKeyNormalizer">Normalizes the storage key before hashing.</param>
    public HashedStorePathProvider( string storageRootDirectory,
                                    string fileName,
                                    IStorageKeyNormalizer storageKeyNormalizer )
    {
        this.storageRootDirectory = storageRootDirectory;
        this.fileName = fileName;
        this.storageKeyNormalizer = storageKeyNormalizer;
    }

    /// <inheritdoc />
    public string GetPath( string storageKey )
    {
        var normalizedKey = storageKeyNormalizer.Normalize( storageKey );
        var directoryName = CreateHashDirectoryName( normalizedKey );

        return Path.Combine( storageRootDirectory, directoryName, fileName );
    }

    /// <inheritdoc />
    public IEnumerable<string> EnumerateEntityFilePaths()
    {
        if( !Directory.Exists( storageRootDirectory ) )
        {
            return [ ];
        }

        return Directory.EnumerateFiles( storageRootDirectory, fileName, SearchOption.AllDirectories );
    }

    private static string CreateHashDirectoryName( string normalizedKey )
    {
        var hashBytes = SHA256.HashData( Encoding.UTF8.GetBytes( normalizedKey ) );

        return Convert.ToHexString( hashBytes );
    }
}