using LTs.Stores.Abstractions;

namespace LTs.Stores.test.Infrastructure;

internal class DiFixedStorePathProvider : IListableStorePathProvider<JsonFileStoreSampleEntity>
{
    private readonly FixedStorePathProvider<JsonFileStoreSampleEntity> inner;

    public DiFixedStorePathProvider()
        => inner = new FixedStorePathProvider<JsonFileStoreSampleEntity>( TestProjectTempStorage.CreateUniqueDirectory() );

    public string GetPath( string storageKey )
        => inner.GetPath( storageKey );

    public IEnumerable<string> EnumerateEntityFilePaths()
        => inner.EnumerateEntityFilePaths();
}