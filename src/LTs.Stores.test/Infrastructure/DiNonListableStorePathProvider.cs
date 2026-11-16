using LTs.Stores.Abstractions;

namespace LTs.Stores.test.Infrastructure;

internal class DiNonListableStorePathProvider : IStorePathProvider<JsonFileStoreSampleEntity>
{
    public DiNonListableStorePathProvider()
    { }

    public string GetPath( string storageKey )
        => Path.Combine( TestProjectTempStorage.CreateUniqueDirectory(), storageKey + ".json" );
}