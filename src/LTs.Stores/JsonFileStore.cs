using LTs.Json.Extensions;
using LTs.Stores.Abstractions;
using LTs.Stores.Infrastructure;

#pragma warning disable IDE0290

namespace LTs.Stores;

// ToDo: Move to LTs.Utils.

/// <summary>
///     <see cref="IStore{TEntity}" /> implementation that persists JSON entities as files on disk.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public class JsonFileStore<TEntity> : IStore<TEntity> where TEntity : class
{
    private const JsonStringOptions JsonOptions = JsonStringOptions.UseIndent;

    private readonly IStorePathProvider<TEntity> storePathProvider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonFileStore{TEntity}" /> class.
    /// </summary>
    /// <param name="storePathProvider">Maps storage keys to on-disk paths for this store instance.</param>
    public JsonFileStore( IStorePathProvider<TEntity> storePathProvider )
        => this.storePathProvider = storePathProvider;

    /// <inheritdoc />
    public async Task<TEntity?> ReadAsync( string key, CancellationToken cancellationToken )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var path = storePathProvider.GetPath( key );

        await using var gate = await FileGate.AcquireAsync( path, cancellationToken );

        return await ReadUnlockedAsync( path, cancellationToken );
    }

    /// <inheritdoc />
    public async Task WriteAsync( string key, TEntity entity, CancellationToken cancellationToken )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var path = storePathProvider.GetPath( key );

        await using var gate = await FileGate.AcquireAsync( path, cancellationToken );

        await WriteUnlockedAsync( path, entity, cancellationToken );
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TEntity>> ListAsync( CancellationToken cancellationToken )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var paths = GetListableEntityFilePaths();
        var entities = new List<TEntity>();

        foreach( var path in paths )
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var gate = await FileGate.AcquireAsync( path, cancellationToken );

            var entity = await ReadUnlockedAsync( path, cancellationToken );

            if( entity is not null )
            {
                entities.Add( entity );
            }
        }

        return entities;
    }

    /// <inheritdoc />
    public async Task DeleteAsync( string key, CancellationToken cancellationToken )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var path = storePathProvider.GetPath( key );

        await using var gate = await FileGate.AcquireAsync( path, cancellationToken );

        if( !File.Exists( path ) )
        {
            return;
        }

        File.Delete( path );

        var directoryPath = Path.GetDirectoryName( path );

        if( string.IsNullOrEmpty( directoryPath ) || !Directory.Exists( directoryPath ) )
        {
            return;
        }

        if( !Directory.EnumerateFileSystemEntries( directoryPath ).Any() )
        {
            Directory.Delete( directoryPath );
        }
    }

    private IEnumerable<string> GetListableEntityFilePaths()
    {
        if( storePathProvider is IListableStorePathProvider<TEntity> listable )
        {
            return listable.EnumerateEntityFilePaths();
        }

        throw new NotSupportedException(
            "Listing requires IStorePathProvider<TEntity> to implement IListableStorePathProvider<TEntity>." );
    }

    private static async Task<TEntity?> ReadUnlockedAsync( string path, CancellationToken cancellationToken )
    {
        if( !File.Exists( path ) )
        {
            return null;
        }

        var json = await File.ReadAllTextAsync( path, cancellationToken );

        return json.ParseAsJToken().ToObject<TEntity>();
    }

    private static async Task WriteUnlockedAsync( string path, TEntity entity, CancellationToken cancellationToken )
    {
        var directoryPath = Path.GetDirectoryName( path )
                            ?? throw new InvalidOperationException( "Store path has no directory." );

        Directory.CreateDirectory( directoryPath );

        var tempPath = path + ".tmp";
        var json = entity.ToJson( JsonOptions );

        await File.WriteAllTextAsync( tempPath, json, cancellationToken );

        File.Move( tempPath, path, true );
    }
}