namespace LTs.DependencyInjections.test.Infrastructure;

internal class KeyedFactoryStorage : IStorage
{
    public KeyedFactoryStorage( object key )
    {
        Name = key.ToString() ?? string.Empty;
    }

    public string Name { get; }
}