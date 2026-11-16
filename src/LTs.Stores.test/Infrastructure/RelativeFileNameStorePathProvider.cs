using LTs.Stores.Abstractions;

namespace LTs.Stores.test.Infrastructure;

internal class RelativeFileNameStorePathProvider<TEntity> : IStorePathProvider<TEntity>
    where TEntity : class
{
    private readonly string fileName;

    public RelativeFileNameStorePathProvider( string fileName )
        => this.fileName = fileName;

    public string GetPath( string storageKey )
        => fileName;
}