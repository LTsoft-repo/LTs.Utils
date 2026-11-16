using LTs.Stores.Abstractions;

#pragma warning disable IDE0290

namespace LTs.Stores.test.Infrastructure;

internal class RecordingStorageKeyNormalizer : IStorageKeyNormalizer
{
    private readonly Func<string, string> normalize;

    public RecordingStorageKeyNormalizer( Func<string, string>? normalize = null )
    {
        this.normalize = normalize ?? ( static key => key );
    }

    public string? LastStorageKey { get; private set; }

    public string Normalize( string storageKey )
    {
        LastStorageKey = storageKey;

        return normalize( storageKey );
    }
}