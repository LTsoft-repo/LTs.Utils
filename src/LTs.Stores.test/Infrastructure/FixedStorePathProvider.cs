using LTs.Stores.Abstractions;

namespace LTs.Stores.test.Infrastructure;

internal class FixedStorePathProvider<TEntity> : IListableStorePathProvider<TEntity>
    where TEntity : class, new()
{
    private readonly string storageRootDirectory;
    private readonly string fileExtension;

    public FixedStorePathProvider( string storageRootDirectory, string fileExtension = ".json" )
    {
        this.storageRootDirectory = storageRootDirectory;
        this.fileExtension = fileExtension;
    }

    public string GetPath( string storageKey )
        => Path.Combine( storageRootDirectory, storageKey + fileExtension );

    public IEnumerable<string> EnumerateEntityFilePaths()
    {
        if( !Directory.Exists( storageRootDirectory ) )
        {
            return [ ];
        }

        return Directory.EnumerateFiles( storageRootDirectory, "*" + fileExtension );
    }
}