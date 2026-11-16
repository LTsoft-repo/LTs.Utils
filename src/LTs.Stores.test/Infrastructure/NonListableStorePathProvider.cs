using LTs.Stores.Abstractions;

namespace LTs.Stores.test.Infrastructure;

internal class NonListableStorePathProvider<TEntity> : IStorePathProvider<TEntity>
    where TEntity : class
{
    private readonly string storageRootDirectory;

    public NonListableStorePathProvider( string storageRootDirectory )
        => this.storageRootDirectory = storageRootDirectory;

    public string GetPath( string storageKey )
        => Path.Combine( storageRootDirectory, storageKey + ".json" );
}